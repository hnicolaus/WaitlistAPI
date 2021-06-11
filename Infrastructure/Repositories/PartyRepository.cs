using Domain.Models;
using Domain.Repositories;
using Infrastructure.DbContexts;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private IWaitlistDbContext _context;

        public PartyRepository(IWaitlistDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Party> GetParties(string customerId, bool? isActive = null)
        {
            var parties = _context.Parties.AsQueryable();
            if (!string.IsNullOrEmpty(customerId))
            {
                parties = parties.Where(party => party.CustomerId == customerId);
            }
            if (isActive.HasValue)
            {
                parties = parties.Where(party => party.IsActive == isActive.Value);
            }

            return parties.ToList();
        }

        public void Add(Party party)
        {
            _context.Parties.Add(party);
        }

        public void Save()
        {
            _context.Save();
        }

        public Party GetParty(int partyId)
        {
            return _context.Parties.SingleOrDefault(party => party.Id == partyId);
        }
    }
}
