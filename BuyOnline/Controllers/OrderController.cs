using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BuyOnline.Domain;
using BuyOnline.Events;
using BuyOnline.Infra;
using BuyOnline.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BuyOnline.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IRepository _repository;
        private readonly IBus _bus;

        public OrderController(IRepository repository, IBus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        [HttpPost]
        public ObjectResult PlaceOrder(PlaceOrderRequest request)
        {
            try
            {
                var user = UpdateUserBalance(request);

                if (user.HasExceededBalance())
                    _bus.Publish(new CanNotOrder());

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private User UpdateUserBalance(PlaceOrderRequest request)
        {
            var user = _repository.GetUser(request.UserId);
            user.UpdateBalance(request.OrderAmount);
            _repository.UpdateUser(user);

            return user;
        }
    }

   
    public class PlaceOrderRequest
    {

        public PlaceOrderRequest(int userId, int orderAmount)
        {
            OrderAmount = orderAmount;
            UserId = userId;
        }

        public int UserId { get; set; }
        public int OrderAmount { get; set; }
    }
}
