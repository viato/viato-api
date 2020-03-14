using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using Viato.Api.Entities;
using Viato.Api.Services;

namespace Viato.Api.Auth
{
    public class ExternalAuthenticationGrant : IExtensionGrantValidator
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly FacebookAuthProvider _facebookAuthProvider;
        private readonly GoogleAuthProvider _googleAuthProvider;
        private readonly TwitterAuthProvider _twitterAuthProvider;
        private readonly IStagedContributionService _stagedContributionService;

        private readonly Dictionary<ExternalProviderType, IExternalAuthProvider> _providers;

        public ExternalAuthenticationGrant(
            UserManager<AppUser> userManager,
            FacebookAuthProvider facebookAuthProvider,
            GoogleAuthProvider googleAuthProvider,
            TwitterAuthProvider twitterAuthProvider,
            IStagedContributionService stagedContributionService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _facebookAuthProvider = facebookAuthProvider ?? throw new ArgumentNullException(nameof(facebookAuthProvider));
            _googleAuthProvider = googleAuthProvider ?? throw new ArgumentNullException(nameof(googleAuthProvider));
            _twitterAuthProvider = twitterAuthProvider ?? throw new ArgumentNullException(nameof(twitterAuthProvider));
            _stagedContributionService = stagedContributionService ?? throw new ArgumentNullException(nameof(stagedContributionService));

            _providers = new Dictionary<ExternalProviderType, IExternalAuthProvider>
            {
                 { ExternalProviderType.Facebook, _facebookAuthProvider },
                 { ExternalProviderType.Google, _googleAuthProvider },
                 { ExternalProviderType.Twitter, _twitterAuthProvider },
            };
        }

        public string GrantType => "external";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var provider = context.Request.Raw.Get("provider");
            if (string.IsNullOrWhiteSpace(provider))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid provider");
                return;
            }

            var token = context.Request.Raw.Get("external_token");
            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid external token");
                return;
            }

            var providerType = (ExternalProviderType)Enum.Parse(typeof(ExternalProviderType), provider, ignoreCase: true);

            if (!Enum.IsDefined(typeof(ExternalProviderType), providerType))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid provider");
                return;
            }

            var userInfo = _providers[providerType].GetUserInfo(token);

            if (userInfo == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "couldn't retrieve user info from specified provider, please make sure that access token is not expired");
                return;
            }

            var externalId = userInfo.Value<string>("id");
            if (!string.IsNullOrWhiteSpace(externalId))
            {
                var user = await _userManager.FindByLoginAsync(provider, externalId);
                if (user != null)
                {
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    context.Result = new GrantValidationResult(user.Id.ToString(), provider, userClaims, provider, null);
                    return;
                }
            }

            context.Result = await ProcessUserAsync(context, userInfo, provider);
            return;
        }

        public async Task<GrantValidationResult> ProcessUserAsync(ExtensionGrantValidationContext context, JObject userInfo, string provider)
        {
            var userEmail = userInfo.Value<string>("email");
            var userExternalId = userInfo.Value<string>("id");

            if (userEmail == null)
            {
                var customResponse = new Dictionary<string, object>
                {
                    { "userInfo", userInfo },
                };

                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not retrieve user's email from the given provider", customResponse);
            }

            var existingUser = _userManager.FindByEmailAsync(userEmail).Result;
            if (existingUser != null)
            {
                var userClaims = await _userManager.GetClaimsAsync(existingUser);
                return new GrantValidationResult(existingUser.Id.ToString(), provider, userClaims, provider, null);
            }
            else
            {
                var newUser = new AppUser { Email = userEmail, UserName = userEmail };
                var result = await _userManager.CreateAsync(newUser);
                if (result.Succeeded)
                {
                    await _userManager.AddLoginAsync(newUser, new UserLoginInfo(provider, userExternalId, provider));
                    var userClaims = await _userManager.GetClaimsAsync(newUser);

                    if (Guid.TryParse(context.Request.Raw.Get("staged_contribution_id"), out Guid stagedContributionId))
                    {
                        await _stagedContributionService.AttachStagedContributionAsync(stagedContributionId, newUser);
                    }

                    return new GrantValidationResult(newUser.Id.ToString(), provider, userClaims, provider, null);
                }

                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "user could not be created, please try again");
            }
        }
    }
}
