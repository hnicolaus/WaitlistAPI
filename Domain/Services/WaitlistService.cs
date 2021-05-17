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
        public IWaitlistRepository _waitlistRepository;

        public WaitlistService(IWaitlistRepository waitlistRepository)
        {
            _waitlistRepository = waitlistRepository;
        }

        public void CreateWaitlist(CreateWaitlistRequest request)
        {
            var customer = _waitlistRepository.GetCustomer(request.CustomerId);
            if (customer == null)
            {
                throw new Exception("Requested customer does not exist.");
            }

            var activeWaitlist = GetActiveWaitlist(request.CustomerId);
            if (activeWaitlist != null)
            {
                throw new Exception("User has an existing active waitlist");
            }
            
            var waitlist = new Waitlist(request.CustomerId, request.PartySize);
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
