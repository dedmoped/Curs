using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ResourceAuth.Models;

namespace ResourceAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotsController : ControllerBase
    {
        private readonly ApplicationContext store;
        private int UserID => Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        static IFormFile file;
        static string str;
        public SlotsController(ApplicationContext store)
        {
            this.store = store;
        }
        [HttpGet]
        [Route("")]
        public IEnumerable<Slots> GetAllSlots()
        {
            return store.slots.ToList();
        }

        [HttpGet]
        [Route("rate/{id}")]
        public decimal GetRating(int id)
        {
            try
            {
                var retingbyid = store.rating.Where(d => d.Sellerid == id).Select(x => (int)x.Rate).ToArray();
                decimal summaryrating = 0;
                foreach (int c in retingbyid)
                {
                    summaryrating += c;
                }
                summaryrating = Math.Round(summaryrating / retingbyid.Count(), 1);
                return summaryrating;
            }
            catch
            {
                return 0;
            }
                
        }

        [HttpDelete]
        [Authorize(Roles = "user")]
        [Route("deleteslot/{id}")]
        public string DeleteSlot(int id)
        {
            try
            {
                store.orders.RemoveRange(store.orders.Where(b => b.Slotid == id));
                store.slots.RemoveRange(store.slots.Where(b => b.Id == id));
                store.SaveChanges();
                return "Удалено успешно";
            }
            catch
            {
                return "Ошибка удвления";
            }
        }

        [HttpPost]

        [Route("setrate/{myid}/{id}/{rt}")]
        public decimal Getuserrate(int myid,int id,int rt)
        {
            store.rating.RemoveRange(store.rating.Where(d => d.Sellerid == id && d.Userid==myid));
            store.rating.Add(new Rating() { Userid = myid, Sellerid = id, Rate = rt });
            store.SaveChanges();
            return rt;
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("byeslot/{slotid}/{newprice}")]
        public void ByeSlot(int slotid,int newprice)
        {
            store.orders.RemoveRange(store.orders.Where(d => d.Slotid==slotid && d.Userid == UserID));
            store.slots.Where(sl => sl.Id == slotid).FirstOrDefault().Price = newprice;
            store.orders.Add(new Orders() { Slotid = slotid, Userid = UserID, Userprice = newprice });
            store.SaveChanges();
        }

      
        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("addslot")]
        public void AddUserSlot([FromForm]IFormFile pic,[FromForm]string slotinfo)
        {
            
            file = pic;
            Slots add = JsonConvert.DeserializeObject<Slots>(slotinfo);
            var task = Task.Run((Func<Task>)SlotsController.Run);
            task.Wait();
           // store.orders.RemoveRange(store.orders.Where(d => d.Slotid == add.Id && d.Userid == UserID));
            string c = store.accounts.Where(b => b.Id == UserID).SingleOrDefault().Email;
            Slots newSlot = new Slots() {Description=add.Description,Seller=c,Price=add.Price,Sellerid=UserID,Imageurl=str};
            store.slots.Add(newSlot);
            store.SaveChanges();
        }

        static async Task Run()
        {
            using (var dbx = new DropboxClient("rPWXczyuKPcAAAAAAAAAATZo9acve3FbYnA1Fwol8Flroqb8HBX9iBapvJGePNud"))
            {
                //string file = "appsettings.json";
                string url = "";
                string folder = "/UploadPhoto";
                using (var memory = file.OpenReadStream())
                {
                    var upload = dbx.Files.UploadAsync(folder + "/" + file.FileName, WriteMode.Overwrite.Instance, body: memory);
                    upload.Wait();
                    try
                    {

                        var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync(folder + "/" + file.FileName);
                        tx.Wait();
                        url = tx.Result.Url;

                    }
                    catch {
                        var c = dbx.Sharing.ListSharedLinksAsync(folder + "/" + file.FileName);
                        c.Wait();
                        url = c.Result.Links[0].Url;
                    }
                    str = url.Remove(url.Length - 1) + '1';
                }
            }
        }
    }
}
