using Api.Helpers;
using Api.Models;
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
        private const string _phoneNumberPatchPath = "/phoneNumber";

        public CustomerController(CustomerService waitlistService)
        {
            _customerService = waitlistService;
        }

        [Authorize]
        [HttpGet]
        [Route("{id}", Name = "GetCustomer")]
        [MapException(new[] { typeof(CustomerNotFoundException), typeof(InvalidRequestException) },
            new[] { HttpStatusCode.NotFound, HttpStatusCode.BadRequest })]
        public IActionResult GetCustomer(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidRequestException("Id cannot be empty.");
            }

            var domainCustomer = _customerService.GetCustomer(id);

            var result = new Customer(domainCustomer);
            return Ok(result);
        }

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
            if (patchDoc == null || !patchDoc.Operations.Any())
            {
                throw new InvalidRequestException("Request cannot be empty.");
            }

            var requestedFields = patchDoc.Operations.Select(o => o.path);
            var restrictedFields = requestedFields.Where(field => !field.Equals(_phoneNumberPatchPath));
            if (restrictedFields.Any())
            {
                throw new InvalidRequestException("Cannot update the following restricted field(s): " + string.Join(", ", restrictedFields));
            }

            var requestedPhoneNumber = patchDoc.Operations.First(o => o.path.Equals(_phoneNumberPatchPath)).value.ToString();
            Validator.ValidatePhoneNumber(requestedPhoneNumber);

            var domainCustomer = _customerService.GetCustomer(id);

            patchDoc.ApplyTo(domainCustomer, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _customerService.SaveChanges();
            return Ok();
        }
    }
}