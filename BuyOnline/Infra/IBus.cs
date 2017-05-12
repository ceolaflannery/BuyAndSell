using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyOnline.Events;

namespace BuyOnline.Infra
{
    public interface IBus
    {
        void Publish(CanNotOrder canNotOrder);
    }
}
