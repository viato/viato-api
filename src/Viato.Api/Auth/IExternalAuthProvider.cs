using Newtonsoft.Json.Linq;

namespace Viato.Api.Auth
{
    public interface IExternalAuthProvider
    {
        JObject GetUserInfo(string accessToken);
    }
}
