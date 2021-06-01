using Domain.Models;
using Domain.Repositories;
using Domain.Requests;
using System;
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

        public Waitlist GetWaitlist(int waitlistId)
        {
            var waitlist = _waitlistRepository.GetWaitlist(waitlistId);
            if (waitlist == null)
            {
                throw new WaitlistNotFoundException(waitlistId);
            }

            return waitlist;
        }

        private Waitlist GetActiveWaitlist(string customerId)
        {
            return GetWaitlists(customerId, isActive: true).SingleOrDefault();
        }

        public void SaveChanges()
        {
            //This currently needs to be exposed for PATCH.
            _waitlistRepository.Save();
        }
    }
}
