using System.Collections.Generic;

namespace Domain
{
    public interface IWaitlistRepository
    {
        IEnumerable<Waitlist> GetWaitlists(string userName);
        void Add(Waitlist waitlist);
        void Save();
    }
}
