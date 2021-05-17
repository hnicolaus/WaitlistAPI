using System;

namespace Waitlist
{
    public class Waitlist
    {
        public Waitlist(Domain.Waitlist domainWaitlist)
        {
            Id = domainWaitlist.Id;
            UserName = domainWaitlist.UserName;
            PartySize = domainWaitlist.PartySize;
            CreatedDateTime = domainWaitlist.CreatedDateTime;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public int PartySize { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
