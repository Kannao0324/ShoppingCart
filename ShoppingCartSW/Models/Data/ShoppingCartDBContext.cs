using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ShoppingCartSW.Models.Data
{
    public class ShoppingCartDBContext : DbContext
    {
        // Pass the parameters on to the parent class
        public ShoppingCartDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingOrderLine> ShoppingOrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //In a live system you would eventually remove this section to avoid security issues
            //if someone saw your source code.
            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    Email = "admin@test.com",
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword("Password1")
                });
        }
    }
    
}
