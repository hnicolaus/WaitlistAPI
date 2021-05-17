using Domain;
using Infrastructure.ModelMaps;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class WaitlistDbContext : DbContext, IWaitlistDbContext
    {
        public DbSet<Waitlist> Waitlists { get ; set; }

        public WaitlistDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new WaitlistMap());
        }

        public void Save() => base.SaveChanges();
    }
}
