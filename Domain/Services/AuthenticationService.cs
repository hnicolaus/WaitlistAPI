using Domain.Exceptions;
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
        private readonly AdminService _adminService;

        public AuthenticationService(CustomerService customerService,
            TokenService tokenService,
            IOptions<Authentication> authenticationConfig,
            AdminService adminService)
        {
            _customerService = customerService;
            _tokenService = tokenService;
            _adminService = adminService;
            _authenticationConfig = authenticationConfig.Value;
        }

        public void AuthenticateGoogleUser(string googleIdToken, out string jwtToken)
        {
            Payload validatedPayload;
            if (!ValidateGoogleIdToken(googleIdToken, out validatedPayload))
            {
                throw new InvalidRequestException("ID token is invalid.");
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

            jwtToken = _tokenService.GetToken(customer.Id, validatedPayload.Audience.ToString(), TokenType.Customer);
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

        public void AdminLogin(string userName, string password)
        {
            var admin = _adminService.GetAdmin(userName, password);
            _adminService.UpdateAdminVerificationCode(admin);
            _adminService.SendLoginVerificationSMS(admin);
        }

        public void AuthenticateAdminUser(AuthenticateAdminRequest request, out string jwtToken)
        {
            var admin = _adminService.GetAdmin(request.Username, request.Password);

            _adminService.VerifyLoginVerification(admin, request.VerificationCode);

            if (!Array.Exists(_authenticationConfig.ClientIds, c => c == request.ClientId))
            {
                throw new NotAuthorizedException("Client ID is unauthorized.");
            }

            _adminService.ResetLoginVerification(admin);

            jwtToken = _tokenService.GetToken(admin.Id.ToString(), request.ClientId.ToString(), TokenType.Admin);
        }
    }
}
