using System;

namespace Domain.Services
{
    public class InternalServiceException : Exception
    {
        public InternalServiceException(string message) : base(message) { }
    }
}
