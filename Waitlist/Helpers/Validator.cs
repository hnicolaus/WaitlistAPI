using System;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Api.Helpers
{
    public class Validator
    {
        public static bool ValidateGoogleUser(string googleIdToken, out GoogleUser user)
        {
            Payload payload;
            try
            {
                var audienceValidationSetting = new ValidationSettings
                {
                    //Validates the calling client is the one making the request (passing the id token on behalf of the user)
                    Audience = new[] { "407408718192.apps.googleusercontent.com" } //TODO: Replace Audience values with actual client value(s)! The Audience value is Google's OAuth 2.0 Playground (for testing only).
                };
                payload = ValidateAsync(googleIdToken, audienceValidationSetting).Result;
            }
            catch (Exception)
            {
                user = null;
                return false;
            }

            user = new GoogleUser(payload);
            return true;
        }

        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            //TODO: Validate US phone number.
            return true;
        }
    }

    public class GoogleUser
    {
        public GoogleUser(Payload validatedPayload)
        {
            FirstName = validatedPayload.GivenName;
            LastName = validatedPayload.FamilyName;
            UserId = validatedPayload.Subject;
            Email = validatedPayload.Email;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; }
        public string Email { get; set; }

    }
}