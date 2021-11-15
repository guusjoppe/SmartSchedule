using System.Collections.Generic;
using JetBrains.Annotations;
using Serilog.Core;
using Serilog.Events;

namespace SmartSchedule.Xunit
{
    [PublicAPI]
    public class CollectorSink : ILogEventSink
    {
        private readonly IList<LogEvent> _logEvents = new List<LogEvent>();
        public IEnumerable<LogEvent> LogEvents => _logEvents;
        
        public void Emit(LogEvent logEvent)
        {
            lock (_logEvents)
            {
                _logEvents.Add(logEvent);
            }
        }
    }
}