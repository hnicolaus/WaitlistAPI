using Domain.Models;
using Domain.Repositories;
using Domain.Requests;

namespace Domain.Services
{
    public class CustomerService
    {
        public ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository waitlistRepository)
        {
            _customerRepository = waitlistRepository;
        }

        public Customer GetCustomer(string customerId)
        {
            var customer = _customerRepository.GetCustomer(customerId);
            if (customer == null)
            {
                throw new CustomerNotFoundException(customerId);
            }

            return customer;
        }

        public Customer CreateCustomer(CreateCustomerRequest request)
        {
            if (request.Email == null || request.Email == "")
            {
                throw new InvalidRequestException("Customer e-mail cannot be null or empty.");
            }

            var customer =  new Customer(request);
            _customerRepository.Add(customer);
            SaveChanges();

            return customer;
        }

        public void SaveChanges()
        {
            //This currently needs to be exposed for PATCH.
            _customerRepository.SaveChanges();
        }
    }
}
