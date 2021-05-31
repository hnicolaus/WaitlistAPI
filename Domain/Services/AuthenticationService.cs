using Domain.Models;
using Domain.Requests;
using Microsoft.Extensions.Options;
using System;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Domain.Services
{
    public class AuthenticationService
    {
        private readonly TokenService _tokenService;
        private readonly Authentication _authenticationConfig;
        private readonly CustomerService _customerService;

        public AuthenticationService(CustomerService customerService, TokenService tokenService, IOptions<Authentication> authenticationConfig)
        {
            _customerService = customerService;
            _tokenService = tokenService;
            _authenticationConfig = authenticationConfig.Value;
        }

        public bool AuthenticateGoogleUser(string googleIdToken, out string jwtToken)
        {
            Payload validatedPayload;
            if (!ValidateGoogleIdToken(googleIdToken, out validatedPayload))
            {
                jwtToken = null;
                return false;
            }

            var customerId = validatedPayload.Subject;
            Customer customer;
            try
            {
                customer = _customerService.GetCustomer(customerId);
            }
            catch (CustomerNotFoundException)
            {
                var createCustomerRequest = new CreateCustomerRequest(validatedPayload.Subject, validatedPayload.GivenName, validatedPayload.FamilyName, validatedPayload.Email);
                customer = _customerService.CreateCustomer(createCustomerRequest);
            }

            jwtToken = _tokenService.GetToken(customer.Id, validatedPayload.Audience.ToString());
            return true;
        }

        private bool ValidateGoogleIdToken(string googleIdToken, out Payload user)
        {
            Payload payload;
            try
            {
                var audienceValidationSetting = new ValidationSettings
                {
                    Audience = _authenticationConfig.ClientIds
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
