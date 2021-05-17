using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts
{
    public interface IWaitlistDbContext
    {
        DbSet<Waitlist> Waitlists { get; set; }
        DbSet<Customer> Customers { get; set; }

        void Save();
    }
}
