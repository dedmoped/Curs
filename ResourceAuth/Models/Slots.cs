using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Models
{
    public class Slots
    {
        public int Id { get; set;}

        public string Seller { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Imageurl { get; set; }

        public int Sellerid { get; set; }
    }
}
