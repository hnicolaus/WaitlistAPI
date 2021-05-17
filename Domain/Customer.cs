namespace Domain
{
    public class Customer
    {
        public Customer(string userName, string phoneNumber)
        {
            UserName = userName;
            PhoneNumber = phoneNumber;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
    }
}
