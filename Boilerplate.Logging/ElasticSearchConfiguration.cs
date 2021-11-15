namespace Boilerplate.Logging
{
    public class ElasticSearchConfiguration : LogConfigurationBase
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? IndexFormat { get; set; }
        public string? BaseUrl { get; set; }
    }
}