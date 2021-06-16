using Domain.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq;

namespace Api.Helpers
{
    public class Validate<T> where T : class
    {
        public static void PatchRestrictedFields(JsonPatchDocument<T> patchDoc, string[] allowedPatchFields)
        {
            var requestedFields = patchDoc.Operations.Select(o => o.path);
            var restrictedFields = requestedFields.Where(field => !allowedPatchFields.Contains(field));
            if (restrictedFields.Any())
            {
                throw new InvalidRequestException("Cannot update the following restricted field(s): " + string.Join(", ", restrictedFields));
            }
        }

        public static void EmptyPatchRequest(JsonPatchDocument<T> patchDoc)
        {
            if (patchDoc == null || !patchDoc.Operations.Any())
            {
                throw new InvalidRequestException("Request cannot be empty.");
            }
        }
    }

    public static class Validate
    {
        public static void PhoneNumberFormat(string requestedPhoneNumber)
        {
            var result = true;
            if (string.IsNullOrEmpty(requestedPhoneNumber))
            {
                throw new InvalidRequestException("Phone number format is null or empty.");
            }

            //TODO: Validate US phone number format.
            if (result == false)
            {
                throw new InvalidRequestException("Phone number format is invalid");
            }
        }
    }
}