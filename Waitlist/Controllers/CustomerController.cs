using Api.Authorization;
using Api.Helpers;
using Api.Models;
using Api.Requests;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using DomainCustomer = Domain.Models.Customer;

namespace Api.Controllers
{
    [Authorize(Roles = "Customer")]
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

        [HttpGet]
        [Route("{id}")]
        [MapException(typeof(CustomerNotFoundException), HttpStatusCode.NotFound)]
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        [MapException(typeof(NotAuthorizedException), HttpStatusCode.Unauthorized)]
        public IActionResult GetCustomer([Required] string id)
        {
            Authorize.TokenAgainstResource(HttpContext.User, id);

            var domainCustomer = _customerService.GetCustomer(id);

            var apiCustomer = new Customer(domainCustomer);
            return Ok(apiCustomer);
        }

        [HttpPost]
        [Route("{id}/phone/send-verification")]
        [MapException(typeof(CustomerNotFoundException), HttpStatusCode.NotFound)]
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        [MapException(typeof(InternalServiceException), HttpStatusCode.InternalServerError)]
        [MapException(typeof(NotAuthorizedException), HttpStatusCode.Unauthorized)]
        public IActionResult SendPhoneVerification([Required] string id)
        {
            Authorize.TokenAgainstResource(HttpContext.User, id);

            _customerService.SendPhoneVerification(id);

            return Ok();
        }

        [HttpPost]
        [Route("{id}/phone/verify")]
        [MapException(typeof(CustomerNotFoundException), HttpStatusCode.NotFound)]
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        [MapException(typeof(InternalServiceException), HttpStatusCode.InternalServerError)]
        [MapException(typeof(NotAuthorizedException), HttpStatusCode.Unauthorized)]
        public IActionResult VerifyPhoneNumber([Required] string id, [FromBody] VerifyPhoneNumberRequest request)
        {
            Authorize.TokenAgainstResource(HttpContext.User, id);

            _customerService.VerifyPhoneNumber(id, request.VerificationCode);

            return Ok();
        }

        //NOTE: In the request header, clients have to specify Content-Type: application/json-patch+json.
        //Otherwise, JsonPatchDocument serialization will fail.
        [HttpPatch]
        [Route("{id}")]
        [MapException(typeof(CustomerNotFoundException), HttpStatusCode.NotFound)]
        [MapException(typeof(InvalidRequestException), HttpStatusCode.BadRequest)]
        [MapException(typeof(NotAuthorizedException), HttpStatusCode.Unauthorized)]
        public IActionResult UpdateCustomer([Required] string id, [FromBody] JsonPatchDocument<DomainCustomer> patchDoc)
        {
            Authorize.TokenAgainstResource(HttpContext.User, id);

            Validate<DomainCustomer>.EmptyPatchRequest(patchDoc);
            Validate<DomainCustomer>.PatchRestrictedFields(patchDoc, _allowedPatchFields);

            var domainCustomer = _customerService.GetCustomer(id);

            var isPhoneNumberRequest = ValidateUpdatePhoneNumberRequest(domainCustomer, patchDoc);

            patchDoc.ApplyTo(domainCustomer, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _customerService.UpdateCustomer(domainCustomer, isPhoneNumberRequest);

            return Ok();
        }

        private static bool ValidateUpdatePhoneNumberRequest(DomainCustomer customer, JsonPatchDocument<DomainCustomer> patchDoc)
        {
            var phoneNumberPath = DomainCustomer.PropNameToPatchPath[nameof(DomainCustomer.Phone.PhoneNumber)];
            var phoneNumberRequest = patchDoc.Operations.SingleOrDefault(o => o.path.Equals(phoneNumberPath));
            var isPhoneNumberRequest = phoneNumberRequest != null;
            if (!isPhoneNumberRequest)
            {
                return false;
            }

            var requestedPhoneNumber = phoneNumberRequest?.value.ToString();

            Validate.PhoneNumberFormat(requestedPhoneNumber);
                
            if (string.IsNullOrEmpty(requestedPhoneNumber))
            {
                throw new InvalidRequestException("Phone number cannot be null or empty.");
            }

            if (customer.Phone?.PhoneNumber == requestedPhoneNumber)
            {
                patchDoc.Operations.Remove(phoneNumberRequest);
                return false;
            }

            return true;
        }
    }
}