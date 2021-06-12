using Domain.Exceptions;
using Domain.Models;
using Domain.Repositories;
using Domain.Requests;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services
{
    public class PartyService
    {
        public CustomerService _customerService;
        public IPartyRepository _partyRepository;

        public PartyService(CustomerService customerService, IPartyRepository partyRepository)
        {
            _customerService = customerService;
            _partyRepository = partyRepository;
        }

        public void CreateParty(CreatePartyRequest request)
        {
            var customer = _customerService.GetCustomer(request.CustomerId);

            if (string.IsNullOrEmpty(customer.Phone.PhoneNumber))
            {
                throw new InvalidRequestException($"Customer {customer.Id} has not provided a phone number.");
            }
            if (!customer.Phone.IsValidated)
            {
                throw new InvalidRequestException($"Phone number for customer {customer.Id} has not been validated.");
            }

            var activeParty = GetParties(customer.Id, isActive: true);
            if (activeParty.Any())
            {
                throw new InvalidRequestException($"Customer {customer.Id} has an existing active party");
            }
            
            var party = new Party(request);
            _partyRepository.Add(party);
            _partyRepository.Save();
        }

        public IEnumerable<Party> GetParties(string customerId, bool? isActive)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new InvalidRequestException("customerId cannot be null or empty.");
            }

            return _partyRepository.GetParties(customerId, isActive);
        }

        public Party GetParty(int partyId)
        {
            var party = _partyRepository.GetParty(partyId);
            if (party == null)
            {
                throw new PartyNotFoundException(partyId);
            }

            return party;
        }

        public void SaveChanges()
        {
            //This currently needs to be exposed for PATCH.
            _partyRepository.Save();
        }
    }
}
