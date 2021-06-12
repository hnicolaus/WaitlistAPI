using System;

namespace Domain.Exceptions
{
    public class InternalServiceException : Exception
    {
        public InternalServiceException(string message) : base(message) { }
    }
}
