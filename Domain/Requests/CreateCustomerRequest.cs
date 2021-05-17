namespace Domain.Requests
{
    public class CreateCustomerRequest
    {
        public readonly string Id;
        public readonly string FirstName;
        public readonly string LastName;
        public readonly string Email;

        public CreateCustomerRequest(
            string id, 
            string firstName,
            string lastName,
            string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}