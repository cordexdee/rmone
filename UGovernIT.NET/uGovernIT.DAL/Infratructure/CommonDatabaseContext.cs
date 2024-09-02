using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility;

namespace uGovernIT.DAL.Infratructure
{
    public class CommonDatabaseContext : DbContext
    {
        CustomDbContext commonContext;

        public CommonDatabaseContext(CustomDbContext context)
        {
            this.commonContext = context;
        }

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.commonContext.DatabaseCommon);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
