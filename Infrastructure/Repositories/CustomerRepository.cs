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

        public Customer GetCustomer(int customerId)
        {
            return _context.Customers.SingleOrDefault(customer => customer.Id == customerId);
        }

        public Customer GetCustomerByEmail(string email)
        {
            return _context.Customers.SingleOrDefault(customer => customer.Email == email);
        }

        public Customer GetCustomer(string phoneNumber)
        {
            return _context.Customers.SingleOrDefault(customer => customer.PhoneNumber.Equals(phoneNumber));
        }

        public void Add(Customer customer)
        {
            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                _context.Customers.Update(customer);
            }
        }

        public void SaveChanges()
        {
            _context.Save();
        }
    }
}