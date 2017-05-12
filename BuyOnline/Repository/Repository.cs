using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyOnline.Domain;
using BuyOnline.Events;

namespace BuyOnline.Repository
{
    public class Repository : IRepository
    {
        private Dictionary<int,User> Users { get; }
        private Dictionary<int, PlaceOrderScreenViewModel> PlaceOrderScreenViewModels { get; }

        public Repository()
        {
            Users = new Dictionary<int, User>();
        }

        public User GetUser(int id)
        {
            return Users[id];
        }

        public void UpdateUser(User updatedUser)
        {
            Users[updatedUser.Id].Balance = updatedUser.Balance;
        }

        public void Save(PlaceOrderScreenViewModel placeOrderScreenViewModel)
        {
            PlaceOrderScreenViewModels[placeOrderScreenViewModel.UserId] = placeOrderScreenViewModel;
        }
    }

    public class PlaceOrderScreenViewModel
    {
        public bool CanOrder { get; set; }
        public int UserId { get; set; }

        public PlaceOrderScreenViewModel(CanNotOrder canNotOrder)
        {
            if (!CanCreate(canNotOrder.UserId))
                throw  new Exception();

            CanOrder = false;
            UserId = canNotOrder.UserId;
        }

        public bool CanCreate(int userid)
        {
            return userid != 0;
        }
    }
}
