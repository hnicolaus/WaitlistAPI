namespace Api
{
    public class CreateWaitlistRequest
    {
        public int CustomerId { get; set; }
        public int PartySize { get; set; }

        public Domain.CreateWaitlistRequest ToDomain()
        {
            return new Domain.CreateWaitlistRequest(CustomerId, PartySize);
        }
    }
}