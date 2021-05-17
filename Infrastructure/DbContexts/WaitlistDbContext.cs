using Domain.Models;
using Infrastructure.ModelMaps;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts
{
    public class WaitlistDbContext : DbContext, IWaitlistDbContext
    {
        public DbSet<Waitlist> Waitlists { get ; set; }
        public DbSet<Customer> Customers { get ; set; }

        public WaitlistDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new WaitlistMap());
            builder.ApplyConfiguration(new CustomerMap());
        }

        public void Save() => base.SaveChanges();
    }
}
