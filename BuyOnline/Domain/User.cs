using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyOnline.Domain
{
    public class User
    {
        public User(int id, int balance)
        {
            Id = id;
            Balance = balance;
        }

        public int Id { get; }
        public int Balance { get; set; }

        public bool HasExceededBalance()
        {
            return Balance >= 100;
        }

        public void UpdateBalance(int orderAmount)
        {
            if (HasExceededBalance())
                throw new Exception("User has already exceeded the maximum balance");

            Balance += orderAmount;
        }
    }
}
