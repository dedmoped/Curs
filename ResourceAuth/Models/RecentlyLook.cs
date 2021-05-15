using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Models
{
    public class RecentlyLook
    {
        public int Id { get; set; }
        public DateTime dateLook { get; set; }
        public int lot_id { get; set; }
        public Lots lots { get; set; }
        public int user_id { get; set; }
        public Accounts user { get; set; }
    }
}
