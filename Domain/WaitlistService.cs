using System;
using System.Linq;

namespace Domain
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
            if (request.UserName == null)
            {
                throw new Exception("UserName cannot be null");
            }

            var existingWaitlist = GetActiveWaitlist(request.UserName);
            if (existingWaitlist != null)
            {
                throw new Exception("User has an existing active waitlist");
            }
            

            var waitlist = new Waitlist(request.UserName, request.PartySize);
            _waitlistRepository.Add(waitlist);
            _waitlistRepository.Save();
        }

        public Waitlist GetActiveWaitlist(string userName)
        {
            return _waitlistRepository.GetWaitlists(userName)
                .SingleOrDefault(waitlist => waitlist.IsActive);
        }
    }
}
