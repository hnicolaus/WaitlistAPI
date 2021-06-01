using Domain.Models;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface IWaitlistRepository
    {
        IEnumerable<Waitlist> GetWaitlists(string customerId, bool? isActive);
        void Add(Waitlist waitlist);
        void Save();
        Customer GetCustomer(string customerId);
        Waitlist GetWaitlist(int waitlistId);
    }
}
