using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Viato.Api.Entities;
using Viato.Api.Models;

namespace Viato.Api.Controllers
{
    [AllowAnonymous]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser()
            {
                UserName = model.UserName,
                Email = model.UserName,
                EmailConfirmed = true, // TODO: Add email confirmation
            };

            var userCreationResult = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, model.Role); // asume thahat this will not fail

            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return BadRequest(ModelState);
                }
            }

            return Ok(new RegisterResponseModel()
            {
                UserId = user.Id,
                Email = user.Email,
            });
        }
    }
}
