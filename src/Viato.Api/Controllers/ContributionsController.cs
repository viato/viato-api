using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viato.Api.Auth;

namespace Viato.Api.Controllers
{
    [ApiController]
    [Route("contributions")]
    public class ContributionsController : Controller
    {
        private readonly ViatoContext _dbContext;

        public ContributionsController(ViatoContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet("my")]
        public IActionResult GetMy(int take = 10, int skip = 0)
        {
            var userId = User.GetUserId();
            var contributions = _dbContext.Contributions
                .Where(c => c.ContributorId == userId)
                .Skip(skip)
                .Take(take);

            return Ok(contributions);
        }
    }
}
