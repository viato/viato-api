using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
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

            if (!Enum.IsDefined(typeof(AppUserRole), model.Role))
            {
                ModelState.AddModelError(nameof(model.Role), $"{model.Role} is not valid role");
                return BadRequest(ModelState);
            }

            var user = new AppUser()
            {
                UserName = model.UserName,
                Email = model.UserName,
                EmailConfirmed = true, // TODO: Add email confirmation
            };

            var userCreationResult = await _userManager.CreateAsync(user, model.Password);

            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return BadRequest(ModelState);
                }
            }

            // asume that this will not fail, even if it fails, we don't need to stop it
            var addRoleClaimResult = await _userManager.AddClaimAsync(user, new Claim("role", model.Role.ToString()));
            if (!addRoleClaimResult.Succeeded)
            {
                // TODO: log
            }

            return Ok(new RegisterResponseModel()
            {
                UserId = user.Id,
                Email = user.Email,
            });
        }
    }
}
