using Domain.Services;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq;
using DomainCustomer = Domain.Models.Customer;

namespace Api.Helpers
{
    public class Validate
    {
        
        public static bool UpdateCustomerPhoneNumber(JsonPatchDocument<DomainCustomer> patchDoc)
        {
            var customerPhoneNumberPath = "/phoneNumber";
            var updatePhoneNumberOperation = patchDoc.Operations.FirstOrDefault(o => o.path.Equals(customerPhoneNumberPath));
            if (updatePhoneNumberOperation == null)
            {
                return true;
            }

            var requestedPhoneNumber = updatePhoneNumberOperation.value.ToString();

            //TODO: Validate US phone number.
            var result = true;
            if (result == false)
            {
                throw new InvalidRequestException("Phone number format is invalid");
            }

            return result ;
        }
    }
}