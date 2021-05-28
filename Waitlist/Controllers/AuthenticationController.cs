using Api.Requests;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        //Google sign-in is the only way to sign-up/sign-in.
        //Treat every successful Google sign-in as if they are an existing user.
        [HttpPost]
        [Route("google-user")]
        public IActionResult Authenticate(AuthenticationRequest request)
        {
            string jwtToken;
            if (!_authenticationService.AuthenticateGoogleUser(request.IdToken, out jwtToken))
            {
                return BadRequest("Google ID token is invalid.");
            }

            return Ok(new { JwtToken = jwtToken });
        }
    }
}
