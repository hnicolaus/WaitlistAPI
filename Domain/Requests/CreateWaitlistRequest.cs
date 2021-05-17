namespace Domain.Requests
{
    public class CreateWaitlistRequest
    {
        public string CustomerId { get; set; }
        public int PartySize { get; set; }

        public CreateWaitlistRequest(string customerId, int partySize)
        {
            CustomerId = customerId;
            PartySize = partySize;
        }
    }
}
