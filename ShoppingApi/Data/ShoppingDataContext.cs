using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Data
{
    public class ShoppingDataContext : DbContext
    {
        public ShoppingDataContext(DbContextOptions<ShoppingDataContext> options):base(options)
        {

        }

        public IQueryable<Product> GetItemsInInventory()
        {
            return this.Products.Where(p => p.InInventory);
        }

        public IQueryable<Product> GetItemsFromCategory(string category)
        {
            return this.GetItemsInInventory().Where(p => p.Category == category);
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Category).HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(200);
            modelBuilder.Entity<Product>().Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
        }
    }
}
