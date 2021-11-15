namespace Boilerplate.Logging
{
    /// <summary>
    /// Primarily used dynamically so the initializer can read the branch and commit values from the assembly, and pass them to Serilog initialization
    /// </summary>
    public class GitVersionConfiguration
    {
        public string Branch { get; set; } = "unknown";
        public string Commit { get; set; } = "unknown";
    }
}