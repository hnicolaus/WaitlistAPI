namespace Domain
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(int customerId);
        void Add(Customer waitlist);
        void Save();
    }
}
