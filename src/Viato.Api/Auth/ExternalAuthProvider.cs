using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using Viato.Api.Entities;
using Viato.Api.Stores;

namespace Viato.Api.Auth
{
    public abstract class ExternalAuthProvider : IExternalAuthProvider
    {
        private readonly ExternalProviderType _providerType;
        private readonly IExternalAuthProvidersStore _providerStore;
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalAuthProvider(
            ExternalProviderType providerType,
            IExternalAuthProvidersStore providerStore,
            IHttpClientFactory httpClientFactory)
        {
            _providerStore = providerStore;
            _httpClientFactory = httpClientFactory;
        }

        public Entities.ExternalAuthProvider Provider => _providerStore.Get(_providerType);

        public JObject GetUserInfo(string accessToken)
        {
            if (Provider == null)
            {
                throw new ArgumentNullException(nameof(Provider));
            }

            var result = _httpClientFactory.CreateClient().GetAsync(Provider.UserInfoEndPoint + GetQuery(accessToken)).Result;
            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                return infoObject;
            }

            return null;
        }

        protected abstract string GetQuery(string accessToken);
    }
}
