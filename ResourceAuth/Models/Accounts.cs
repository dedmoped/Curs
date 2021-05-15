using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Models
{
    public class Accounts
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Mobile { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }  
        public Role UserRoles { get; set; }
    }
}
