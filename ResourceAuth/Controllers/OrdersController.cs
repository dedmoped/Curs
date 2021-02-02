using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dropbox.Api;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResourceAuth.Models;
using Dropbox.Api.Files;
using Newtonsoft.Json;

namespace ResourceAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationContext store;
        private readonly SlotsStore slot;
        static IFormFile file1;
        private static string str;
        private int UserID =>Int32.Parse(User.Claims.Single(c=>c.Type==ClaimTypes.NameIdentifier).Value);
        public OrdersController(ApplicationContext store,SlotsStore slots)
        {
            this.store = store;
            this.slot = slots;
        }
        [HttpGet]
        [Authorize(Roles ="user")]
        [Route("")]
        public IActionResult GetOrders()
        {
            if (!store.orders.Where(b => b.Userid == UserID).Select(g=>g.Userid).Contains(UserID)) return Ok(Enumerable.Empty<Slots>());
            var c = store.orders.Where(b=>b.Userid==UserID);
            var sel = c.Select(f => f.Slotid);
            var orderedBooks = store.slots.Where(b => sel.Contains(b.Id));
            return Ok(orderedBooks);
        }

        [HttpGet]

        [Route("uspri/{userid}/{slotid}")]
        public string UserPrice(int userid,int slotid)
        {
            try
            {
                var price = store.orders.Where(d => d.Slotid == slotid && d.Userid == userid).FirstOrDefault().Userprice;
                return price.ToString();
            }
            catch
            {
                return "0";
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("add/{id}")]
        public void Add(int id)
        {
            var c = store.slots.Where(b => b.Id == id).FirstOrDefault();
            store.orders.Add(new Orders() {Slotid=c.Id,Userid=UserID,Userprice=0});
            store.SaveChanges();
        }

        [HttpGet]
        [Route("getmaxprice/{id}")]
        public IActionResult Getmaxprice(int id)
        {
            try
            {
                var c = store.orders.Where(b => b.Slotid == id);
                decimal price = 0;
                foreach (var pr in c)
                {
                    if (pr.Userprice >= price)
                        price = pr.Userprice;
                }
                var user = c.Where(f => f.Userprice == price).FirstOrDefault();
                string us = store.accounts.Where(h => h.Id == user.Userid).SingleOrDefault().Email;
                return Ok(new { username = us});
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Authorize(Roles = "user")]
        [Route("deleteorder/{id}")]
        public void DeleteOrder(int id)
        {
            store.orders.RemoveRange(store.orders.Where(d => d.Slotid == id && d.Userid == UserID));
            store.SaveChanges();
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("photo")]
        public string AddFoto([FromForm] IFormFile upload_file,[FromBody]Slots slots)
        {

            file1 = upload_file;
            var task = Task.Run((Func<Task>)OrdersController.Run);
            task.Wait();
            store.slots.Add(new Slots() { Description = slots.Description, Price = slots.Price, Seller = slots.Seller, Imageurl = str });
            return str;
            
        }
        [HttpGet]
        [Route("getslotbyid/{id}")]
        public IActionResult getSlotById(int id)
        {
            var orderedBooks = store.slots.Where(b => b.Id==id);
            return Ok(orderedBooks);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("update")]
        public void updateslot([FromForm] IFormFile pic, [FromForm] string slot)
        {

            Slots info = JsonConvert.DeserializeObject<Slots>(slot);
            var updatedata = store.slots.Where(b => b.Id == info.Id && b.Sellerid == UserID).FirstOrDefault();
            if (updatedata != null)
            {
                file1 = pic;
                if (file1 != null)
                {
                    var task = Task.Run((Func<Task>)OrdersController.Run);
                    task.Wait();
                    store.slots.Where(b => b.Id == info.Id).FirstOrDefault().Imageurl = str;
                }
                store.slots.Where(b => b.Id == info.Id && b.Sellerid == UserID).FirstOrDefault().Description = info.Description;
                store.slots.Where(b => b.Id == info.Id).FirstOrDefault().Price = info.Price;
                store.slots.Where(b => b.Id == info.Id).FirstOrDefault().Seller = info.Seller;
                store.SaveChanges();
            }
        }





        static async Task  Run()
        {
            using (var dbx = new DropboxClient("rPWXczyuKPcAAAAAAAAAATZo9acve3FbYnA1Fwol8Flroqb8HBX9iBapvJGePNud"))
            {
                //string file = "appsettings.json";
                string url = "";
                string folder = "/UploadPhoto";
                using (var memory = file1.OpenReadStream())
                {
                    var upload = dbx.Files.UploadAsync(folder + "/" + file1.FileName, WriteMode.Overwrite.Instance,body:memory);
                    upload.Wait();
                    try
                    {

                        var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync(folder + "/" + file1.FileName);
                        tx.Wait();
                        url = tx.Result.Url;

                    }
                    catch
                    {
                        var c = dbx.Sharing.ListSharedLinksAsync(folder + "/" + file1.FileName);
                        c.Wait();
                        url = c.Result.Links[0].Url;
                    }
                    str = url.Remove(url.Length-1)+'1';
                }
            }
        }

    }
}
