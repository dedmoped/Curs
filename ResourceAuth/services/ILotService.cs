using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResourceAuth.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.services
{
    public interface ILotService
    {
        public IEnumerable<Lot> GetAllSlots(int id, int type, bool asc, int status);
        public void DeleteSlot(int id);
        public void ByeSlot(int slotid, int newprice,int UserID);
        public void AddUserSlot(List<IFormFile> pic, string slotinfo,int UserID);
    }
}
