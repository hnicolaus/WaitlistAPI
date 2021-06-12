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
        public DbSet<Admin> Admins { get; set; }

        public WaitlistDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new PartyMap());
            builder.ApplyConfiguration(new CustomerMap());
            builder.ApplyConfiguration(new TableMap());
            builder.ApplyConfiguration(new AdminMap());
        }

        public void Save() => base.SaveChanges();
    }
}
