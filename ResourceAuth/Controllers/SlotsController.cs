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
using ResourceAuth.Help;
using ResourceAuth.Models;
using ResourceAuth.services;

namespace ResourceAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotsController : ControllerBase
    {
        private readonly ApplicationContext store;
        private readonly IRatingService rating;
        private readonly ILotService lotService;
        private int UserID => Int32.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        static List<IFormFile> files;
        static List<string> str = new List<string>() { };
        public SlotsController(ApplicationContext store,IRatingService rating,ILotService lotService)
        {
            this.store = store;
            this.rating = rating;
            this.lotService = lotService;
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
        public IEnumerable<Lot> GetAllSlots(int id,int type,bool asc,int status)
        {
            return lotService.GetAllSlots(id, type, asc, status);
        }

        [HttpGet]
        [Route("rate/{id}")]
        public double GetRating(int id)
        {
           return rating.GetRating(id);                
        }

        [HttpDelete]
        [Authorize(Roles = "user")]
        [Route("deleteslot/{id}")]
        public IActionResult DeleteSlot(int id)
        {
            try
            {
                lotService.DeleteSlot(id);
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
           return rating.SetRating(sellerid, currentrate, UserID);
        }

        [HttpGet]
        [Authorize]
        [Route("getcurrentuserrate/{sellerid}")]
        public decimal getCurrentRating(int sellerid)
        {
          return rating.getCurrentRating(sellerid, UserID);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("byeslot/{slotid}/{newprice}")]
        public IActionResult ByeSlot(int slotid, int newprice)
        {
            try
            {
                lotService.ByeSlot(slotid, newprice, UserID);
                return Ok("Успешно");
            }
            catch
            {
                return BadRequest("Ошибка покупки");
            }
        }

        [HttpGet]
        [Route("Type")]
        public IEnumerable<LotTypes> LotType()
        {
            return store.lotTypes.ToList();
        }
      
        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("addslot")]
        public void AddUserSlot([FromForm]List<IFormFile> pic,[FromForm]string slotinfo)
        {
            lotService.AddUserSlot(pic, slotinfo, UserID);
        }

        static async Task Run()
        {
            using (var dbx = new DropboxClient("rPWXczyuKPcAAAAAAAAAATZo9acve3FbYnA1Fwol8Flroqb8HBX9iBapvJGePNud"))
            {
                foreach (var file in files)
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
                        catch
                        {
                            var c = dbx.Sharing.ListSharedLinksAsync(folder + "/" + file.FileName);
                            c.Wait();
                            url = c.Result.Links[0].Url;
                        }
                        str.Add(url.Remove(url.Length - 1) + '1');
                    }
                }
            }
        }
    }
}
