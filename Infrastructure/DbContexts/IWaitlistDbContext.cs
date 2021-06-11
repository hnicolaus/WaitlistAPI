using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts
{
    public interface IWaitlistDbContext
    {
        DbSet<Party> Parties { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Table> Tables { get; set; }

        void Save();
    }
}
