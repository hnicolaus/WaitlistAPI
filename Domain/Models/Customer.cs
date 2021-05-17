using Domain.Requests;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Customer
    {
        //EF requires empty ctor for model binding
        public Customer()
        {

        }

        public Customer(CreateCustomerRequest request)
        {
            FirstName = request.FirstName;
            LastName = request.LastName;
            Email = request.Email;

            IsPhoneNumberValidated = false;
            CreatedDateTime = DateTime.Now;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberValidated { get; set; }
        public string PhoneNumberValidationCode { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public virtual IEnumerable<Waitlist> Waitlists { get; set; } = new List<Waitlist>();
    }
}
