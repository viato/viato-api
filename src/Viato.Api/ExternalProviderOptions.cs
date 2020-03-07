using System;

namespace Viato.Api
{
    public class ExternalProviderOptions
    {
        public Uri GoogleUserInfoEndpoint { get; set; }

        public Uri TwitterUserInfoEndpoint { get; set; }

        public Uri FacebookUserInfoEndpoint { get; set; }
    }
}
