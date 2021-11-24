﻿using CashierDB.Connection;
using CashierDB.Model.Configure;
using CashierDB.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CashierDB.Model
{
    public class CashierContext : DbContext
    {
        public CashierContext() : base() {}
        public CashierContext(DbContextOptions options) : base(options) {}

        public DbSet<ProductDbDto> Products { get; set; }
        public DbSet<ItemDbDto> Items { get; set; }
        public DbSet<BillDbDto> Bills { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(new ConnectionStringConfiguration().ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BillStatusConfiguration());
        }
    }
}
