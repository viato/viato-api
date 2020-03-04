using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
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

        public AuthController(UserManager<AppUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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
                EmailConfirmed = false,
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
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel model)
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
                        ModelState.AddModelError("", error.Description);
                        return BadRequest(ModelState);
                    }
                }
            }

            return Ok();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordModel model)
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
        public async Task<IActionResult> ConfirmEmailAsync(ConfirmEmailModel model)
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
                        ModelState.AddModelError("", error.Description);
                        return BadRequest(ModelState);
                    }
                }
            }

            return Ok();
        }
    }
}
