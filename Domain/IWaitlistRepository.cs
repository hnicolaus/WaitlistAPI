using System.Collections.Generic;

namespace Domain
{
    public interface IWaitlistRepository
    {
        IEnumerable<Waitlist> GetWaitlists(int customerId, bool? isActive);
        void Add(Waitlist waitlist);
        void Save();
        Customer GetCustomer(int customerId);
    }
}
