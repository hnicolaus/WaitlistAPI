using System;
using System.Runtime.Serialization;

namespace Domain.Services
{
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException() { }

        public InvalidRequestException(string message) : base(message) { }
    }
}