using System;
using JetBrains.Annotations;
using Serilog.Events;

namespace SmartSchedule.Xunit
{
    [PublicAPI]
    public interface ITestLogSession : IDisposable
    {
        string CaptureIdKey { get; }
        Guid CaptureId { get; }
        bool LogPredicate(LogEvent logEvent);
    }
}