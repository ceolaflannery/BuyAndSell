using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyOnline.Repository;

namespace BuyOnline.Events
{
    public class CanNotOrderHandler
    {
        private readonly IRepository _repository;

        public CanNotOrderHandler(IRepository repository)
        {
            _repository = repository;
        }
        public void Handle(CanNotOrder canNotOrder)
        {
            _repository.Save(new PlaceOrderScreenViewModel(canNotOrder));
        }
    }
}
