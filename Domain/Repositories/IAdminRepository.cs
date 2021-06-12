using Domain.Models;

namespace Domain.Repositories
{
    public interface IAdminRepository
    {
        Admin GetAdmin(string userName, string password);
        void SaveChanges();
    }
}
