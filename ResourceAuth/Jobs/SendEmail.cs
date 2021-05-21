using Microsoft.Extensions.DependencyInjection;
using ResourceAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResourceAuth.Help;
using System.Transactions;

namespace ResourceAuth.Jobs
{
    public class SendEmail : IEmailSender
    {
        IServiceProvider _serviceProvider;
        public SendEmail(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
     
        public async Task Send()
        {
            try
            {

                using (IServiceScope scope = _serviceProvider.CreateScope())
                using (var ctx = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
                {
                    using (var efscope = new TransactionScope(TransactionScopeOption.RequiresNew,
                       new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        var rangeuser = from p in ctx.lots
                                    join d in ctx.orders on p.Id equals d.Slotid
                                    join k in ctx.accounts on d.Userid equals k.Id
                                    join o in ctx.accounts on p.user_id equals o.Id
                                    where p.status_id == 3 orderby d.Userprice descending
                                    select new { slot = p, acc = k, seller = o };

                    foreach (var c in rangeuser)
                    {
                        await Email.EmailSend("vilbicos2000@gmail.com", "Ваш лот продан", $"В борьбе за ваш лот #{c.slot.Id} победил(a) {c.acc.Name}, email: {c.acc.Email}, mobile:+{c.acc.Mobile}", scope);
                        await Email.EmailSend("vilbicos2000@gmail.com", "Ваша ставка была наибольшей", $"Вашлот #{c.slot.Id} продавец {c.seller.Name}, email: {c.seller.Email}, mobile:+{c.seller.Mobile}", scope);
                        c.slot.status_id = 4;
                            break;
                    }
                        ctx.SaveChanges();
                        efscope.Complete();
                    }
                }
            
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
