using System.Collections.Generic;
using Serilog.Events;

namespace Boilerplate.Logging
{
    public class MinimumLevel
    {
        public LogEventLevel Default { get; set; } = LogEventLevel.Verbose;
        public IDictionary<string, LogEventLevel> Override { get; set; } = new Dictionary<string, LogEventLevel>();
    }
}