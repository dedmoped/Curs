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

        public DbSet<Slots> slots { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<Accounts> accounts { get; set; }
        public DbSet<Rating> rating { get; set; }
    }
}
