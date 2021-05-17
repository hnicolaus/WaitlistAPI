namespace Domain
{
    public class Waitlist
    {
        public Waitlist(string userName, int partySize)
        {
            UserName = userName;
            PartySize = partySize;
            CreatedDateTime = System.DateTime.Now;
            IsNotified = false;
            IsActive = true;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public int PartySize { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public bool IsNotified { get; set; }
        public bool IsActive { get; set; }
    }
}
