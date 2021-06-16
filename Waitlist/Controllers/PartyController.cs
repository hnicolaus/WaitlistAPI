using Api.Authorization;
using Api.Helpers;
using Api.Models;
using Api.Requests;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
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
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        [MapException(typeof(NotAuthorizedException), HttpStatusCode.Unauthorized)]
        public IActionResult GetParties(string customerId, bool? isActive)
        {
            if (string.IsNullOrEmpty(customerId) && !isActive.HasValue)
            {
                throw new InvalidRequestException("No query string was provided.");
            }

            if (!string.IsNullOrEmpty(customerId))
            {
                Authorize.TokenAgainstResource(HttpContext.User, customerId);
            }

            var domainParties = _partyService.GetParties(customerId, isActive);

            var apiParties = domainParties.Select(domainParty => new Party(domainParty));
            return Ok(apiParties);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        [MapException(typeof(CustomerNotFoundException), HttpStatusCode.NotFound)]
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        [MapException(typeof(NotAuthorizedException), HttpStatusCode.Unauthorized)]
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
        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("{id}")]
        [MapException(typeof(PartyNotFoundException), HttpStatusCode.NotFound)]
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        [MapException(typeof(NotAuthorizedException), HttpStatusCode.Unauthorized)]
        public IActionResult UpdateParty(int id, [FromBody] JsonPatchDocument<DomainParty> patchDoc)
        {
            var domainParty = _partyService.GetParty(id);

            Authorize.TokenAgainstResource(HttpContext.User, domainParty.CustomerId);

            Validate<DomainParty>.EmptyPatchRequest(patchDoc);
            Validate<DomainParty>.PatchRestrictedFields(patchDoc, _allowedPatchFields);

            patchDoc.ApplyTo(domainParty, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isNotifyRequest = ValidateNotifyRequest(domainParty, patchDoc);
            _partyService.UpdateParty(domainParty, isNotifyRequest);

            return Ok();
        }

        private static bool ValidateNotifyRequest(DomainParty party, JsonPatchDocument<DomainParty> patchDoc)
        {
            var notifyPath = DomainParty.PropNameToPatchPath[nameof(DomainParty.IsNotified)];
            var notifyRequest = patchDoc.Operations.SingleOrDefault(o => o.path.Equals(notifyPath));
            var isNotifyRequest = notifyRequest != null;
            if (!isNotifyRequest)
            {
                return false;
            }

            var requestedIsNotified = Convert.ToBoolean(notifyRequest.value);
            if (requestedIsNotified == party.IsNotified)
            {
                patchDoc.Operations.Remove(notifyRequest);
                return false;
            }

            return true;
        }
    }
}