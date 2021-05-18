using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ResourceAuth.Models
{
    public class Order_History
    {
        public int Id { get; set; }
        public int user_id { get; set; }
        public Accounts userid { get; set; }
        public DateTime dateDeleted { get; set; }
        public string  lotinfo { get; set; }
    }
}
