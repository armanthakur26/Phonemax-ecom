using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Phonemax.Models;
using Phonemax.Models.Twilio;

namespace Phonemax.dataaccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> categories { get; set; }
        public DbSet<Covertype> covertypes { get; set; }
        public DbSet<Processor> processor { get; set; }
        public DbSet<Product> products { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Applicationuser> applicationusers { get; set; }
        public DbSet<Shoppingcart> shoppingcarts { get; set; }
        public DbSet<Orderheader> orderheaders { get; set; }
        public DbSet<orderdetail> orderdetails { get; set; }
       
    }
}