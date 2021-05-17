using System;
using DomainWaitlist = Domain.Models.Waitlist;

namespace Api.Models
{
    public class Waitlist
    {
        public Waitlist(DomainWaitlist domainWaitlist)
        {
            Id = domainWaitlist.Id;
            CustomerId = domainWaitlist.CustomerId;
            PartySize = domainWaitlist.PartySize;
            CreatedDateTime = domainWaitlist.CreatedDateTime;
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PartySize { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
