using System;

namespace Domain.Exceptions
{
    public class PartyNotFoundException : Exception
    {
        public PartyNotFoundException() { }

        public PartyNotFoundException(int partyId) : base($"Party with ID {partyId} does not exist.") { }
    }
}