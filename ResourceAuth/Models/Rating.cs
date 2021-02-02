using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Models
{
    public class Rating
    {
      public  int Id { get; set; }
      public  int Sellerid { get; set; }
      public int Userid { get; set; }
      public  int Rate { get; set; } 
    }
}
