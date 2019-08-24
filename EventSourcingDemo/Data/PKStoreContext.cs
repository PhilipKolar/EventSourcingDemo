using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingDemo.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingDemo.Data
{
    public class PkStoreContext : DbContext
    {
        public PkStoreContext(DbContextOptions<PkStoreContext> options) : base(options)
        {
        }

        public DbSet<Addon> Addons { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Addon>().ToTable(nameof(Addon));
            modelBuilder.Entity<Order>().ToTable(nameof(Order));
            modelBuilder.Entity<Product>().ToTable(nameof(Product));
            modelBuilder.Entity<OrderStatus>().ToTable(nameof(OrderStatus));
        }
    }
}
