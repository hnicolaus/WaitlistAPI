using System;

namespace Domain
{
    public class CreateCustomerRequest
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string PhoneNumberValidationCode { get; set; }
    }
}