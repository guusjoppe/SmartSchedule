using System;
using System.IO;
using Boilerplate.Logging;
using JetBrains.Annotations;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Xunit.Abstractions;

namespace SmartSchedule.Xunit
{
    [PublicAPI]
    public class TestOutputHelperLogSink : ILogEventSink
    {
        private static readonly MessageTemplateTextFormatter Formatter = new MessageTemplateTextFormatter("{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{Thread}] {Message}{NewLine}{Exception}", null);
        
        private readonly ITestOutputHelper _outputHelper;
        private readonly Func<LogEvent, bool>? _predicate;

        public TestOutputHelperLogSink(ITestOutputHelper outputHelper, Func<LogEvent, bool>? predicate = null)
        {
            _outputHelper = outputHelper;
            _predicate = predicate;
        }

        public static ILogger<T> Create<T>(ITestOutputHelper outputHelper, LogEventLevel minimumLevel = LogEventLevel.Verbose, Func<LogEvent, bool>? predicate = null)
        {
            return new Logger<T>(Create(outputHelper, minimumLevel, predicate));
        }

        public static Logger Create(ITestOutputHelper outputHelper, LogEventLevel minimumLevel = LogEventLevel.Verbose, Func<LogEvent, bool>? predicate = null)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Is(minimumLevel)
                .Enrich.FromLogContext()
                .WriteTo.Sink(new TestOutputHelperLogSink(outputHelper, predicate))
                .WriteTo.Console(Formatter);

            return loggerConfiguration.CreateLogger();
        }

        public void Emit(LogEvent logEvent)
        {
            if (_predicate?.Invoke(logEvent) == false)
            {
                return;
            }

            using var writer = new StringWriter();
            
            Formatter.Format(logEvent, writer);
            _outputHelper.WriteLine(writer.ToString());
        }
    }
}