using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models
{
    public class SmartSwitchDbContext : DbContext
    {
        public SmartSwitchDbContext(DbContextOptions<SmartSwitchDbContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }

    }
}
