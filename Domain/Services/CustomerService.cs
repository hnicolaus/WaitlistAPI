using Domain.Exceptions;
using Domain.Helpers;
using Domain.Models;
using Domain.Repositories;
using Domain.Requests;
using System;

namespace Domain.Services
{
    public class CustomerService
    {
        private readonly int _verificationCodeExpiryMinutes = 2;

        public ICustomerRepository _customerRepository;
        public ITextMessageClient _textMessageClient;

        public CustomerService(ICustomerRepository waitlistRepository, ITextMessageClient textMessageClient)
        {
            _customerRepository = waitlistRepository;
            _textMessageClient = textMessageClient;
        }

        public Customer GetCustomer(string customerId)
        {
            var customer = _customerRepository.GetCustomer(customerId);
            if (customer == null)
            {
                throw new CustomerNotFoundException(customerId);
            }

            return customer;
        }

        public Customer CreateCustomer(CreateCustomerRequest request)
        {
            if (request.Email == null || request.Email == "")
            {
                throw new InvalidRequestException("Customer e-mail cannot be null or empty.");
            }

            var customer =  new Customer(request);
            _customerRepository.Add(customer);
            SaveChanges();

            return customer;
        }

        public void SaveChanges()
        {
            //This currently needs to be exposed for PATCH.
            _customerRepository.SaveChanges();
        }

        public void UpdateCustomer(Customer customer, bool isPhoneNumberRequest)
        {
            if (isPhoneNumberRequest)
            {
                UpdateVerificationCode(customer);
                SendPhoneVerificationSMS(customer);
            }

            SaveChanges();
        }

        public void SendPhoneVerification(string customerId)
        {
            var customer = GetCustomer(customerId);
            if (customer.Phone.IsVerified)
            {
                throw new InvalidRequestException("Phone number has been successfully verified.");
            }

            UpdateVerificationCode(customer);
            SaveChanges();

            SendPhoneVerificationSMS(customer);
        }

        private void SendPhoneVerificationSMS(Customer customer)
        {
            var message = "This message is sent on behalf of the Restaurant Waitlist. ";
            message += "Please verify your phone number using the following verification code, case-sensitive and valid for " + _verificationCodeExpiryMinutes + " minutes(s): " + customer.Phone.Verification.VerificationCode;

            _textMessageClient.SendTextMessage(customer.Phone.PhoneNumber, message);
        }

        private void UpdateVerificationCode(Customer customer)
        {
            var verificationCode = Helper.GenerateRandomString();
            customer.Phone.Verification = new SMSVerification(verificationCode, DateTime.Now.AddMinutes(_verificationCodeExpiryMinutes));
            customer.Phone.IsVerified = false;
        }

        public void VerifyPhoneNumber(string id, string verificationCode)
        {
            var customer = GetCustomer(id);

            if (customer.Phone == null || customer.Phone.PhoneNumber == null)
            {
                throw new InvalidRequestException($"Customer {id} has not provided a phone number.");
            }
            if (customer.Phone.IsVerified)
            {
                throw new InvalidRequestException($"Customer {id} has already verified their phone number.");
            }
            if (customer.Phone.Verification == null || customer.Phone.Verification.VerificationCode == null)
            {
                throw new InternalServiceException("Customer has provided a phone number, but has not received a verification code.");
            }
            if (!customer.Phone.Verification.IsValid(verificationCode))
            {
                throw new InvalidRequestException("Verification code does not match.");
            }

            customer.Phone.IsVerified = true;
            customer.Phone.Verification = null;
            SaveChanges();
        }
    }
}
