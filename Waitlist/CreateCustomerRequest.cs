using System;

namespace Api
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

        public Domain.CreateCustomerRequest ToDomain()
        {
            return new Domain.CreateCustomerRequest
            {
                Address = Address,
                CreatedDateTime = CreatedDateTime,
                DateOfBirth = DateOfBirth,
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
                UserName = UserName,
            };
        }
    }
}