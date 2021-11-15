using System;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Serilog;
using Serilog.Events;

namespace Boilerplate.Logging
{
    [PublicAPI]
    public class LogAndMeasureTime
    {
        public LogEventLevel SuccessLevel { get; }
        public LogEventLevel FailureLevel { get; }

        public string Subject { get; set; }
        public string? SubjectPrefix { get; set; }

        public LogAndMeasureTime(string subject, LogEventLevel successLevel = LogEventLevel.Information, LogEventLevel failureLevel = LogEventLevel.Error)
        {
            Subject = subject;
            SuccessLevel = successLevel;
            FailureLevel = failureLevel;
        }

        public LogAndMeasureTime(LogEventLevel successLevel = LogEventLevel.Information, LogEventLevel failureLevel = LogEventLevel.Error)
            : this("Subject", successLevel, failureLevel)
        {
        }

        public static LogAndMeasureTime ForFunction { get; } = new LogAndMeasureTime("Function");

        public LogAndMeasureTime For(string subject) => new LogAndMeasureTime(subject, SuccessLevel, FailureLevel) { SubjectPrefix = SubjectPrefix };

        public T Run<T>(ILogger logger, Func<T> action)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var result = action();
                LogSuccess(logger, sw);
                return result;
            }
            catch (Exception ex)
            {
                LogError(logger, sw, ex);
                throw;
            }
        }

        public async Task<T> RunAsync<T>(ILogger logger, Func<Task<T>> action)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var result = await action();
                LogSuccess(logger, sw);
                return result;
            }
            catch (Exception ex)
            {
                LogError(logger, sw, ex);
                throw;
            }
        }

        private void LogSuccess(ILogger logger, Stopwatch sw)
        {
            logger.Write(SuccessLevel, "{Subject} execution {Status} after {Elapsed}ms", SubjectPrefix + Subject, "Succeeded", sw.ElapsedMilliseconds);
        }

        private void LogError(ILogger logger, Stopwatch sw, Exception ex)
        {
            logger.Write(FailureLevel, ex, "{Subject} execution {Status} after {Elapsed}ms", SubjectPrefix + Subject, "Failed", sw.ElapsedMilliseconds);
        }
    }
}