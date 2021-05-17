using System.ComponentModel.DataAnnotations;

namespace Api.Requests
{
    public class GoogleUserSignInRequest
    {
        [Required]
        public string IdToken { get; set; }
    }
}