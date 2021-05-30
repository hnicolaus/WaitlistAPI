using Domain.Services;

namespace Api.Helpers
{
    public class Validator
    {
        
        public static bool ValidatePhoneNumber(string phoneNumber)
        {
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