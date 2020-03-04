using System.Text.Json.Serialization;

namespace Viato.Api.Models
{
    public class RegisterResponseModel
    {
        [JsonPropertyName("user-id")]
        public long UserId { get; set; }
    }
}
