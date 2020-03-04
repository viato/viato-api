namespace Viato.Api
{
    public class AppSettings
    {
        public string PostgresConnectionString { get; set; }

        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }

        public string TwitterConsumerKey { get; set; }
        public string TwitterConsumerSecret { get; set; }

        public string FacebookAppId { get; set; }
        public string FacebookAppSecret { get; set; }
    }
}
