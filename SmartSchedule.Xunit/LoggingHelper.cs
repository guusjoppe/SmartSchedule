using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using SmartSchedule.Xunit;
using Xunit.Abstractions;

namespace SmartSchedule.Xunit
{
    [PublicAPI]
    public static class LoggingHelper
    {
        private const string CaptureCorrelationIdKey = "CaptureCorrelationId";
        private static readonly MessageTemplateTextFormatter Formatter = new MessageTemplateTextFormatter("{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}", null);

        private static readonly DynamicCompoundSink LogEventSink = new DynamicCompoundSink();
        public static ILogEventSink Sink => LogEventSink;

        public static Logger Setup(ILogger? existing = null)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Sink(Sink);

            if (existing != null)
            {
                loggerConfiguration.WriteTo.Logger(existing);
            }

            return loggerConfiguration.CreateLogger();
        }

        public static ITestLogSession Capture(ITestOutputHelper testOutputHelper)
        {
            return new TestLogSession(CaptureCorrelationIdKey, LogEventSink, testOutputHelper);
        }

        public static IDisposable AddLogSink(ILogEventSink logEventSink)
        {
            lock (LogEventSink)
            {
                LogEventSink.Add(logEventSink);
            }

            return new ActionDisposable(() =>
            {
                lock (LogEventSink)
                {
                    LogEventSink.Remove(logEventSink);
                }
            });
        }

        private class DynamicCompoundSink : List<ILogEventSink>, ILogEventSink
        {
            public void Emit(LogEvent logEvent)
            {
                foreach (var logEventSink in this)
                {
                    try
                    {
                        logEventSink.Emit(logEvent);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        private class TestLogSession : ITestLogSession
        {
            private readonly IDisposable _pushProperty;
            private readonly DynamicCompoundSink _logEventSink;
            private readonly TestOutputHelperLogSink _sink;

            public Guid CaptureId { get; }
            public string CaptureIdKey { get; }

            public TestLogSession(string captureIdKey, DynamicCompoundSink logEventSink, ITestOutputHelper testOutputHelper)
            {
                _logEventSink = logEventSink;
                CaptureIdKey = captureIdKey;
                CaptureId = Guid.NewGuid();
                _sink = new TestOutputHelperLogSink(testOutputHelper, LogPredicate);

                LogEventSink.Add(_sink);
                _pushProperty = LogContext.PushProperty(captureIdKey, CaptureId);
            }

            public void Dispose()
            {
                lock (_logEventSink)
                {
                    _logEventSink.Remove(_sink);
                }

                _pushProperty.Dispose();
            }

            public bool LogPredicate(LogEvent logEvent) => logEvent.Properties.TryGetValue(CaptureIdKey, out var property)
                                                           && property is ScalarValue scalarProperty
                                                           && (Guid) scalarProperty.Value == CaptureId;
        }
    }

    public class ActionDisposable : IDisposable
    {
        private readonly Action _action;

        public ActionDisposable(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action?.Invoke();
        }
    }
}