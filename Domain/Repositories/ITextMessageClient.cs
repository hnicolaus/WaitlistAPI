namespace Domain.Repositories
{
    public interface ITextMessageClient
    {
        public void SendTextMessage(string phoneNumber, string message);
    }
}
