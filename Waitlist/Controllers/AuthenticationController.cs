using Api.Requests;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        [Route("google-user")]
        public IActionResult Authenticate(GoogleUserAuthenticationRequest request)
        {
            string jwtToken;
            _authenticationService.AuthenticateGoogleUser(request.IdToken, out jwtToken);

            return Ok(new { JwtToken = jwtToken });
        }

        [HttpPost]
        [MapException(typeof(AdminNotFoundException), HttpStatusCode.NotFound)]
        [Route("login-admin")]
        public IActionResult AdminLogin(AdminLoginRequest request)
        {
            _authenticationService.AdminLogin(request.Username, request.Password);

            return Ok();
        }

        [HttpPost]
        [MapException(typeof(AdminNotFoundException), HttpStatusCode.NotFound)]
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        [MapException(typeof(NotAuthorizedException), HttpStatusCode.Unauthorized)]
        [Route("verify-admin-login")]
        public IActionResult AuthenticateAdmin(AdminAuthenticationRequest request)
        {
            string jwtToken;
            _authenticationService.AuthenticateAdminUser(request.ToDomain(), out jwtToken);

            return Ok(new { JwtToken = jwtToken });
        }
    }
}
