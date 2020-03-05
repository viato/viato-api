using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Viato.Api.Entities;

namespace Viato.Api.Stores
{
    public class InMemmoryExternalAuthProvidersStore : IExternalAuthProvidersStore
    {
        private readonly IEnumerable<ExternalAuthProvider> _providers;

        public InMemmoryExternalAuthProvidersStore(IOptions<AppSettings> appSettings)
        {
            _providers = new List<ExternalAuthProvider>()
            {
                new ExternalAuthProvider
                {
                    Name = "Facebook",
                    UserInfoEndPoint = appSettings.Value.FacebookUserInfoEndpoint.ToString(),
                },
                new ExternalAuthProvider
                {
                    Name = "Google",
                    UserInfoEndPoint = appSettings.Value.GoogleUserInfoEndpoint.ToString(),
                },
                new ExternalAuthProvider
                {
                    Name = "Twitter",
                    UserInfoEndPoint = appSettings.Value.TwitterUserInfoEndpoint.ToString(),
                },
            };
        }

        public ExternalAuthProvider Get(ExternalProviderType providerType)
        {
            return _providers.FirstOrDefault(x => x.Type == providerType);
        }
    }
}
