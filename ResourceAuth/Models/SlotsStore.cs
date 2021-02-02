
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace ResourceAuth.Models
{
    public class SlotsStore
    {
       // ApplicationContext db;
     
        //public string getConnection()
        //{
        //    return ConfigurationSection.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;

        //}

        public Dictionary<int, int[]> Orders = new Dictionary<int, int[]>()
        {

            {1,new int[]{1,2} }
        };
    }
}
