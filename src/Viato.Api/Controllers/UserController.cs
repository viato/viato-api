﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Viato.Api.Entities;
using Viato.Api.Models;
using Viato.Api.Notification;
using Viato.Api.Services;

namespace Viato.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public UserController(
            UserManager<AppUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        [HttpPost("register")]
        [Produces(typeof(RegisterResponseModel))]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserModel model)
        {
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

        [Authorize]
        [HttpGet("two-factor")]
        [Produces(typeof(TwoFactorResponseModel))]
        public async Task<IActionResult> GetTwoFactor()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.TwoFactorEnabled)
            {
                return StatusCode(AppHttpStatusCodes.User2FAAlreadyEnabled);
            }

            var code = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(code))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                code = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var response = new TwoFactorResponseModel
            {
                Code = code,
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost("two-factor")]
        [Produces(typeof(RecoveryCodeResponseModel))]
        public async Task<IActionResult> SetTwoFactor([FromBody] SetTwoFactorModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, model.Code);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError(nameof(model.Code), "Invalid 2FA code");
                return BadRequest(ModelState);
            }

            var identityResult = await _userManager.SetTwoFactorEnabledAsync(user, model.Enabled);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return BadRequest(ModelState);
                }
            }

            if (model.Enabled)
            {
                var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                var response = new RecoveryCodeResponseModel
                {
                    RecoveryCodes = recoveryCodes,
                };

                return Ok(response);
            }

            return Ok();
        }
    }
}
