using Microsoft.Extensions.DependencyInjection;
using ResourceAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ResourceAuth.Help;

namespace ResourceAuth.Jobs
{
    public class LotStatus : ILotsStatus
    {
        IServiceProvider _serviceProvider;
        ApplicationContext store;
        public LotStatus(ApplicationContext db,IServiceProvider _serviceProvider)
        {
            this.store = db;
            this._serviceProvider = _serviceProvider;
        }
        public void ChangeLotStatus()
        {

            try
            {



                using (TransactionScope transaction = new TransactionScope())
                {
                    List<int> statuses = new List<int>() { 0, 1, 2 };
                    var createdlots = store.lots.Where(x => statuses.Contains(x.status_id)).ToList();
                    foreach (var lot in createdlots)
                    {
                        if (lot.StartDate > DateTime.Now && lot.status_id != 2)
                        {
                            lot.status_id = 2;
                        }
                        else if (lot.StartDate < DateTime.Now && lot.status_id != 1)
                        {
                            lot.status_id = 1;
                        }
                        else if (lot.EndDate < DateTime.Now)
                            lot.status_id = 3;
                    }
                    store.SaveChanges();
                    transaction.Complete();
                }
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {

                        var rangeuser = from p in store.lots
                                        join d in store.orders on p.Id equals d.Slotid
                                        join k in store.accounts on d.Userid equals k.Id
                                        join o in store.accounts on p.user_id equals o.Id
                                        where p.status_id == 3
                                        orderby d.Userprice descending
                                        select new { slot = p, acc = k, seller = o };

                        foreach (var c in rangeuser)
                        {
                            var user = Email.EmailSend("vilbicos2000@gmail.com", "Ваш лот продан", $"В борьбе за ваш лот #{c.slot.Id} победил(a) {c.acc.Name}, email: {c.acc.Email}, mobile:+{c.acc.Mobile}", scope);
                            user.Wait();
                            var seller = Email.EmailSend("vilbicos2000@gmail.com", "Ваша ставка была наибольшей", $"Вашлот #{c.slot.Id} продавец {c.seller.Name}, email: {c.seller.Email}, mobile:+{c.seller.Mobile}", scope);
                            seller.Wait();
                            c.slot.status_id = 4;
                            break;
                        }

                    }
                    store.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
