using AdminPanel.Models.Identity;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
         protected override void OnModelCreating(ModelBuilder builder)
         {
             base.OnModelCreating(builder);
             builder.Entity<ClientBook>().HasNoKey();
           //      table => new {
           //  table.ClientId, table.Book
           //  });
         }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        public DbSet<ClientBook> ClientBooks { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
