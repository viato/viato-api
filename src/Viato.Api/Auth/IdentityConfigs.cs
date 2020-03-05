using System.Collections.Generic;
using IdentityServer4.Models;

namespace Viato.Api.Auth
{
    public static class IdentityConfigs
    {
        public static IEnumerable<IdentityResource> Ids =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("api", "Viato API"),
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "viato-web-ui",
                    ClientName = "Viato Web Ui Client",
                    AllowedGrantTypes = new[] { "password", "external" },
                    RequireClientSecret = false,
                    AllowedScopes =
                    {
                        "api",
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true,
                    AlwaysSendClientClaims = true,
                    Enabled = true,
                },
            };
    }
}
