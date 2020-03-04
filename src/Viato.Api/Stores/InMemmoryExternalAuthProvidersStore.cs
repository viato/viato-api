using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Viato.Api.Entities;

namespace Viato.Api.Stores
{
    public class InMemmoryExternalAuthProvidersStore : IExternalAuthProvidersStore
    {
        private readonly IEnumerable<ExternalAuthProver> _providers;

        public InMemmoryExternalAuthProvidersStore(IOptions<AppSettings> appSettings)
        {
            _providers = new List<ExternalAuthProver>()
            {
                new ExternalAuthProver
                {
                    Name = "Facebook",
                    UserInfoEndPoint = appSettings.Value.FacebookUserInfoEndpoint.ToString()
                },
                new ExternalAuthProver
                {
                    Name = "Google",
                    UserInfoEndPoint = appSettings.Value.GoogleUserInfoEndpoint.ToString()
                },
                new ExternalAuthProver
                {
                    Name = "Twitter",
                    UserInfoEndPoint = appSettings.Value.TwitterUserInfoEndpoint.ToString()
                }
            };
        }

        public ExternalAuthProver Get(ExternalProviderType providerType)
        {
            return _providers.FirstOrDefault(x => x.Type == providerType);
        }
    }
}
