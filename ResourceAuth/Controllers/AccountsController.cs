using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ResourceAuth.Help;
using ResourceAuth.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ResourceAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        ApplicationContext db;
        private int UserID => Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public AccountsController( ApplicationContext connect)
        {
            this.db = connect;
        }
        [HttpGet]
        [Route("Account")]
        public IEnumerable<Accounts> getAccount()
        {
            return db.accounts.Where(x => x.Id == UserID).ToList();
        }

        [HttpPost]
        [Route("Account")]
        public void UpdateAccount([FromForm] List<IFormFile> pic, [FromForm] string accounts)
        {

            Accounts add = JsonConvert.DeserializeObject<Accounts>(accounts);
            var database = db.accounts.Where(x => x.Id == UserID).FirstOrDefault();
            if (pic != null)
            {
                SaveImage saveImage = new SaveImage(pic);
                var tast = saveImage.Run();
                tast.Wait();
                database.ImageUrl = saveImage.str[0];
            }
            database.Email = add.Email;
            database.Description = add.Description;
            database.Password = add.Password;
            database.Name = add.Name;
            database.Mobile = add.Mobile;
            db.SaveChanges();
        }

        [HttpGet]
        [Route("Stat")]
        public IActionResult GetStatistic()
        {
          
            List<RatingStatistic> ratingStatistics = new List<RatingStatistic>();
            var users = db.rating.AsEnumerable().GroupBy(x => x.SellerId).ToList();
            foreach (var c in users)
            {
                var email = db.accounts.Where(x => x.Id == c.Key).FirstOrDefault().Email;
                var p = db.rating.Where(x=>x.SellerId==c.Key).Average(x=>x.Rate);
                ratingStatistics.Add(new RatingStatistic() { statistic = p, email = email, sellerId = 1 });
            }
            return Ok(ratingStatistics);
        }

        [HttpPost]
        [Route("phone")]
        public ActionResult SendSMS()
        {

                // Find your Account Sid and Auth Token at twilio.com/console
                const string accountSid = "ACc04d20d70dff669d6fe2b2ec3ea77e59";
                const string authToken = "aa5570beb374bbbb942ece3e75c40f38";

                TwilioClient.Init(accountSid, authToken);
                var to = new PhoneNumber("+375447032803");
            var from = new PhoneNumber("+1 725 444 8513");
                var message = MessageResource.Create(
                    to,
                    from:from, 
                    body: $"Это ваня, проверка сообщений");
            return Ok();
        }
    }
}
