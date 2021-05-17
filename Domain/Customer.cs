using System;
using System.Collections.Generic;

namespace Domain
{
    public class Customer
    {
        //EF requires empty ctor for model binding
        public Customer()
        {
        }

        public Customer(CreateCustomerRequest request)
        {
            UserName = request.UserName;
            FirstName = request.FirstName;
            LastName = request.LastName;
            Address = request.Address;
            DateOfBirth = request.DateOfBirth;

            PhoneNumber = request.PhoneNumber;
            PhoneNumberValidationCode = request.PhoneNumberValidationCode;
            IsPhoneNumberValidated = false;

            CreatedDateTime = DateTime.Now;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberValidated { get; set; }
        public string PhoneNumberValidationCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public virtual IEnumerable<Waitlist> Waitlists { get; set; } = new List<Waitlist>();
    }
}
