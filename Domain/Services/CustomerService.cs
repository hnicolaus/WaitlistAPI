using Domain.Models;
using Domain.Repositories;
using Domain.Requests;
using System;

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
            return _customerRepository.GetCustomer(customerId);
        }

        public Customer GetOrCreateCustomer(CreateCustomerRequest request)
        {
            if (request.Email == null || request.Email == "")
            {
                throw new Exception("Customer e-mail cannot be null or empty.");
            }

            var customer = _customerRepository.GetCustomer(request.Id);
            if (customer == null)
            {
                customer = new Customer(request);
                
                _customerRepository.Add(customer);
                SaveChanges();
            }

            return customer;
        }

        public void SaveChanges()
        {
            //This currently needs to be exposed for PATCH.
            _customerRepository.SaveChanges();
        }
    }
}
