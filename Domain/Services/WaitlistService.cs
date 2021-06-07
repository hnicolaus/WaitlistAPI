using Domain.Models;
using Domain.Repositories;
using Domain.Requests;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services
{
    public class WaitlistService
    {
        public CustomerService _customerService;
        public IWaitlistRepository _waitlistRepository;

        public WaitlistService(CustomerService customerService, IWaitlistRepository waitlistRepository)
        {
            _customerService = customerService;
            _waitlistRepository = waitlistRepository;
        }

        public void CreateWaitlist(CreateWaitlistRequest request)
        {
            var customer = _customerService.GetCustomer(request.CustomerId);

            if (string.IsNullOrEmpty(customer.Phone.PhoneNumber))
            {
                throw new InvalidRequestException($"Customer {customer.Id} has not provided a phone number.");
            }
            if (!customer.Phone.IsValidated)
            {
                throw new InvalidRequestException($"Phone number for customer {customer.Id} has not been validated.");
            }

            var activeWaitlist = GetWaitlists(customer.Id, isActive: true);
            if (activeWaitlist.Any())
            {
                throw new InvalidRequestException($"Customer {customer.Id} has an existing active waitlist");
            }
            
            var waitlist = new Waitlist(request);
            _waitlistRepository.Add(waitlist);
            _waitlistRepository.Save();
        }

        public IEnumerable<Waitlist> GetWaitlists(string customerId, bool? isActive)
        {
            return _waitlistRepository.GetWaitlists(customerId, isActive);
        }

        public Waitlist GetWaitlist(int waitlistId)
        {
            var waitlist = _waitlistRepository.GetWaitlist(waitlistId);
            if (waitlist == null)
            {
                throw new WaitlistNotFoundException(waitlistId);
            }

            return waitlist;
        }

        public void SaveChanges()
        {
            //This currently needs to be exposed for PATCH.
            _waitlistRepository.Save();
        }
    }
}
