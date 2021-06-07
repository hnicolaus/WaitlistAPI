using System;

namespace Domain.Services
{
    public class WaitlistNotFoundException : Exception
    {
        public WaitlistNotFoundException() { }

        public WaitlistNotFoundException(int waitlistId) : base($"Waitlist with ID {waitlistId} does not exist.") { }
    }
}