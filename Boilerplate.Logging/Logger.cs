using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Boilerplate.Logging
{
    /// <summary>
    /// Emulates <see cref="Boilerplate.Logging.ILogger{T}"/> in that it automatically sets the source context.
    /// </summary>
    /// <typeparam name="T">verplicht :(.</typeparam>
    public class Logger<T> : ILogger<T>
    {
        private static readonly Type Type = typeof(ILogger<T>).GetGenericArguments()[0];

        private readonly ILogger _logger;

        public Logger(ILogger logger)
        {
            this._logger = logger.ForContext(Type);
        }

        public ILogger ForContext(ILogEventEnricher enricher)
        {
            return _logger.ForContext(enricher);
        }

        public ILogger ForContext(IEnumerable<ILogEventEnricher> enrichers)
        {
            return _logger.ForContext(enrichers);
        }

        public ILogger ForContext(string propertyName, object value, bool destructureObjects = false)
        {
            return _logger.ForContext(propertyName, value, destructureObjects);
        }

        public ILogger ForContext<TSource>()
        {
            return _logger.ForContext<TSource>();
        }

        public ILogger ForContext(Type source)
        {
            return _logger.ForContext(source);
        }

        public void Write(LogEvent logEvent)
        {
            _logger.Write(logEvent);
        }

        public void Write(LogEventLevel level, string messageTemplate)
        {
            _logger.Write(level, messageTemplate);
        }

        public void Write<T1>(LogEventLevel level, string messageTemplate, T1 propertyValue)
        {
            _logger.Write(level, messageTemplate, propertyValue);
        }

        public void Write<T0, T1>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Write(level, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Write<T0, T1, T2>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Write(level, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Write(LogEventLevel level, string messageTemplate, params object[] propertyValues)
        {
            _logger.Write(level, messageTemplate, propertyValues);
        }

        public void Write(LogEventLevel level, Exception exception, string messageTemplate)
        {
            _logger.Write(level, exception, messageTemplate);
        }

        public void Write<T1>(LogEventLevel level, Exception exception, string messageTemplate, T1 propertyValue)
        {
            _logger.Write(level, exception, messageTemplate, propertyValue);
        }

        public void Write<T0, T1>(LogEventLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Write<T0, T1, T2>(LogEventLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Write(LogEventLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Write(level, exception, messageTemplate, propertyValues);
        }

        public bool IsEnabled(LogEventLevel level)
        {
            return _logger.IsEnabled(level);
        }

        public void Verbose(string messageTemplate)
        {
            _logger.Verbose(messageTemplate);
        }

        public void Verbose<T1>(string messageTemplate, T1 propertyValue)
        {
            _logger.Verbose(messageTemplate, propertyValue);
        }

        public void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Verbose(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            _logger.Verbose(messageTemplate, propertyValues);
        }

        public void Verbose(Exception exception, string messageTemplate)
        {
            _logger.Verbose(exception, messageTemplate);
        }

        public void Verbose<T1>(Exception exception, string messageTemplate, T1 propertyValue)
        {
            _logger.Verbose(exception, messageTemplate, propertyValue);
        }

        public void Verbose<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Verbose<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Verbose(exception, messageTemplate, propertyValues);
        }

        public void Debug(string messageTemplate)
        {
            _logger.Debug(messageTemplate);
        }

        public void Debug<T1>(string messageTemplate, T1 propertyValue)
        {
            _logger.Debug(messageTemplate, propertyValue);
        }

        public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Debug(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            _logger.Debug(messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate)
        {
            _logger.Debug(exception, messageTemplate);
        }

        public void Debug<T1>(Exception exception, string messageTemplate, T1 propertyValue)
        {
            _logger.Debug(exception, messageTemplate, propertyValue);
        }

        public void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Debug(exception, messageTemplate, propertyValues);
        }

        public void Information(string messageTemplate)
        {
            _logger.Information(messageTemplate);
        }

        public void Information<T1>(string messageTemplate, T1 propertyValue)
        {
            _logger.Information(messageTemplate, propertyValue);
        }

        public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Information(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            _logger.Information(messageTemplate, propertyValues);
        }

        public void Information(Exception exception, string messageTemplate)
        {
            _logger.Information(exception, messageTemplate);
        }

        public void Information<T1>(Exception exception, string messageTemplate, T1 propertyValue)
        {
            _logger.Information(exception, messageTemplate, propertyValue);
        }

        public void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Information(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Information(exception, messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate)
        {
            _logger.Warning(messageTemplate);
        }

        public void Warning<T1>(string messageTemplate, T1 propertyValue)
        {
            _logger.Warning(messageTemplate, propertyValue);
        }

        public void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Warning(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            _logger.Warning(messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate)
        {
            _logger.Warning(exception, messageTemplate);
        }

        public void Warning<T1>(Exception exception, string messageTemplate, T1 propertyValue)
        {
            _logger.Warning(exception, messageTemplate, propertyValue);
        }

        public void Warning<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Warning<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Warning(exception, messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate)
        {
            _logger.Error(messageTemplate);
        }

        public void Error<T1>(string messageTemplate, T1 propertyValue)
        {
            _logger.Error(messageTemplate, propertyValue);
        }

        public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Error(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            _logger.Error(messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate)
        {
            _logger.Error(exception, messageTemplate);
        }

        public void Error<T1>(Exception exception, string messageTemplate, T1 propertyValue)
        {
            _logger.Error(exception, messageTemplate, propertyValue);
        }

        public void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Error(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Error(exception, messageTemplate, propertyValues);
        }

        public void Fatal(string messageTemplate)
        {
            _logger.Fatal(messageTemplate);
        }

        public void Fatal<T1>(string messageTemplate, T1 propertyValue)
        {
            _logger.Fatal(messageTemplate, propertyValue);
        }

        public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Fatal(messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            _logger.Fatal(messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate)
        {
            _logger.Fatal(exception, messageTemplate);
        }

        public void Fatal<T1>(Exception exception, string messageTemplate, T1 propertyValue)
        {
            _logger.Fatal(exception, messageTemplate, propertyValue);
        }

        public void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            _logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            _logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            _logger.Fatal(exception, messageTemplate, propertyValues);
        }

        public bool BindMessageTemplate(string messageTemplate, object[] propertyValues, out MessageTemplate parsedTemplate, out IEnumerable<LogEventProperty> boundProperties)
        {
            return _logger.BindMessageTemplate(messageTemplate, propertyValues, out parsedTemplate, out boundProperties);
        }

        public bool BindProperty(string propertyName, object value, bool destructureObjects, out LogEventProperty property)
        {
            return _logger.BindProperty(propertyName, value, destructureObjects, out property);
        }
    }
}