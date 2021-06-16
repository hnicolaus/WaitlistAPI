using System;
using DomainParty = Domain.Models.Party;

namespace Api.Models
{
    public class Party
    {
        public Party(DomainParty domainParty)
        {
            Id = domainParty.Id;
            CustomerId = domainParty.CustomerId;
            PartySize = domainParty.PartySize;
            CreatedDateTime = domainParty.CreatedDateTime;
            IsNotified = domainParty.IsNotified;
            IsActive = domainParty.IsActive;
        }

        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int PartySize { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsNotified { get; set; }
        public bool IsActive { get; set; }
    }
}
