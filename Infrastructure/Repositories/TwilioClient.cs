using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Http;

namespace Infrastructure.Repositories
{
    //https://www.twilio.com/blog/dependency-injection-twilio-sms-asp-net-core-2-1
    public class TwilioClient : ITwilioRestClient
    {
        private readonly TwilioConfig _twilioConfig;
        private readonly ITwilioRestClient _innerClient;


        public TwilioClient(IOptions<TwilioConfig> twilioConfig, System.Net.Http.HttpClient httpClient)
        {
            _twilioConfig = twilioConfig.Value;

            _innerClient = new TwilioRestClient(
                _twilioConfig.AccountSid,
                _twilioConfig.AuthToken,
                httpClient: new SystemNetHttpClient(httpClient));
        }

        public Response Request(Request request) => _innerClient.Request(request);
        public Task<Response> RequestAsync(Request request) => _innerClient.RequestAsync(request);
        public string AccountSid => _innerClient.AccountSid;
        public string Region => _innerClient.Region;
        public Twilio.Http.HttpClient HttpClient => _innerClient.HttpClient;
    }
}