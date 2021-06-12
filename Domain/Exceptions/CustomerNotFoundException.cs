using System;

namespace Domain.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException() { }

        public CustomerNotFoundException(string customerId) : base($"Customer with ID {customerId} does not exist.") { }
    }
}