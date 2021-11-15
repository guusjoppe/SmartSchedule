namespace Boilerplate.Logging
{
    public abstract class LogConfigurationBase
    {
        public bool Enabled { get; set; }

        public MinimumLevel MinimumLevel { get; set; } = new MinimumLevel();
    }
}