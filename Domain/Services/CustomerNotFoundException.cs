using System;

namespace Domain.Services
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException() { }

        public CustomerNotFoundException(string customerId) : base($"Customer with ID {customerId} does not exist.") { }
    }
}