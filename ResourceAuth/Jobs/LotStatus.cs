using Microsoft.Extensions.DependencyInjection;
using ResourceAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace ResourceAuth.Jobs
{
    public class LotStatus : ILotsStatus
    {
        IServiceProvider _serviceProvider;
        public LotStatus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void ChangeLotStatus()
        {

            try
            {

                using (IServiceScope scope = _serviceProvider.CreateScope())
                using (var ctx = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
                {
                    using (var efscope = new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted}))
                    {
                        List<int> statuses = new List<int>() { 0, 1, 2 };
                        var createdlots = ctx.lots.Where(x => statuses.Contains(x.status_id));
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
                            else if (lot.EndDate < DateTime.Now && lot.status_id != 3)
                                lot.status_id = 3;
                        }
                        ctx.SaveChanges();
                        efscope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
