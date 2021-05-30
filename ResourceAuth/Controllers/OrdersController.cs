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
using ResourceAuth.Help;
using ResourceAuth.services;

namespace ResourceAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationContext store;
        private readonly IOrdersService ordersService;
        private readonly SlotsStore slot;
        static IFormFile file1;
        private static string str;
        private int UserID =>Int32.Parse(User.Claims.Single(c=>c.Type==ClaimTypes.NameIdentifier).Value);
        public OrdersController(ApplicationContext store,SlotsStore slots,IOrdersService ordersService)
        {
            this.store = store;
            this.slot = slots;
            this.ordersService = ordersService;
        }
        [HttpGet]
        [Authorize(Roles ="user")]
        [Route("")]
        public IActionResult GetOrders()
        {
            //if (!store.orders.Where(b => b.Userid == UserID).Select(g=>g.Userid).Contains(UserID)) return Ok(Enumerable.Empty<Lots>());
            //var c = store.orders.Where(b=>b.Userid==UserID);
            //var sel = c.Select(f => f.Slotid);
            //var orderedBooks = store.lots.Where(b => sel.Contains(b.Id)).ToList();
            /////var user = store.orders.Where(d => sel.Contains(d.Slotid)).OrderByDescending(x=>x.Userprice).GroupBy(x => x.Slotid).ToList();
            ////var listprices = store.orders.Where(x => sel.Contains(x.Slotid)).ToList();
            //List<WhoTakeLot> whoTakeLots = new List<WhoTakeLot>();
            //var c1 = store.orders.Where(b => sel.Contains(b.Slotid));

            //foreach (var pr in orderedBooks)
            //{
            //    var uord = store.orders.Where(x => x.Userid == UserID && x.Slotid==pr.Id).FirstOrDefault();
            //    var ord=store.orders.Where(x => x.Slotid == pr.Id).OrderByDescending(x => x.Userprice).FirstOrDefault();
            //    var user = store.accounts.Where(x => x.Id == ord.Userid).FirstOrDefault();
            //    whoTakeLots.Add(new WhoTakeLot() { user = user, ordprice = uord });
            //}

            //return Ok(new { orders = orderedBooks, userprice = whoTakeLots});
            return ordersService.GetOrders(UserID);
        }


        [HttpGet]
        [Authorize(Roles = "user")]
        [Route("ord")]
        public IActionResult GetuserlotsOrders()
        {
            //if (!store.lots.Where(b => b.user_id == UserID).Select(g => g.user_id).Contains(UserID)) return Ok(Enumerable.Empty<Lots>());
            //var c = store.lots.Where(b => b.user_id == UserID);
            //var sel = c.Select(f => f.Id);
            //var orderedBooks = store.lots.Where(b => sel.Contains(b.Id)).ToList();
            /////var user = store.orders.Where(d => sel.Contains(d.Slotid)).OrderByDescending(x=>x.Userprice).GroupBy(x => x.Slotid).ToList();
            ////var listprices = store.orders.Where(x => sel.Contains(x.Slotid)).ToList();
            //List<WhoTakeLot> whoTakeLots = new List<WhoTakeLot>();
            //var c1 = store.orders.Where(b => sel.Contains(b.Slotid));

            //foreach (var pr in orderedBooks)
            //{
            //    var ord = store.orders.Where(x => x.Slotid == pr.Id).OrderByDescending(x => x.Userprice).FirstOrDefault();
            //    var user = ord != null?store.accounts.Where(x => x.Id == ord.Userid).FirstOrDefault():null;
            //    whoTakeLots.Add(new WhoTakeLot() { user = user, ordprice = ord });
            //}

            //return Ok(new { orders = orderedBooks, userprice = whoTakeLots });
            return ordersService.GetuserlotsOrders(UserID);
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
            var c = store.lots.Where(b => b.Id == id).FirstOrDefault();
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
            if (store.orders.Any(x => x.Slotid == id))
            {
                store.lots.Where(x => x.Id == id).FirstOrDefault().Cost = store.orders.Where(x => x.Slotid == id).OrderByDescending(x => x.Userprice).FirstOrDefault().Userprice;
            }
            store.SaveChanges();
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("photo")]
        public string AddFoto([FromForm] IFormFile upload_file,[FromBody]Lots slots)
        {

            file1 = upload_file;
            var task = Task.Run((Func<Task>)OrdersController.Run);
            task.Wait();
            slots.Imageurl = str;
            slots.user_id = UserID;
            store.lots.Add(slots);
            return str;
            
        }
        [HttpGet]
        [Route("getslotbyid/{id}")]
        public IActionResult getSlotById(int id)
        {
            var orderedBooks = store.lots.Where(b => b.Id==id && (b.status_id==2 || b.status_id==1)).Select(Lot.Create).ToList();
            return Ok(orderedBooks);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("update")]
        public IActionResult updateslot([FromForm] IFormFile pic, [FromForm] string slot)
        {
            try
            {
                Lots info = JsonConvert.DeserializeObject<Lots>(slot);
                var updatedata = store.lots.Where(b => b.Id == info.Id && b.user_id == UserID).FirstOrDefault();
                if (updatedata != null)
                {
                    file1 = pic;
                    if (file1 != null)
                    {
                        var task = Task.Run((Func<Task>)OrdersController.Run);
                        task.Wait();
                        store.lots.Where(b => b.Id == info.Id).FirstOrDefault().Imageurl = str;
                    }
                    store.lots.Where(b => b.Id == info.Id && b.user_id == UserID).FirstOrDefault().Description = info.Description;
                    store.lots.Where(b => b.Id == info.Id).FirstOrDefault().Cost = info.Cost;
                    store.lots.Where(b => b.Id == info.Id).FirstOrDefault().Seller = info.Seller;
                    store.SaveChanges();
                }
                return Ok("Обновление прошло успешно");
            }
            catch (Exception ex)
            {
                return BadRequest("Не удалось обновить лот");
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
