using System;

namespace Api
{
    public class Waitlist
    {
        public Waitlist(Domain.Waitlist domainWaitlist)
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
