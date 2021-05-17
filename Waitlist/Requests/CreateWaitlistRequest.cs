using DomainCreateWaitlistRequest = Domain.Requests.CreateWaitlistRequest;

namespace Api.Requests
{
    public class CreateWaitlistRequest
    {
        public int CustomerId { get; set; }
        public int PartySize { get; set; }

        public DomainCreateWaitlistRequest ToDomain()
        {
            return new DomainCreateWaitlistRequest(CustomerId, PartySize);
        }
    }
}