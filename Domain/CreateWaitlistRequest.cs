namespace Domain
{
    public class CreateWaitlistRequest
    {
        public string UserName { get; set; }
        public int PartySize { get; set; }

        public CreateWaitlistRequest(string userName, int partySize)
        {
            UserName = userName;
            PartySize = partySize;
        }
    }
}
