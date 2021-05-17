namespace Domain.Models
{
    public class Table
    {
        public Table(int maxPartySize)
        {
            MaxPartySize = maxPartySize;
            IsAvailable = false;
        }

        public int Id { get; set; }
        public int MaxPartySize { get; set; }
        public bool IsAvailable { get; set; }
    }
}
