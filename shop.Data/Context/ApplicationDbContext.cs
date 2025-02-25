using System.Data;
using Microsoft.EntityFrameworkCore;
using Shop.Entities.Models;
namespace Shop.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // Define DbSet properties for your entities
        public DbSet<Category> Category { get; set; } // Another example
        public DbSet<Product> products { get; set; } // Another example

        
    }
}
