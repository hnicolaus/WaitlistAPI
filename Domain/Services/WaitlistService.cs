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

            var activeWaitlist = GetActiveWaitlist(customer.Id);
            if (activeWaitlist != null)
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

        private Waitlist GetActiveWaitlist(string customerId)
        {
            return GetWaitlists(customerId, isActive: true).SingleOrDefault();
        }
    }
}
