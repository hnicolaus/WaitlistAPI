using Api.Models;
using Api.Requests;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

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
        [MapException(new[] { typeof(InvalidRequestException) }, new[] { HttpStatusCode.BadRequest })]
        public IActionResult GetWaitlists(string customerId, bool? isActive)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new InvalidRequestException("customerId query string must be specified.");
            }

            var domainWaitlists = _waitlistService.GetWaitlists(customerId, isActive);

            var apiWaitlists = domainWaitlists.Select(domainWaitlist => new Waitlist(domainWaitlist));
            return Ok(apiWaitlists);
        }

        [HttpPost]
        [MapException(new[] { typeof(CustomerNotFoundException), typeof(InvalidRequestException) },
            new[] { HttpStatusCode.NotFound, HttpStatusCode.BadRequest })]
        public IActionResult CreateWaitlist([FromBody]CreateWaitlistRequest request)
        {
            if (string.IsNullOrEmpty(request.CustomerId))
            {
                throw new InvalidRequestException("CustomerId must be specified." );
            }
            if (request.PartySize == 0)
            {
                throw new InvalidRequestException("PartySize must be greater than 0.");
            }

            var domainRequest = request.ToDomain();
            _waitlistService.CreateWaitlist(domainRequest);

            return Ok();
        }
    }
}