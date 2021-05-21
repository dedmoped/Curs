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
                store.lotTypes.Add(new LotTypes() { LotType = "Детские игрушки", Description = "Иформация об Детские игрушки" });
                store.lotTypes.Add(new LotTypes() { LotType = "Исскусство", Description = "Иформация о Исскусстве" });
                store.lotTypes.Add(new LotTypes() { LotType = "Книги", Description = "Иформация о Книгах" });
                store.lotTypes.Add(new LotTypes() { LotType = "Компьютерные игры", Description = "Иформация о компьютерах" });
                store.lotTypes.Add(new LotTypes() { LotType = "Монеты", Description = "Иформация о монетах" });
                store.lotTypes.Add(new LotTypes() { LotType = "Мобильные телефоны", Description = "Иформация о мобильных телефонах" });
                store.lotTypes.Add(new LotTypes() { LotType = "Спорт", Description = "Иформация о мобильных спорте" });
               
            }
            store.SaveChanges();
        }
        [HttpGet]
        [Route("lotList/{id}/{type}/{asc}/{status}")]
        public IEnumerable<Lots> GetAllSlots(int id,int type,bool asc,int status)
        {
            int skipedpages = id == 1 ? 0 : 5 * id;
            int takepages = id == 1 ? 10 : 5;
            IQueryable<Lots>lots=store.lots.Where(x=>x.status_id == status);
            if (type!=0)
            {
               lots= lots.Where(x => x.type_id == type);
            }
            lots = asc == false ? lots.OrderBy(x => x.Id).ThenByDescending(x=>x.EndDate): lots.OrderBy(x => x.Id).ThenBy(x=>x.EndDate);
            return lots.Skip(skipedpages).Take(takepages).ToList();
        }

        [HttpGet]
        [Route("rate/{id}")]
        public double GetRating(int id)
        {
            try
            {
                return store.rating.Where(d => d.SellerId == id).Average(x => x.Rate);
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
                store.lots.RemoveRange(store.lots.Where(b => b.Id == id));
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
            store.rating.RemoveRange(store.rating.Where(d => d.SellerId == sellerid && d.UserId==UserID));
            store.SaveChanges();
            store.rating.Add(new Rating() { UserId = UserID, SellerId = sellerid, Rate = currentrate });
            store.SaveChanges();
            return currentrate;
        }

        [HttpGet]
        [Authorize]
        [Route("getcurrentuserrate/{sellerid}")]
        public decimal getCurrentRating(int sellerid)
        {
            var rate = store.rating.Where(x => x.SellerId == sellerid && x.UserId == UserID);
            if (rate != null)
            {
                if(rate.FirstOrDefault()!=null)
                return rate.FirstOrDefault().Rate;
            }
            return 0;
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("byeslot/{slotid}/{newprice}")]
        public IActionResult ByeSlot(int slotid,int newprice)
        {
            try
            {
                store.orders.RemoveRange(store.orders.Where(d => d.Slotid == slotid && d.Userid == UserID));
                store.lots.Where(sl => sl.Id == slotid).FirstOrDefault().Cost = newprice;
                store.orders.Add(new Orders() { Slotid = slotid, Userid = UserID, Userprice = newprice });
                store.SaveChanges();
                return Ok("Успешно");
            }
            catch (Exception ex)
            {
                return BadRequest("Ошибка покупки");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("Type")]
        public IEnumerable<LotTypes> LotType()
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
            Lots newSlot = new Lots() {Description=add.Description,Seller=c,Cost=add.Cost,user_id=UserID,EndDate=add.EndDate,StartDate=add.StartDate,status_id=1,Title=add.Title,type_id=0,Imageurl=str};
            store.lots.Add(newSlot);
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
