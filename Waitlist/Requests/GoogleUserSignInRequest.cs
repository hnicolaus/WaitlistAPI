using System.ComponentModel.DataAnnotations;

namespace Api.Requests
{
    public class AuthenticationRequest
    {
        [Required]
        public string IdToken { get; set; }
    }
}