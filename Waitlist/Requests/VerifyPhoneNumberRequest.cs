using System.ComponentModel.DataAnnotations;

namespace Api.Requests
{
    public class VerifyPhoneNumberRequest
    {
        [Required]
        public string VerificationCode { get; set; }
    }
}
