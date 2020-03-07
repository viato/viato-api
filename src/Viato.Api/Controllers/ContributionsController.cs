using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Viato.Api.Controllers
{
    [AllowAnonymous]
    [Route("contributions")]
    public class ContributionsController : Controller
    {
        private readonly ViatoContext _dbContext;

        public ContributionsController(ViatoContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
