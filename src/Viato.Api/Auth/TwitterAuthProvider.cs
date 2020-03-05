using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Viato.Api.Entities;
using Viato.Api.Stores;

namespace Viato.Api.Auth
{
    public class TwitterAuthProvider : ExternalAuthProvider
    {
        public TwitterAuthProvider(IExternalAuthProvidersStore providerStore, IHttpClientFactory httpClientFactory)
            : base(ExternalProviderType.Twitter, providerStore, httpClientFactory)
        {
        }

        protected override string GetQuery(string accessToken)
        {
            var userInfoEndpoint = Provider.UserInfoEndPoint;

            var tokenString = accessToken.Split('&').ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]);
            if (tokenString.Count < 4)
            {
                return null;
            }

            var oauth_consumer_key = tokenString["oauth_consumer_key"];
            var consumerSecret = tokenString["oauth_consumer_secret"];
            var oauth_token_secret = tokenString["oauth_token_secret"];
            var oauth_token = tokenString["oauth_token"];
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";
            string oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));

            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            string oauth_timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();

            var sd = new SortedDictionary<string, string>
                    {
                        { "oauth_version", oauth_version },
                        { "oauth_consumer_key", oauth_consumer_key },
                        { "oauth_nonce", oauth_nonce },
                        { "oauth_signature_method", oauth_signature_method },
                        { "oauth_timestamp", oauth_timestamp },
                        { "oauth_token", oauth_token },
                    };

            string baseString = string.Empty;
            baseString += "GET" + "&";
            baseString += Uri.EscapeDataString(userInfoEndpoint) + "&";
            foreach (var entry in sd)
            {
                baseString += Uri.EscapeDataString(entry.Key + "=" + entry.Value + "&");
            }

            baseString = baseString[0..^3];

            var signingKey = Uri.EscapeDataString(consumerSecret) + "&" + Uri.EscapeDataString(oauth_token_secret);

            var hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey));

            var signatureString = Convert.ToBase64String(hasher.ComputeHash(new ASCIIEncoding().GetBytes(baseString)));

            var authorizationHeaderParams = string.Empty;
            authorizationHeaderParams += "OAuth ";
            authorizationHeaderParams += "oauth_nonce=" + "\"" +
                Uri.EscapeDataString(oauth_nonce) + "\",";

            authorizationHeaderParams +=
                "oauth_signature_method=" + "\"" +
                Uri.EscapeDataString(oauth_signature_method) +
                "\",";

            authorizationHeaderParams += "oauth_timestamp=" + "\"" +
                Uri.EscapeDataString(oauth_timestamp) + "\",";

            authorizationHeaderParams += "oauth_consumer_key="
                + "\"" + Uri.EscapeDataString(
                oauth_consumer_key) + "\",";

            authorizationHeaderParams += "oauth_token=" + "\"" +
                Uri.EscapeDataString(oauth_token) + "\",";

            authorizationHeaderParams += "oauth_signature=" + "\""
                + Uri.EscapeDataString(signatureString) + "\",";

            authorizationHeaderParams += "oauth_version=" + "\"" +
                Uri.EscapeDataString(oauth_version) + "\"";

            return authorizationHeaderParams;
        }
    }
}
