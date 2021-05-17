using Api.Models;
using Api.Requests;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("waitlists")]
    public class WaitlistController : ControllerBase
    {
        public readonly WaitlistService _waitlistService;

        public WaitlistController(WaitlistService waitlistService)
        {
            _waitlistService = waitlistService;
        }

        [HttpGet]
        public IActionResult GetWaitlists(string customerId, bool? isActive)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return BadRequest(new { ErrorMessage = "customerId query string must be specified." });
            }

            var domainWaitlists = _waitlistService.GetWaitlists(customerId, isActive);

            var apiWaitlists = domainWaitlists.Select(domainWaitlist => new Waitlist(domainWaitlist));
            return Ok(apiWaitlists);
        }

        [HttpPost]
        public IActionResult CreateWaitlist([FromBody]CreateWaitlistRequest request)
        {
            if (string.IsNullOrEmpty(request.CustomerId))
            {
                return BadRequest(new { ErrorMessage = "CustomerId must be specified." });
            }
            if (request.PartySize == 0)
            {
                return BadRequest(new { ErrorMessage = "Invalid PartySize value." });
            }

            var domainRequest = request.ToDomain();
            _waitlistService.CreateWaitlist(domainRequest);

            return Ok();
        }
    }
}