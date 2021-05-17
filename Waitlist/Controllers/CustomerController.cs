using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        public CustomerService _customerService;

        public CustomerController(CustomerService waitlistService)
        {
            _customerService = waitlistService;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetCustomer(int id)
        {
            var domainCustomer = _customerService.GetCustomer(id);
            var apiCustomer = new Customer(domainCustomer);
            return Ok(apiCustomer);
        }

        [HttpPost]
        public IActionResult CreateCustomer(CreateCustomerRequest request)
        {
            var domainRequest = request.ToDomain();
            _customerService.CreateCustomer(domainRequest);
            return Ok();
        }
    }
}
