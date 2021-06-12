using Domain.Models;
using Domain.Repositories;
using Infrastructure.DbContexts;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private IWaitlistDbContext _context;

        public AdminRepository(IWaitlistDbContext context)
        {
            _context = context;
        }

        public Admin GetAdmin(string userName, string password)
        {
            return _context.Admins.SingleOrDefault(a => a.Username == userName && a.Password == password);
        }

        public void SaveChanges()
        {
            _context.Save();
        }
    }
}
