using System.ComponentModel.DataAnnotations;
using DomainCreateWaitlistRequest = Domain.Requests.CreateWaitlistRequest;

namespace Api.Requests
{
    public class CreateWaitlistRequest
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