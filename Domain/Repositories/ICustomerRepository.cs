using Domain.Models;

namespace Domain.Repositories
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(string customerId);
        void Add(Customer waitlist);
        void SaveChanges();
    }
}
