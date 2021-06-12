using System.ComponentModel.DataAnnotations;

namespace Api.Requests
{
    public class AdminLoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}