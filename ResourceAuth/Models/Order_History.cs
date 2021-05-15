using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Models
{
    public class Order_History
    {
        public int Id { get; set; }
        public int user_id { get; set; }
        public Accounts userid { get; set; }
        public string image { get; set; }
        public int status_id { get; set; }
        public LotStatus statusid { get; set; }
        public DateTime dateDeleted { get; set; }
        public int order_id { get; set; }
        public Orders orderid { get; set; }
        public int sellerId { get; set; }
    }
}
