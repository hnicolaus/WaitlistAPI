using Domain.Requests;
using System;
using static Google.Apis.Auth.GoogleJsonWebSignature;


namespace Domain.Services
{
    public class AuthenticationService
    {
        private readonly TokenService _tokenService;
        private readonly CustomerService _customerService;

        public AuthenticationService(CustomerService customerService, TokenService tokenService)
        {
            _customerService = customerService;
            _tokenService = tokenService;
        }

        public bool AuthenticateGoogleUser(string googleIdToken, out string jwtToken)
        {
            Payload validatedPayload;
            if (!ValidateGoogleIdToken(googleIdToken, out validatedPayload))
            {
                jwtToken = null;
                return false;
            }

            var createCustomerRequest = new CreateCustomerRequest(validatedPayload.Subject, validatedPayload.GivenName, validatedPayload.FamilyName, validatedPayload.Email);
            var customer = _customerService.GetOrCreateCustomer(createCustomerRequest);

            jwtToken = _tokenService.GetToken(customer.Id);
            return true;
        }

        private bool ValidateGoogleIdToken(string googleIdToken, out Payload user)
        {
            Payload payload;
            try
            {
                var audienceValidationSetting = new ValidationSettings
                {
                    //Back-end recognizes the Google Client ID of the front-end.
                    //Validate that the ID token's claim has the client ID of the front-end.
                    Audience = new[] { "407408718192.apps.googleusercontent.com" } //TODO: Replace Audience values with front-end's Client ID from Google!
                };
                payload = ValidateAsync(googleIdToken, audienceValidationSetting).Result;
            }
            catch (Exception)
            {
                user = null;
                return false;
            }

            user = payload;
            return true;
        }
    }
}
