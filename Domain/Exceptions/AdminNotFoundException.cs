using System;

namespace Domain.Exceptions
{
    public class AdminNotFoundException : Exception
    {
        public AdminNotFoundException() : base("Admin user not found.") { }
    }
}
