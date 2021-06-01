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
            IsNotified = domainWaitlist.IsNotified;
        }

        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int PartySize { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsNotified { get; set; }
    }
}
