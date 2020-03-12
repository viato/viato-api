using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Viato.Api.Entities;

namespace Viato.Api.Stores
{
    public class InMemmoryExternalAuthProvidersStore : IExternalAuthProvidersStore
    {
        private readonly IEnumerable<ExternalAuthProvider> _providers;

        public InMemmoryExternalAuthProvidersStore(IOptions<ExternalProviderOptions> externalProviderOptions)
        {
            _providers = new List<ExternalAuthProvider>()
            {
                new ExternalAuthProvider
                {
                    Name = "Facebook",
                    Type = ExternalProviderType.Facebook,
                    UserInfoEndPoint = externalProviderOptions.Value.FacebookUserInfoEndpoint.ToString(),
                },
                new ExternalAuthProvider
                {
                    Name = "Google",
                    Type = ExternalProviderType.Google,
                    UserInfoEndPoint = externalProviderOptions.Value.GoogleUserInfoEndpoint.ToString(),
                },
                new ExternalAuthProvider
                {
                    Name = "Twitter",
                    Type = ExternalProviderType.Twitter,
                    UserInfoEndPoint = externalProviderOptions.Value.TwitterUserInfoEndpoint.ToString(),
                },
            };
        }

        public ExternalAuthProvider Get(ExternalProviderType providerType)
        {
            return _providers.FirstOrDefault(x => x.Type == providerType);
        }
    }
}
