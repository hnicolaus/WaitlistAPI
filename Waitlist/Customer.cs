using System;

namespace Api
{
    public class Customer
    {
        public Customer(Domain.Customer domainCustomer)
        {
            Id = domainCustomer.Id;
            UserName = domainCustomer.UserName;
            FirstName = domainCustomer.FirstName;
            LastName = domainCustomer.LastName;
            Address = domainCustomer.Address;
            PhoneNumber = domainCustomer.PhoneNumber;
            DateOfBirth = domainCustomer.DateOfBirth;
            CreatedDateTime = domainCustomer.CreatedDateTime;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
