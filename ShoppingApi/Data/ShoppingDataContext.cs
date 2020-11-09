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
    }
}
