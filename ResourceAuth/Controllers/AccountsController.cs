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
        public void UpdateAccount([FromForm]string accounts, [FromForm] IFormFile pic)
        {
            Accounts add = JsonConvert.DeserializeObject<Accounts>(accounts);
            SaveImage saveImage = new SaveImage(pic);
            var tast=saveImage.Run();
            tast.Wait();
            var database = db.accounts.Where(x => x.Id == UserID).FirstOrDefault();
            database.Email = add.Email;
            database.Description = add.Description;
            database.Password = add.Password;
            database.Name = add.Name;
            database.ImageUrl = saveImage.str;
            db.SaveChanges();
        } 
    }
}
