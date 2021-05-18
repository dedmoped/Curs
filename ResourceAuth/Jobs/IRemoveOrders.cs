using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Jobs
{
    interface IRemoveOrders
    {
        void RemoveOrder();
        void Print();
    }
}
