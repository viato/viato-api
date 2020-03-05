using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Viato.Api.Entities;
using Viato.Api.Models;
using Viato.Api.Notification;

namespace Viato.Api.Controllers
{
    [AllowAnonymous]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AuthController(UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserModel model)
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
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = false,
            };

            var userCreationResult = await _userManager.CreateAsync(user, model.Password);

            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return BadRequest(ModelState);
                }
            }

            var addRoleClaimResult = await _userManager.AddClaimAsync(user, new Claim("role", model.Role.ToString()));
            if (!addRoleClaimResult.Succeeded)
            {
                // TOOD add warning log.
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await _emailSender.SendAsync(
                    user.Email,
                    "Confirm Email",
                    $"Verification code {code}");

            return Ok(new RegisterResponseModel()
            {
                UserId = user.Id,
                Email = user.Email,
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var identityResult = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        return BadRequest(ModelState);
                    }
                }
            }

            return Ok();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody]ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && (await _userManager.IsEmailConfirmedAsync(user)))
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                await _emailSender.SendAsync(
                    model.Email,
                    "Reset Password",
                    $"Verification code {code}");
            }

            return Ok();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody]ConfirmEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var identityResult = await _userManager.ConfirmEmailAsync(user, model.Code);

                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        return BadRequest(ModelState);
                    }
                }
            }

            return Ok();
        }
    }
}
