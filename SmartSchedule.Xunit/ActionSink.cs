using System;
using Serilog.Core;
using Serilog.Events;

namespace SmartSchedule.Xunit
{
    public class ActionSink : ILogEventSink
    {
        private readonly Action<LogEvent> _emit;

        public ActionSink(Action<LogEvent> emit)
        {
            _emit = emit;
        }

        public void Emit(LogEvent logEvent)
        {
            _emit?.Invoke(logEvent);
        }
    }
}