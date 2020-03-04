using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Viato.Api.Controllers
{
    [Route("api/ping")]
    public class TestController : Controller
    {
        [Authorize]
        [HttpGet("authorized")]
        public IActionResult Authorized()
        {
            return Ok(new { result = "authorized-alive" });
        }

        [HttpGet("anonymous")]
        public IActionResult Anonymous()
        {
            return Ok(new { result = "anonymous-alive" });
        }
    }
}
