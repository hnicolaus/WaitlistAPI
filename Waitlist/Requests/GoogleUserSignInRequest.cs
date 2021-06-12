using System.ComponentModel.DataAnnotations;

namespace Api.Requests
{
    public class GoogleUserAuthenticationRequest
    {
        [Required]
        public string IdToken { get; set; }
    }
}