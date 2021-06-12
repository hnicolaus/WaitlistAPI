using Domain.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq;
using DomainCustomer = Domain.Models.Customer;

namespace Api.Helpers
{
    public class Validate
    {
        public static void PatchCustomerFields(JsonPatchDocument<DomainCustomer> patchDoc, string[] allowedPatchFields)
        {
            var requestedFields = patchDoc.Operations.Select(o => o.path);
            var restrictedFields = requestedFields.Where(field => !allowedPatchFields.Contains(field));
            if (restrictedFields.Any())
            {
                throw new InvalidRequestException("Cannot update the following restricted field(s): " + string.Join(", ", restrictedFields));
            }
        }
        
        public static bool PhoneNumberFormat(string requestedPhoneNumber)
        {
            //TODO: Validate US phone number format.
            var result = true;
            if (result == false)
            {
                throw new InvalidRequestException("Phone number format is invalid");
            }

            return result ;
        }
    }
}