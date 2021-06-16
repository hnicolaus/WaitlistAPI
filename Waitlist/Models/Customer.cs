using System;
using DomainCustomer = Domain.Models.Customer;

namespace Api.Models
{
    public class Customer
    {
        public Customer(DomainCustomer domainCustomer)
        {
            Id = domainCustomer.Id;
            FirstName = domainCustomer.FirstName;
            LastName = domainCustomer.LastName;
            Email = domainCustomer.Email;
            Phone = new Phone
            {
                PhoneNumber = domainCustomer.Phone.PhoneNumber,
                IsVerified = domainCustomer.Phone.IsVerified,
            };
            CreatedDateTime = domainCustomer.CreatedDateTime;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Phone Phone { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class Phone
    {
        public string PhoneNumber { get; set; }
        public bool IsVerified { get; set; }
    }
}
