namespace Domain
{
    public class CustomerService
    {
        public ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository waitlistRepository)
        {
            _customerRepository = waitlistRepository;
        }

        public void CreateCustomer(CreateCustomerRequest request)
        {
            ValidateCreateRequest(request);
            
            var waitlist = new Customer(request);
            _customerRepository.Add(waitlist);
            _customerRepository.Save();
        }

        private void ValidateCreateRequest(CreateCustomerRequest request)
        {
            //unique phoneNumber validation - Twilio send unique string to customer, persist code in Customer
            string validationCode = "placeholder";
            request.PhoneNumberValidationCode = validationCode;

            //unique userName validation
        }

        public Customer GetCustomer(int customerId)
        {
            return _customerRepository.GetCustomer(customerId);
        }
    }
}
