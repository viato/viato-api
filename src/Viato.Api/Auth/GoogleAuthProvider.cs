using System.Net.Http;
using Viato.Api.Stores;
using Viato.Api.Entities;

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
