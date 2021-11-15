namespace Boilerplate.Logging
{
    public class SelfLogConfiguration
    {
        public bool Enabled { get; set; }

        public string Path { get; set; } = "serilog-selflog.log";
    }
}