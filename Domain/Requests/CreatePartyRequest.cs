namespace Domain.Requests
{
    public class CreatePartyRequest
    {
        public string CustomerId { get; set; }
        public int PartySize { get; set; }

        public CreatePartyRequest(string customerId, int partySize)
        {
            CustomerId = customerId;
            PartySize = partySize;
        }
    }
}
