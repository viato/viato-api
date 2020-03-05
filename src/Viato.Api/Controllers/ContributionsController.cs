using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Viato.Api.Controllers
{
    [AllowAnonymous]
    [Route("contributions")]
    public class ContributionsController : Controller
    {
    }
}
