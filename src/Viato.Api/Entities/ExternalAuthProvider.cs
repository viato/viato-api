namespace Viato.Api.Entities
{
    public class ExternalAuthProvider
    {
        public string Name { get; set; }
        public ExternalProviderType Type { get; set; }
        public string UserInfoEndPoint { get; set; }
    }

    public enum ExternalProviderType
    {
        Facebook,
        Twitter,
        Google,
    }
}
