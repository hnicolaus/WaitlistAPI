using Domain.Models;
using Domain.Repositories;
using Infrastructure.DbContexts;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class WaitlistRepository : IWaitlistRepository
    {
        private IWaitlistDbContext _context;

        public WaitlistRepository(IWaitlistDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Waitlist> GetWaitlists(string customerId, bool? isActive = null)
        {
            var waitlists = _context.Waitlists.AsQueryable()
                .Where(waitlist => waitlist.CustomerId == customerId);

            if (isActive != null)
            {
                waitlists.Where(waitlist => waitlist.IsActive == isActive.Value);
            }

            return waitlists.ToList();
        }

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

        public Customer GetCustomer(string customerId)
        {
            return _context.Customers.AsQueryable().SingleOrDefault(customer => customer.Id == customerId);
        }

        public Waitlist GetWaitlist(int waitlistId)
        {
            return _context.Waitlists.SingleOrDefault(waitlist => waitlist.Id == waitlistId);
        }
    }
}
