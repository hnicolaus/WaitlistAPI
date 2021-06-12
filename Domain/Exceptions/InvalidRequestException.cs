using System;

namespace Domain.Exceptions
{
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException() { }

        public InvalidRequestException(string message) : base(message) { }
    }
}