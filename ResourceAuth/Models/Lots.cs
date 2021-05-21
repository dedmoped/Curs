using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Models
{
    public class Lots
    {
        public int Id { get; set;}

        public string Seller { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public string Imageurl { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int user_id { get; set; }
        public Accounts userid { get; set; }
        public int type_id { get; set; }
        public LotTypes typeid { get; set; }
        public int status_id { get; set; }
        public LotStatus statusid { get; set; }
    }
}
