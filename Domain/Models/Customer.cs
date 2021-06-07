using Domain.Requests;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Customer
    {
        public static readonly Dictionary<string, string> PropNameToPatchPath = new Dictionary<string, string>
        {
            [nameof(Customer.Phone.PhoneNumber)] = "/phone/phoneNumber"
        };

        //EF requires empty ctor for model binding
        public Customer()
        {

        }

        public Customer(CreateCustomerRequest request)
        {
            Id = request.Id;
            FirstName = request.FirstName;
            LastName = request.LastName;
            Email = request.Email;

            Phone = new Phone
            {
                IsValidated = false
            };

            CreatedDateTime = DateTime.Now;
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Phone Phone { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public virtual IEnumerable<Waitlist> Waitlists { get; set; } = new List<Waitlist>();
    }

    public class Phone
    {
        public string PhoneNumber { get; set; }
        public bool IsValidated { get; set; }
        public string VerificationCode { get; set; }
    }
}
