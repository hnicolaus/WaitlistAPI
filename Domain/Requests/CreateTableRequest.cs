namespace Domain.Requests
{
    public class CreateTableRequest
    {
        public readonly int Number;
        public readonly int PartySize;

        public CreateTableRequest(
            int number,
            int partySize)
        {
            Number = number;
            PartySize = partySize;
        }
    }
}
