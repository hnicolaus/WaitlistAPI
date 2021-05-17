using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Waitlist.Controllers
{
    [ApiController]
    [Route("waitlist")]
    public class WaitlistController : ControllerBase
    {
        public WaitlistService _waitlistService;

        public WaitlistController(WaitlistService waitlistService)
        {
            _waitlistService = waitlistService;
        }

        [HttpGet]
        public IActionResult GetWaitlist(string userName)
        {
            var domainWaitlist = _waitlistService.GetActiveWaitlist(userName);
            if (domainWaitlist == null)
            {
                //throw new System.Exception($"Active waitlist for userName {userName} is not found");
                return new NotFoundResult();
            
            }
            var apiWaitlist = new Waitlist(domainWaitlist);
            return new OkObjectResult(apiWaitlist);
        }

        [HttpPost]
        public IActionResult CreateWaitlist([FromBody]CreateWaitlistRequest request)
        {
            var domainRequest = request.ToDomain();
            _waitlistService.CreateWaitlist(domainRequest);
            return Ok();
        }
    }
}
