using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public interface IWaitlistDbContext
    {
        DbSet<Waitlist> Waitlists { get; set; }
        DbSet<Customer> Customers { get; set; }

        void Save();
    }
}
