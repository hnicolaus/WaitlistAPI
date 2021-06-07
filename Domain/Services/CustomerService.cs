using Domain.Models;
using Domain.Repositories;
using Domain.Requests;
using System;

namespace Domain.Services
{
    public class CustomerService
    {
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
                var verificationCode = GeneratePhoneVerificationCode();
                customer.Phone.VerificationCode = verificationCode;
                SendPhoneVerificationCode(customer.Phone.PhoneNumber, verificationCode);
            }

            SaveChanges();
        }

        private string GeneratePhoneVerificationCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
        }

        private void SendPhoneVerificationCode(string phoneNumber, string verificationCode)
        {
            var message = "This message is sent on behalf of the Restaurant Waitlist. ";
            message += "Please verify your phone number using the following verification code (case-sensitive): " + verificationCode;

            _textMessageClient.SendTextMessage(phoneNumber, message);
        }

        public void VerifyPhoneNumber(string id, string verificationCode)
        {
            var customer = GetCustomer(id);

            if (customer.Phone.PhoneNumber == null)
            {
                throw new InvalidRequestException($"Customer {id} has not provided a phone number.");
            }
            if (customer.Phone.VerificationCode == null && !customer.Phone.IsValidated)
            {
                throw new InternalServiceException("Customer has provided a phone number, but has not received a verification code.");
            }
            if (customer.Phone.VerificationCode != verificationCode)
            {
                throw new InvalidRequestException("Verification code does not match.");
            }

            customer.Phone.IsValidated = true;
            SaveChanges();
        }
    }
}
