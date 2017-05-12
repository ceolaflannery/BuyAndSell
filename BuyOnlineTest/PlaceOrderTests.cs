using System;
using BuyOnline.Controllers;
using BuyOnline.Domain;
using BuyOnline.Events;
using BuyOnline.Infra;
using BuyOnline.Repository;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Moq;
using Xunit;

namespace BuyOnlineTest
{
    public class PlaceOrderTests
    {
        [Fact]
        public void Should_accept_order_for_valid_user_and_order()
        {
            var repo = new Mock<IRepository>();
            var bus = new Mock<IBus>();
            var controller = new OrderController(repo.Object, bus.Object);
            var result = controller.PlaceOrder(new PlaceOrderRequest(111, 60));
            Assert.Equal(result.StatusCode, 200);
        }

        [Fact]
        public void Should_return_error_has_balance_of_over_100()
        {
            var user = new User(1234, 120);
            var repo = new Mock<IRepository>();
            var bus = new Mock<IBus>();
            var controller = new OrderController(repo.Object, bus.Object);

            var result = controller.PlaceOrder(new PlaceOrderRequest(user.Id , 60));
            Assert.Equal(result.StatusCode, 400);
            
        }

        [Fact]
        public void Should_update_balance_When_new_order_is_placed()
        {
            var user = new User(1234, 10);
            var repo = new Mock<IRepository>();
            repo.Setup(x => x.GetUser(It.IsAny<int>())).Returns(user);
            var bus = new Mock<IBus>();
            var controller = new OrderController(repo.Object, bus.Object);

            var result = controller.PlaceOrder(new PlaceOrderRequest(user.Id, 60));

            Assert.Equal(200, result.StatusCode);
            repo.Verify(x => x.UpdateUser(It.IsAny<User>()));
            bus.Verify(x => x.Publish(It.IsAny<CanNotOrder>()), Times.Never);
        }

        [Fact]
        public void Should_fire_balance_exceeded_event_When_new_order_causes_user_balance_to_exceede_100()
        {
            var user = new User(1234, 50);
            var repo = new Mock<IRepository>();
            repo.Setup(x => x.GetUser(It.IsAny<int>())).Returns(user);
            var bus = new Mock<IBus>();
            var controller = new OrderController(repo.Object, bus.Object);

            var result = controller.PlaceOrder(new PlaceOrderRequest(user.Id, 60));

            Assert.Equal(200, result.StatusCode);
            bus.Verify(x => x.Publish(It.IsAny<CanNotOrder>()));
        }

        [Fact]
        public void Should_return_bad_request_When_user_balance_has_already_been_exceeded()
        {
            var user = new User(1234, 120);
            var repo = new Mock<IRepository>();
            repo.Setup(x => x.GetUser(It.IsAny<int>())).Returns(user);
            var bus = new Mock<IBus>();
            var controller = new OrderController(repo.Object, bus.Object);

            var result = controller.PlaceOrder(new PlaceOrderRequest(user.Id, 60));

            Assert.Equal(400, result.StatusCode);
        }
    }
}
