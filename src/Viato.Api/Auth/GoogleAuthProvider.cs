using System.Net.Http;
using Viato.Api.Entities;
using Viato.Api.Stores;

namespace Viato.Api.Auth
{
    public class GoogleAuthProvider : ExternalAuthProvider
    {
        public GoogleAuthProvider(IExternalAuthProvidersStore providerStore, IHttpClientFactory httpClientFactory)
            : base(ExternalProviderType.Google, providerStore, httpClientFactory)
        {
        }

        protected override string GetQuery(string accessToken)
        {
            return $"?access_token={accessToken}";
        }
    }
}
