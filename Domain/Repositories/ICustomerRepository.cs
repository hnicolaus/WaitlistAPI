using Domain.Models;

namespace Domain.Repositories
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(int customerId);
        void Add(Customer waitlist);
        Customer GetCustomerByEmail(string email);
        void SaveChanges();
    }
}
