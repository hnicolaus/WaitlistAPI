using Domain.Repositories;
using Microsoft.Extensions.Options;
using System;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Infrastructure.Repositories
{
    public class TextMessageClient : ITextMessageClient
    {
        private readonly ITwilioRestClient _twilioClient;
        private readonly TwilioConfig _twilioConfig;

        public TextMessageClient(ITwilioRestClient twilioClient, IOptions<TwilioConfig> twilioConfig)
        {
            _twilioClient = twilioClient;
            _twilioConfig = twilioConfig.Value;
        }

        public void SendTextMessage(string phoneNumber, string message)
        {
            var messageResource = MessageResource.Create(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(_twilioConfig.PhoneNumber),
                body: message,
                client: _twilioClient);

            if (messageResource.ErrorCode.HasValue)
            {
                throw new Exception("SMS client error.");
            }
        }
    }
}
