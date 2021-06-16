using System.ComponentModel.DataAnnotations;
using DomainAuthenticateAdminRequest = Domain.Requests.AuthenticateAdminRequest;

namespace Api.Requests
{
    public class AdminAuthenticationRequest : AdminLoginRequest
    {
        [Required]
        public string VerificationCode { get; set; }

        [Required]
        public string ClientId { get; set; }

        public DomainAuthenticateAdminRequest ToDomain()
        {
            return new DomainAuthenticateAdminRequest(
                Username,
                Password,
                VerificationCode,
                ClientId);
        }
    }
}