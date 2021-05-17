using Domain;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class WaitlistRepository : IWaitlistRepository
    {
        private IWaitlistDbContext _context;

        public WaitlistRepository(IWaitlistDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Waitlist> GetWaitlists(string userName)
            => _context.Waitlists.AsQueryable().Where(waitlist => waitlist.UserName == userName).ToList();

        public void Add(Waitlist waitlist)
        {
            if (waitlist.Id == 0)
            {
                _context.Waitlists.Add(waitlist);
            }
            else
            {
                _context.Waitlists.Update(waitlist);
            }
        }

        public void Save()
        {
            _context.Save();
        }
    }
}
