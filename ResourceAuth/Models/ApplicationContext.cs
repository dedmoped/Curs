using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAuth.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Lots> lots { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<Accounts> accounts { get; set; }
        public DbSet<Rating> rating { get; set; }
        public DbSet<Role> roles { get; set; }
        //public DbSet<Order_History> orderHistory { get; set; }
        public DbSet<RecentlyLook> recentlyLooks { get; set; }
        public DbSet<LotType> lotTypes { get; set; }
        public DbSet<LotStatus> lotStatuses { get; set; }
    }
}
