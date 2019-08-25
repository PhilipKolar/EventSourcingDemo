using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingDemo.Data.Domain;
using EventSourcingDemo.Data.DomainConfig;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingDemo.Data
{
    public class PkStoreContext : DbContext
    {
        public PkStoreContext(DbContextOptions<PkStoreContext> options) : base(options)
        {
            options = new DbContextOptions<PkStoreContext>();
        }

        public DbSet<Addon> Addons { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderEvent> OrderEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AddonConfig());
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new OrderStatusConfig());
            modelBuilder.ApplyConfiguration(new OrderEventConfig());
        }
    }
}
