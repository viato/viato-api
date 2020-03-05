using System.Net.Http;
using Viato.Api.Entities;
using Viato.Api.Stores;

namespace Viato.Api.Auth
{
    public class FacebookAuthProvider : ExternalAuthProvider
    {
        public FacebookAuthProvider(IExternalAuthProvidersStore providerStore, IHttpClientFactory httpClientFactory)
            : base(ExternalProviderType.Facebook, providerStore, httpClientFactory)
        {
        }

        protected override string GetQuery(string accessToken)
        {
            return $"?fields=id,email,name&access_token={accessToken}";
        }
    }
}
