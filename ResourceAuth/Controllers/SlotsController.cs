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
            if (!store.lotTypes.Any())
            {
                store.lotTypes.Add(new LotType() { lotType = "Мобильные телефоны", Description = "Иформация о мобильных телефонах" });
                store.lotTypes.Add(new LotType() { lotType = "Автомобили", Description = "Иформация об автомобилях телефонах" });
                store.lotTypes.Add(new LotType() { lotType = "Ноутбуки", Description = "Иформация о мобильных телефонах" });
                store.lotTypes.Add(new LotType() { lotType = "Детские товары", Description = "Иформация о мобильных телефонах" });
                store.lotTypes.Add(new LotType() { lotType = "Игры", Description = "Иформация о мобильных телефонах" });
                store.lotTypes.Add(new LotType() { lotType = "Книги", Description = "Иформация о мобильных телефонах" });
                store.lotTypes.Add(new LotType() { lotType = "Спорт", Description = "Иформация о мобильных телефонах" });
                store.SaveChanges();
            }
        }
        [HttpGet]
        [Route("lotList/{id}")]
        public IEnumerable<Lots> GetAllSlots(int id)
        {
            int skipedpages = id == 1 ? 0 : 5*id;
            int takepages = id == 1 ? 10 : 5;
            return store.slots.OrderBy(x=>x.Id).Skip(skipedpages).Take(takepages).ToList();
        }

        [HttpGet]
        [Route("rate/{id}")]
        public double GetRating(int id)
        {
            try
            {
                return store.rating.Where(d => d.Sellerid == id).Average(x => x.Rate);
            }
            catch
            {
                return 0;
            }
                
        }

        [HttpDelete]
        [Authorize(Roles = "user")]
        [Route("deleteslot/{id}")]
        public IActionResult DeleteSlot(int id)
        {
            try
            {
                store.orders.RemoveRange(store.orders.Where(b => b.Slotid == id));
                store.slots.RemoveRange(store.slots.Where(b => b.Id == id));
                store.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]

        [Route("setrate/{sellerid}/{currentrate}")]
        public decimal Getuserrate(int sellerid, int currentrate)
        {
            store.rating.RemoveRange(store.rating.Where(d => d.Sellerid == sellerid && d.Userid==UserID));
            store.SaveChanges();
            store.rating.Add(new Rating() { Userid = UserID, Sellerid = sellerid, Rate = currentrate });
            store.SaveChanges();
            return currentrate;
        }

        [HttpGet]
        [Route("getcurrentuserrate/{sellerid}")]
        public decimal getCurrentRating(int sellerid)
        {
            return store.rating.Where(x => x.Sellerid == sellerid && x.Userid == UserID).FirstOrDefault().Rate;
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("byeslot/{slotid}/{newprice}")]
        public void ByeSlot(int slotid,int newprice)
        {
            store.orders.RemoveRange(store.orders.Where(d => d.Slotid==slotid && d.Userid == UserID));
            store.slots.Where(sl => sl.Id == slotid).FirstOrDefault().Cost = newprice;
            store.orders.Add(new Orders() { Slotid = slotid, Userid = UserID, Userprice = newprice });
            store.SaveChanges();
        }

        [HttpGet]
        [Authorize]
        [Route("Type")]
        public IEnumerable<LotType> LotType()
        {
            return store.lotTypes.ToList();
        }
      
        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("addslot")]
        public void AddUserSlot([FromForm]IFormFile pic,[FromForm]string slotinfo)
        {
            
            file = pic;
            Lots add = JsonConvert.DeserializeObject<Lots>(slotinfo);
            var task = Task.Run((Func<Task>)SlotsController.Run);
            task.Wait();
           // store.orders.RemoveRange(store.orders.Where(d => d.Slotid == add.Id && d.Userid == UserID));
            string c = store.accounts.Where(b => b.Id == UserID).SingleOrDefault().Email;
            Lots newSlot = new Lots() {Description=add.Description,Seller=c,Cost=add.Cost,user_id=UserID,EndDate=add.EndDate,StartDate=add.StartDate,status_id=add.status_id,Title=add.Title,type_id=add.type_id,Imageurl=str};
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
