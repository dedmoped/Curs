using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.services
{
    public interface IRatingService
    {
        public double GetRating(int id);
        public decimal SetRating(int sellerid, int currentrate,int UserID,int lotId);
        public decimal getCurrentRating(int sellerid,int UserID);
    }
}
