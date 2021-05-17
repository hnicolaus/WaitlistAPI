namespace Waitlist
{
    public class CreateWaitlistRequest
    {
        public string UserName { get; set; }
        public int PartySize { get; set; }

        public Domain.CreateWaitlistRequest ToDomain()
        {
            return new Domain.CreateWaitlistRequest(UserName, PartySize);
        }
    }
}