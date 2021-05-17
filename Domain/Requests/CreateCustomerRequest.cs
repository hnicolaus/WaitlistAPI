namespace Domain.Requests
{
    public class CreateCustomerRequest
    {
        public readonly string FirstName;
        public readonly string LastName;
        public readonly string Email;
        public readonly string PhoneNumber;

        public CreateCustomerRequest(
            string firstName,
            string lastName,
            string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}