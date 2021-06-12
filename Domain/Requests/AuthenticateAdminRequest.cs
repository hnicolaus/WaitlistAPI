namespace Domain.Requests
{
    public class AuthenticateAdminRequest
    {
        public readonly string Username;
        public readonly string Password;
        public readonly string AuthenticationCode;
        public readonly string ClientId;

        public AuthenticateAdminRequest(string username,
            string password,
            string authenticationCode,
            string clientId)
        {
            Username = username;
            Password = password;
            AuthenticationCode = authenticationCode;
            ClientId = clientId;
        }
    }
}
