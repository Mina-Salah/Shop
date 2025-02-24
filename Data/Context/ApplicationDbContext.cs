using Entity.Models;
using Microsoft.EntityFrameworkCore;
namespace Shop.Date
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // Define DbSet properties for your entities
        public DbSet<Category> Categories { get; set; } // Another example

        
    }
}
