using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ResourceAuth.Help;
using ResourceAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.services
{
    public class LotService : ILotService
    {
        ApplicationContext store;
        public LotService(ApplicationContext db)
        {
            this.store = db;
        }
        public void AddUserSlot(List<IFormFile> pic, string slotinfo,int UserID)
        {

                Lots lot = JsonConvert.DeserializeObject<Lots>(slotinfo);
                SaveImage saveImage = new SaveImage(pic);
                var load=saveImage.Run();
                load.Wait();
                // store.orders.RemoveRange(store.orders.Where(d => d.Slotid == add.Id && d.Userid == UserID));
                string c = store.accounts.Where(b => b.Id == UserID).SingleOrDefault().Email;
                Lots newSlot = new Lots() { Description = lot.Description, Seller = c, Cost = lot.Cost, user_id = UserID, EndDate = lot.EndDate, StartDate = lot.StartDate, status_id = 0, Title = lot.Title, type_id = lot.type_id, Imageurl = JsonConvert.SerializeObject(saveImage.str) };
                store.lots.Add(newSlot);
                store.SaveChanges();
        }

        public void ByeSlot(int slotid, int newprice,int UserID)
        {
           
                store.orders.RemoveRange(store.orders.Where(d => d.Slotid == slotid && d.Userid == UserID));
                store.lots.Where(sl => sl.Id == slotid).FirstOrDefault().Cost = newprice;
                store.orders.Add(new Orders() { Slotid = slotid, Userid = UserID, Userprice = newprice });
                store.SaveChanges();
           
        }

        public void DeleteSlot(int id)
        {
            store.orders.RemoveRange(store.orders.Where(b => b.Slotid == id));
            store.lots.RemoveRange(store.lots.Where(b => b.Id == id));
            store.SaveChanges();
        }

        public IEnumerable<Lot> GetAllSlots(int id, int type, bool asc, int status)
        {
            int skipedpages = id == 1 ? 0 : 5 * id;
            int takepages = id == 1 ? 10 : 5;
            IQueryable<Lots> lots = store.lots.Where(x => x.status_id == status);
            if (type != 0)
            {
                lots = lots.Where(x => x.type_id == type);
            }
            lots = asc == false ? lots.OrderBy(x => x.Id).ThenByDescending(x => x.EndDate) : lots.OrderBy(x => x.Id).ThenBy(x => x.EndDate);
            return lots.Skip(skipedpages).Take(takepages).Select(Lot.Create).ToList();
        }
    }
}
