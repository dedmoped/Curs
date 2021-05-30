using ResourceAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.services
{
    public class RatingService:IRatingService
    {
        ApplicationContext store;
        public RatingService(ApplicationContext db)
        {
            this.store = db;
        }

        public decimal getCurrentRating(int sellerid, int UserID)
        {
            var rate = store.rating.Where(x => x.SellerId == sellerid && x.UserId == UserID);
            if (rate != null)
            {
                if (rate.FirstOrDefault() != null)
                    return rate.FirstOrDefault().Rate;
            }
            return 0;
        }

        public double GetRating(int id)
        {
            try
            {
                return store.rating.Where(d => d.SellerId == id).Average(x => x.Rate);
            }
            catch
            {
                return 0;
            }
        }

        public decimal SetRating(int sellerid, int currentrate,int UserID)
        {
            store.rating.RemoveRange(store.rating.Where(d => d.SellerId == sellerid && d.UserId == UserID));
            store.SaveChanges();
            store.rating.Add(new Rating() { UserId = UserID, SellerId = sellerid, Rate = currentrate });
            store.SaveChanges();
            return currentrate;
        }
    }
}
