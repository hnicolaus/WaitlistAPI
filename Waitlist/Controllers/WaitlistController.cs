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
        public IActionResult GetWaitlists(int? customerId, bool? isActive)
        {
            if (customerId == null)
            {
                return BadRequest(new { ErrorMessage = "customerId query string must be specified" });
            }

            var domainWaitlists = _waitlistService.GetWaitlists(customerId.Value, isActive);

            var apiWaitlists = domainWaitlists.Select(domainWaitlist => new Waitlist(domainWaitlist));
            return Ok(apiWaitlists);
        }

        [HttpPost]
        public IActionResult CreateWaitlist([FromBody]CreateWaitlistRequest request)
        {
            var domainRequest = request.ToDomain();
            _waitlistService.CreateWaitlist(domainRequest);

            return Ok();
        }
    }
}