using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Viato.Api.Controllers
{
    [AllowAnonymous]
    [Route("contributions")]
    public class ContributionsController : Controller
    {
    }
}
