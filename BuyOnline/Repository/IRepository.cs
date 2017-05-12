using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyOnline.Domain;

namespace BuyOnline.Repository
{
    public interface IRepository
    {
        User GetUser(int id);
        void UpdateUser(User updatedUser);

        void Save(PlaceOrderScreenViewModel placeOrderScreenViewModel);
    }
}
