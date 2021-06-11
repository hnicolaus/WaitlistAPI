using System.ComponentModel.DataAnnotations;
using DomainCreateWaitlistRequest = Domain.Requests.CreatePartyRequest;

namespace Api.Requests
{
    public class CreatePartyRequest
    {
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public int PartySize { get; set; }

        public DomainCreateWaitlistRequest ToDomain()
        {
            return new DomainCreateWaitlistRequest(CustomerId, PartySize);
        }
    }
}