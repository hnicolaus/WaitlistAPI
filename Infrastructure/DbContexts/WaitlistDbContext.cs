using Domain.Models;
using Infrastructure.ModelMaps;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts
{
    public class WaitlistDbContext : DbContext, IWaitlistDbContext
    {
        public DbSet<Party> Parties { get ; set; }
        public DbSet<Customer> Customers { get ; set; }
        public DbSet<Table> Tables { get; set; }

        public WaitlistDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new PartyMap());
            builder.ApplyConfiguration(new CustomerMap());
            builder.ApplyConfiguration(new TableMap());
        }

        public void Save() => base.SaveChanges();
    }
}
