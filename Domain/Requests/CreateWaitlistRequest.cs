namespace Domain.Requests
{
    public class CreateWaitlistRequest
    {
        public int CustomerId { get; set; }
        public int PartySize { get; set; }

        public CreateWaitlistRequest(int customerId, int partySize)
        {
            CustomerId = customerId;
            PartySize = partySize;
        }
    }
}
