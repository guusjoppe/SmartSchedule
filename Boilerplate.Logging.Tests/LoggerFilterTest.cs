using System.Linq;
using Serilog;
using Serilog.Events;
using Xunit;

namespace Boilerplate.Logging.Tests
{
    public class LoggerFilterTest
    {
        [Theory]
        [InlineData("A", false, true, true, true, true, true)]
        [InlineData("B", false, false, true, true, true, true)]
        public void TestLoggerFilter(string sourceContext, bool passVerbose, bool passDebug, bool passInformation, bool passWarning, bool passError, bool passFatal)
        {
            var logEventSink = new CollectorSink();
            var loggerConfiguration = new LoggerConfiguration().MinimumLevel.Verbose()
                .WriteTo.Logger(c => c.Filter.LogLevelFromConfiguration(new MinimumLevel
                {
                    Default = LogEventLevel.Debug,
                    Override =
                    {
                        ["B"] = LogEventLevel.Information,
                    },
                }).WriteTo.Sink(logEventSink));
            
            var logger = loggerConfiguration
                .CreateLogger()
                .ForContext("SourceContext", sourceContext);

            logger.Verbose("Verbose");
            logger.Debug("Debug");
            logger.Information("Information");
            logger.Warning("Warning");
            logger.Error("Error");
            logger.Fatal("Fatal");
            
            Assert.Equal(passVerbose, logEventSink.LogEvents.Any(e => e.Level == LogEventLevel.Verbose));
            Assert.Equal(passDebug, logEventSink.LogEvents.Any(e => e.Level == LogEventLevel.Debug));
            Assert.Equal(passInformation, logEventSink.LogEvents.Any(e => e.Level == LogEventLevel.Information));
            Assert.Equal(passWarning, logEventSink.LogEvents.Any(e => e.Level == LogEventLevel.Warning));
            Assert.Equal(passError, logEventSink.LogEvents.Any(e => e.Level == LogEventLevel.Error));
            Assert.Equal(passFatal, logEventSink.LogEvents.Any(e => e.Level == LogEventLevel.Fatal));
        }
    }
}