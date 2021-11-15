using JetBrains.Annotations;

namespace Boilerplate.Logging
{
    [PublicAPI]
    public class SerilogConfiguration
    {
        public ElasticSearchConfiguration ElasticSearch { get; set; } = new ElasticSearchConfiguration();
        public FileLogConfiguration FileLog { get; set; } = new FileLogConfiguration();
        public ConsoleConfiguration Console { get; set; } = new ConsoleConfiguration();
        public SelfLogConfiguration SelfLog { get; set; } = new SelfLogConfiguration();
        public RequestLogging RequestLogging { get; set; } = new RequestLogging();
        public GitVersionConfiguration? GitVersion { get; set; }
    }

    public class RequestLogging
    {
        public bool Enabled { get; set; } = true;
    }
}