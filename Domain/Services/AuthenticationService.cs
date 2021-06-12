using Domain.Exceptions;
using Domain.Helpers;
using Domain.Models;
using Domain.Repositories;
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
        private readonly IAdminRepository _adminRepository;
        public ITextMessageClient _textMessageClient;

        public AuthenticationService(CustomerService customerService,
            TokenService tokenService,
            IOptions<Authentication> authenticationConfig,
            IAdminRepository adminRepository,
            ITextMessageClient textMessageClient)
        {
            _customerService = customerService;
            _tokenService = tokenService;
            _adminRepository = adminRepository;
            _textMessageClient = textMessageClient;
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

        public void AdminLogin(string userName, string password)
        {
            var admin = _adminRepository.GetAdmin(userName, password);
            if (admin == null)
            {
                throw new AdminNotFoundException();
            }

            var authenticationCode = Helper.GenerateRandomString(length: 8);
            admin.AuthenticationCode = authenticationCode;
            _adminRepository.SaveChanges();

            _textMessageClient.SendTextMessage(admin.PhoneNumber, "Front-desk login code: " + authenticationCode);
        }

        public void AuthenticateAdminUser(AuthenticateAdminRequest request, out string jwtToken)
        {
            var admin = _adminRepository.GetAdmin(request.Username, request.Password);
            if (admin == null)
            {
                throw new AdminNotFoundException();
            }
            if (admin.AuthenticationCode != request.AuthenticationCode)
            {
                throw new InvalidRequestException("AuthenticationCode code invalid.");
            }
            if (!Array.Exists(_authenticationConfig.ClientIds, c => c == request.ClientId))
            {
                throw new NotAuthorizedException("Client ID is unauthorized.");
            }

            jwtToken = _tokenService.GetToken(admin.Id.ToString(), request.ClientId.ToString());

            admin.AuthenticationCode = null;
            _adminRepository.SaveChanges();
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
