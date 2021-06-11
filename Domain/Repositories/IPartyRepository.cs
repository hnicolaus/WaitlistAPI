using Domain.Models;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface IPartyRepository
    {
        IEnumerable<Party> GetParties(string customerId, bool? isActive);
        void Add(Party party);
        void Save();
        Party GetParty(int partyId);
    }
}
