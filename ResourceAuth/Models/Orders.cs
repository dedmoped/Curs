using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Models
{
    public class Orders
    {
        public int Id { get; set; }

        public int Userid { get; set; }

        public int Slotid { get; set; }

        public decimal Userprice { get; set; }
    }
}

