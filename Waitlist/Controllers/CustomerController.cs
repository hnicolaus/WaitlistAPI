using Api.Helpers;
using Api.Models;
using Api.Requests;
using Domain.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using DomainCreateCustomerRequest = Domain.Requests.CreateCustomerRequest;
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

        [HttpGet]
        [Route("{id}", Name = "GetCustomer")]
        public IActionResult GetCustomer(int id)
        {
            var domainCustomer = _customerService.GetCustomer(id);
            if (domainCustomer == null)
            {
                return NotFound();
            }

            return Ok(new Customer(domainCustomer));
        }

        [HttpPatch]
        [Route("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] JsonPatchDocument<DomainCustomer> patchDoc)
        {
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

        //Google sign-in is the only way to sign-up/sign-in.
        //Treat every successful Google sign-in as if they are an existing user.
        [HttpPost]
        [Route("google-sign-in")]
        public IActionResult GoogleUserSignIn(GoogleUserSignInRequest request)
        {
            //Keep Domain project focused on business logic and isolated from GoogleUser validation and concept.
            GoogleUser user;
            if (!Validator.ValidateGoogleUser(request.IdToken, out user))
            {
                return BadRequest("Google ID token is invalid");
            }

            var domainRequest = new DomainCreateCustomerRequest(user.FirstName, user.LastName, user.Email);
            var domainCustomer = _customerService.GetOrCreateCustomer(domainRequest);

            var apiCustomer = new Customer(domainCustomer);
            return CreatedAtRoute(nameof(GetCustomer), new { id = apiCustomer.Id }, apiCustomer);
        }
    }
}