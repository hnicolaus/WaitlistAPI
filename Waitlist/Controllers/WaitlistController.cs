using Api.Authorization;
using Api.Models;
using Api.Requests;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using DomainWaitlist = Domain.Models.Waitlist;

namespace Api.Controllers
{
    [ApiController]
    [Route("waitlists")]
    public class WaitlistController : ControllerBase
    {
        public readonly WaitlistService _waitlistService;
        private readonly string[] _allowedPatchFields;

        public WaitlistController(WaitlistService waitlistService)
        {
            _waitlistService = waitlistService;
            _allowedPatchFields = new[] { "/isNotified" };
        }

        [Authorize]
        [HttpGet]
        [MapException(new[] { typeof(InvalidRequestException) }, new[] { HttpStatusCode.BadRequest })]
        public IActionResult GetWaitlists(string customerId, bool? isActive)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new InvalidRequestException("customerId query string must be specified.");
            }

            Authorize.TokenAgainstResource(HttpContext.User, customerId);

            var domainWaitlists = _waitlistService.GetWaitlists(customerId, isActive);

            var apiWaitlists = domainWaitlists.Select(domainWaitlist => new Waitlist(domainWaitlist));
            return Ok(apiWaitlists);
        }

        [Authorize]
        [HttpPost]
        [MapException(new[] { typeof(CustomerNotFoundException), typeof(InvalidRequestException) },
            new[] { HttpStatusCode.NotFound, HttpStatusCode.BadRequest })]
        public IActionResult CreateWaitlist([FromBody]CreateWaitlistRequest request)
        {
            if (string.IsNullOrEmpty(request.CustomerId))
            {
                throw new InvalidRequestException("CustomerId must be specified." );
            }

            Authorize.TokenAgainstResource(HttpContext.User, request.CustomerId);

            if (request.PartySize == 0)
            {
                throw new InvalidRequestException("PartySize must be greater than 0.");
            }

            var domainRequest = request.ToDomain();
            _waitlistService.CreateWaitlist(domainRequest);

            return Ok();
        }

        //NOTE: In the request header, clients have to specify Content-Type: application/json-patch+json.
        //Otherwise, JsonPatchDocument serialization will fail.
        [Authorize]
        [HttpPatch]
        [Route("{id}")]
        [MapException(new[] { typeof(WaitlistNotFoundException), typeof(InvalidRequestException) },
            new[] { HttpStatusCode.NotFound, HttpStatusCode.BadRequest })]
        public IActionResult UpdateWaitlist(int id, [FromBody] JsonPatchDocument<DomainWaitlist> patchDoc)
        {
            var domainWaitlist = _waitlistService.GetWaitlist(id);

            Authorize.TokenAgainstResource(HttpContext.User, domainWaitlist.CustomerId);

            if (patchDoc == null || !patchDoc.Operations.Any())
            {
                throw new InvalidRequestException("Request cannot be empty.");
            }

            var requestedFields = patchDoc.Operations.Select(o => o.path);
            var restrictedFields = requestedFields.Where(field => !_allowedPatchFields.Contains(field));
            if (restrictedFields.Any())
            {
                throw new InvalidRequestException("Cannot update the following restricted field(s): " + string.Join(", ", restrictedFields));
            }

            patchDoc.ApplyTo(domainWaitlist, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _waitlistService.SaveChanges();
            return Ok();
        }
    }
}