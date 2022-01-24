using CashierDB.Connection;
using CashierDB.Model.Configure;
using CashierDB.Model.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CashierDB.Model
{
    public class CashierContext : IdentityDbContext<UserDbDto>
    {
        public CashierContext() : base() { }
        public CashierContext(DbContextOptions options) : base(options) { }

        public DbSet<ProductDbDto> Products { get; set; }
        public DbSet<ItemDbDto> Items { get; set; }
        public DbSet<BillDbDto> Bills { get; set; }
        public DbSet<BillStatusDbDto> BillStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(new ConnectionStringConfiguration().ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new BillStatusConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
        }
    }
}
