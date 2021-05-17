using Domain.Models;
using Domain.Repositories;
using Infrastructure.DbContexts;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private IWaitlistDbContext _context;

        public CustomerRepository(IWaitlistDbContext context)
        {
            _context = context;
        }

        public Customer GetCustomer(string customerId)
        {
            return _context.Customers.SingleOrDefault(customer => customer.Id == customerId);
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public void SaveChanges()
        {
            _context.Save();
        }
    }
}