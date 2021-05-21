using Microsoft.Extensions.DependencyInjection;
using ResourceAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ResourceAuth.Jobs
{
    public class RemoveOrderJob : IRemoveOrders
    {
        IServiceProvider _serviceProvider;
        public RemoveOrderJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void RemoveOrder()
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            using (var ctx = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
            {
                
                ctx.lots.RemoveRange(ctx.lots.Where(x => x.EndDate == DateTime.Now));
                ctx.SaveChanges();
            }
        }
        public void Print()
        {
            //Console.WriteLine($"Hanfire recurring job!");
            //using (IServiceScope scope = _serviceProvider.CreateScope())
            //using (var ctx = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
            //{
              
            //    var range = ctx.lots.Where(x => x.EndDate < DateTime.Now);
            //    foreach(var c in range)
            //    {
            //        //c.status_id = 3;
            //    }
            //    ctx.SaveChanges();
            //}

        }
    }
}
