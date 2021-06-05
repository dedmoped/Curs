using Newtonsoft.Json;
using ResourceAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Help
{
    public class Lot
    {
        public int Id { get; set; }

        public string Seller { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public List<string> Imageurl { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int user_id { get; set; }
        public Accounts userid { get; set; }
        public int type_id { get; set; }
        public LotTypes typeid { get; set; }
        public int status_id { get; set; }
        public LotStatus statusid { get; set; }


        public static Lot Create(Lots _lot)
        {
            List<string> Imageurl = new List<string>();
            try
            {
                Imageurl = JsonConvert.DeserializeObject<List<string>>(_lot.Imageurl);
            }
            catch
            {
                Imageurl.Add(_lot.Imageurl);
            }
            var lot = new Lot
            {
                Cost = _lot.Cost,
                Imageurl = Imageurl,
                Id = _lot.Id,
                Description = _lot.Description,
                Seller = _lot.Seller,
                EndDate = _lot.EndDate,
                StartDate = _lot.StartDate,
                status_id = _lot.status_id,
                type_id = _lot.type_id,
                user_id = _lot.user_id,
                Title = _lot.Title
            };
            return lot;
        }
    }
}
