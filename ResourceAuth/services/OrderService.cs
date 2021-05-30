using Microsoft.AspNetCore.Mvc;
using ResourceAuth.Help;
using ResourceAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.services
{
    public class OrderService : IOrdersService
    {
        ApplicationContext store;
        public OrderService(ApplicationContext db)
        {
            this.store = db;
        }
        public IActionResult GetOrders(int UserID)
        {
            if (!store.orders.Where(b => b.Userid == UserID).Select(g => g.Userid).Contains(UserID)) return new OkObjectResult(Enumerable.Empty<Lots>());
            var c = store.orders.Where(b => b.Userid == UserID);
            var sel = c.Select(f => f.Slotid);
            var orderedBooks = store.lots.Where(b => sel.Contains(b.Id)).Select(Lot.Create).ToList();
            ///var user = store.orders.Where(d => sel.Contains(d.Slotid)).OrderByDescending(x=>x.Userprice).GroupBy(x => x.Slotid).ToList();
            //var listprices = store.orders.Where(x => sel.Contains(x.Slotid)).ToList();
            List<WhoTakeLot> whoTakeLots = new List<WhoTakeLot>();
            var c1 = store.orders.Where(b => sel.Contains(b.Slotid));

            foreach (var pr in orderedBooks)
            {
                var uord = store.orders.Where(x => x.Userid == UserID && x.Slotid == pr.Id).FirstOrDefault();
                var ord = store.orders.Where(x => x.Slotid == pr.Id).OrderByDescending(x => x.Userprice).FirstOrDefault();
                var user = store.accounts.Where(x => x.Id == ord.Userid).FirstOrDefault();
                whoTakeLots.Add(new WhoTakeLot() { user = user, ordprice = uord });
            }

            return new  OkObjectResult (new { orders = orderedBooks, userprice = whoTakeLots });
        }

        public IActionResult GetuserlotsOrders(int UserID)
        {
            if (!store.lots.Where(b => b.user_id == UserID).Select(g => g.user_id).Contains(UserID)) return new OkObjectResult(Enumerable.Empty<Lots>());
            var c = store.lots.Where(b => b.user_id == UserID);
            var sel = c.Select(f => f.Id);
            var orderedBooks = store.lots.Where(b => sel.Contains(b.Id)).Select(Lot.Create).ToList();
            ///var user = store.orders.Where(d => sel.Contains(d.Slotid)).OrderByDescending(x=>x.Userprice).GroupBy(x => x.Slotid).ToList();
            //var listprices = store.orders.Where(x => sel.Contains(x.Slotid)).ToList();
            List<WhoTakeLot> whoTakeLots = new List<WhoTakeLot>();
            var c1 = store.orders.Where(b => sel.Contains(b.Slotid));

            foreach (var pr in orderedBooks)
            {
                var ord = store.orders.Where(x => x.Slotid == pr.Id).OrderByDescending(x => x.Userprice).FirstOrDefault();
                var user = ord != null ? store.accounts.Where(x => x.Id == ord.Userid).FirstOrDefault() : null;
                whoTakeLots.Add(new WhoTakeLot() { user = user, ordprice = ord });
            }

            return new OkObjectResult(new { orders = orderedBooks, userprice = whoTakeLots });
        }
    }
}
