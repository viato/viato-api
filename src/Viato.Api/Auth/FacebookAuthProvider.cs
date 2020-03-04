using System.Net.Http;
using Viato.Api.Stores;
using Viato.Api.Entities;

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
