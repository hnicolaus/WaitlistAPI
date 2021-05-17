using Domain;
using System.Linq;

namespace Infrastructure
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

        public void Save()
        {
            _context.Save();
        }
    }
}