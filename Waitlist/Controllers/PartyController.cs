using Api.Authorization;
using Api.Models;
using Api.Requests;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using DomainParty = Domain.Models.Party;

namespace Api.Controllers
{
    [ApiController]
    [Route("parties")]
    public class PartyController : ControllerBase
    {
        public readonly PartyService _partyService;
        private readonly string[] _allowedPatchFields;

        public PartyController(PartyService partyService)
        {
            _partyService = partyService;
            _allowedPatchFields = DomainParty.PropNameToPatchPath.Values.ToArray();
        }

        [Authorize]
        [HttpGet]
        [MapException(new[] { typeof(InvalidRequestException) }, new[] { HttpStatusCode.BadRequest })]
        public IActionResult GetParties(string customerId, bool? isActive)
        {
            if (string.IsNullOrEmpty(customerId) && !isActive.HasValue)
            {
                throw new InvalidRequestException("No query string was provided.");
            }

            Authorize.TokenAgainstResource(HttpContext.User, customerId);

            var domainParties = _partyService.GetParties(customerId, isActive);

            var apiParties = domainParties.Select(domainParty => new Party(domainParty));
            return Ok(apiParties);
        }

        [Authorize]
        [HttpPost]
        [MapException(new[] { typeof(CustomerNotFoundException), typeof(InvalidRequestException) },
            new[] { HttpStatusCode.NotFound, HttpStatusCode.BadRequest })]
        public IActionResult CreateParty([FromBody]CreatePartyRequest request)
        {
            Authorize.TokenAgainstResource(HttpContext.User, request.CustomerId);

            if (request.PartySize <= 0)
            {
                throw new InvalidRequestException("PartySize must be greater than 0.");
            }

            var domainRequest = request.ToDomain();
            _partyService.CreateParty(domainRequest);

            return Ok();
        }

        //NOTE: In the request header, clients have to specify Content-Type: application/json-patch+json.
        //Otherwise, JsonPatchDocument serialization will fail.
        [HttpPatch]
        [Route("{id}")]
        [MapException(new[] { typeof(PartyNotFoundException), typeof(InvalidRequestException) },
            new[] { HttpStatusCode.NotFound, HttpStatusCode.BadRequest })]
        public IActionResult UpdateParty(int id, [FromBody] JsonPatchDocument<DomainParty> patchDoc)
        {
            var domainParty = _partyService.GetParty(id);

            Authorize.TokenAgainstResource(HttpContext.User, domainParty.CustomerId);

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

            patchDoc.ApplyTo(domainParty, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _partyService.SaveChanges();
            return Ok();
        }
    }
}