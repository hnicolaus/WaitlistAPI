using Api.Authorization;
using Api.Helpers;
using Api.Models;
using Api.Requests;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using DomainCustomer = Domain.Models.Customer;

namespace Api.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        public readonly CustomerService _customerService;
        private string[] _allowedPatchFields;

        public CustomerController(CustomerService waitlistService)
        {
            _customerService = waitlistService;
            _allowedPatchFields = DomainCustomer.PropNameToPatchPath.Values.ToArray();
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        [MapException(new[] { typeof(CustomerNotFoundException), typeof(InvalidRequestException) },
            new[] { HttpStatusCode.NotFound, HttpStatusCode.BadRequest })]
        public IActionResult GetCustomer(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidRequestException("Id cannot be empty.");
            }

            Authorize.TokenAgainstResource(HttpContext.User, id);

            var domainCustomer = _customerService.GetCustomer(id);

            var result = new Customer(domainCustomer);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("{id}/phone-number/verify")]
        [MapException(new[] { typeof(CustomerNotFoundException), typeof(InvalidRequestException), typeof(InternalServiceException) },
            new[] { HttpStatusCode.NotFound, HttpStatusCode.BadRequest, HttpStatusCode.InternalServerError })]
        public IActionResult VerifyPhoneNumber(string id, [FromBody] VerifyPhoneNumberRequest request)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidRequestException("Id cannot be empty.");
            }

            Authorize.TokenAgainstResource(HttpContext.User, id);

            _customerService.VerifyPhoneNumber(id, request.VerificationCode);

            return Ok();
        }

        //NOTE: In the request header, clients have to specify Content-Type: application/json-patch+json.
        //Otherwise, JsonPatchDocument serialization will fail.
        [Authorize]
        [HttpPatch]
        [Route("{id}")]
        [MapException(new[] { typeof(CustomerNotFoundException), typeof(InvalidRequestException) },
            new[] { HttpStatusCode.NotFound, HttpStatusCode.BadRequest })]
        public IActionResult UpdateCustomer(string id, [FromBody] JsonPatchDocument<DomainCustomer> patchDoc)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidRequestException("Id cannot be empty.");
            }

            Authorize.TokenAgainstResource(HttpContext.User, id);

            if (patchDoc == null || !patchDoc.Operations.Any())
            {
                throw new InvalidRequestException("Request cannot be empty.");
            }

            Validate.PatchCustomerFields(patchDoc, _allowedPatchFields);

            var phoneNumberPath = DomainCustomer.PropNameToPatchPath[nameof(DomainCustomer.Phone.PhoneNumber)];
            var requestedPhoneNumber = patchDoc.Operations.SingleOrDefault(o => o.path.Equals(phoneNumberPath))?.value.ToString();
            var isPhoneNumberRequest = requestedPhoneNumber != null;
            if (isPhoneNumberRequest)
            {
                Validate.PhoneNumberFormat(requestedPhoneNumber);
            }

            var domainCustomer = _customerService.GetCustomer(id);

            patchDoc.ApplyTo(domainCustomer, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _customerService.UpdateCustomer(domainCustomer, isPhoneNumberRequest);

            return Ok();
        }
    }
}