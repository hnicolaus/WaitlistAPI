using Api.Helpers;
using Api.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
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
        public IActionResult GetCustomer(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be empty.");
            }

            var tokenUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (tokenUserId != id)
            {
                return Unauthorized("Unauthorized to access resource");
            }

            var domainCustomer = _customerService.GetCustomer(id);
            if (domainCustomer == null)
            {
                return NotFound();
            }

            return Ok(new Customer(domainCustomer));
        }

        [HttpPatch]
        [Route("{id}")]
        public IActionResult UpdateCustomer(string id, [FromBody] JsonPatchDocument<DomainCustomer> patchDoc)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id cannot be empty.");
            }
            if (patchDoc == null || !patchDoc.Operations.Any())
            {
                return BadRequest("Request cannot be empty.");
            }

            var requestedFields = patchDoc.Operations.Select(o => o.path);
            var restrictedFields = requestedFields.Where(field => !field.Equals(_phoneNumberPatchPath));
            if (restrictedFields.Any())
            {
                return BadRequest("Cannot update the following restricted field(s): " + string.Join(", ", restrictedFields));
            }

            var requestedPhoneNumber = patchDoc.Operations.First(o => o.path.Equals(_phoneNumberPatchPath)).value.ToString();
            if (!Validator.ValidatePhoneNumber(requestedPhoneNumber))
            {
                return BadRequest("Phone number format is invalid");
            }

            var domainCustomer = _customerService.GetCustomer(id);
            if (domainCustomer == null)
            {
                return NotFound();
            }

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