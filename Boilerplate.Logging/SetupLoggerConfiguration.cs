using System;
using System.Collections.Generic;
using System.IO;
using Boilerplate.Logging.Utility;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Boilerplate.Logging
{
    [PublicAPI]
    public static class SetupLoggerConfiguration
    {
        public static void Configure(LoggerConfiguration loggerConfiguration, IConfiguration configuration, GitVersionAttribute? gitVersion)
        {
            Configure(loggerConfiguration, configuration, configuration.GetSection("Serilog").Get<SerilogConfiguration>(), gitVersion);
        }

        public static void Configure(LoggerConfiguration loggerConfiguration, IConfiguration configuration, SerilogConfiguration? serilogConfiguration, GitVersionAttribute? gitVersion)
        {
            serilogConfiguration ??= new SerilogConfiguration();

            loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName();

            if (serilogConfiguration.RequestLogging.Enabled)
            {
                loggerConfiguration.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
            }

            if (gitVersion != null)
            {
                loggerConfiguration
                    .Enrich.WithProperty("GitBranch", gitVersion.Branch)
                    .Enrich.WithProperty("GitCommit", gitVersion.Commit);
            }

            if (configuration != null)
            {
                loggerConfiguration.ReadFrom.Configuration(configuration);
            }

            SetupConsole(loggerConfiguration, serilogConfiguration.Console);
            SetupElasticSearch(loggerConfiguration, serilogConfiguration.ElasticSearch);
            SetupFileLog(loggerConfiguration, serilogConfiguration.FileLog);

            SetupSelfLog(serilogConfiguration.SelfLog);
        }

        private static void SetupSelfLog(SelfLogConfiguration selfLogConfiguration)
        {
            if (!selfLogConfiguration.Enabled)
            {
                return;
            }

            var selfLog = File.CreateText(selfLogConfiguration.Path);
            selfLog.WriteLine($"----------- INITIALIZED SELFLOG ----------- {DateTimeOffset.UtcNow} -----------");
            selfLog.AutoFlush = true;
            Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(selfLog));
        }

        private static void SetupConsole(LoggerConfiguration loggerConfiguration, ConsoleConfiguration consoleConfiguration)
        {
            if (!consoleConfiguration.Enabled)
            {
                return;
            }

            loggerConfiguration
                .WriteTo.Logger(c => c.Filter.LogLevelFromConfiguration(consoleConfiguration.MinimumLevel)
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <{SourceContext}>{NewLine}{Exception}"));
        }

        private static void SetupElasticSearch(LoggerConfiguration loggerConfiguration, ElasticSearchConfiguration elasticSearchConfiguration)
        {
            if (!elasticSearchConfiguration.Enabled)
            {
                return;
            }

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            void ValidateConfiguration(string property, string? value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception($"Serilog.ElasticSearch.Enabled is true but Serilog.ElasticSearch.{property} is not set");
                }
            }

            ValidateConfiguration(nameof(ElasticSearchConfiguration.BaseUrl), elasticSearchConfiguration.BaseUrl);
            ValidateConfiguration(nameof(ElasticSearchConfiguration.Username), elasticSearchConfiguration.Username);
            ValidateConfiguration(nameof(ElasticSearchConfiguration.Password), elasticSearchConfiguration.Password);
            ValidateConfiguration(nameof(ElasticSearchConfiguration.BaseUrl), elasticSearchConfiguration.BaseUrl);
            ValidateConfiguration(nameof(ElasticSearchConfiguration.IndexFormat), elasticSearchConfiguration.IndexFormat);

            var uri = string.Format(elasticSearchConfiguration.BaseUrl!, elasticSearchConfiguration.Username!,
                elasticSearchConfiguration.Password!);

            loggerConfiguration
                .WriteTo.Logger(c => c
                    .Filter.LogLevelFromConfiguration(elasticSearchConfiguration.MinimumLevel)
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(uri))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        IndexFormat = elasticSearchConfiguration.IndexFormat,
                        // BufferBaseFilename = "./logs/buffer"
                    }));
        }

        private static void SetupFileLog(LoggerConfiguration loggerConfiguration, FileLogConfiguration fileLogConfiguration)
        {
            if (!fileLogConfiguration.Enabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(fileLogConfiguration.Path))
            {
                throw new Exception("Serilog.ElasticSearch.Enabled is true but Serilog.FileLog.Path is not set");
            }

            loggerConfiguration.WriteTo.Logger(c => c
                .Filter.LogLevelFromConfiguration(fileLogConfiguration.MinimumLevel)
                .WriteTo.File(
                    fileLogConfiguration.Path,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {SourceContext}]{NewLine}{Message:lj}{NewLine}{Exception}"));
        }

        internal static LoggerConfiguration LogLevelFromConfiguration(this LoggerFilterConfiguration loggerFilterConfiguration, MinimumLevel logLevels)
        {
            return loggerFilterConfiguration.With(new LogLevelFilter(logLevels.Default, logLevels.Override));
        }

        private class LogLevelFilter : ILogEventFilter
        {
            private readonly LogEventLevel _default;
            private readonly TypeNameTrie<LogEventLevel> _overrides;

            public LogLevelFilter(LogEventLevel @default, IDictionary<string, LogEventLevel> overrides)
            {
                _default = @default;
                _overrides = new TypeNameTrie<LogEventLevel>(overrides);
            }

            public bool IsEnabled(LogEvent logEvent)
            {
                return logEvent.Level >= GetEffectiveLevel(logEvent);
            }

            private LogEventLevel GetEffectiveLevel(LogEvent logEvent)
            {
                var context = GetSourceContext(logEvent);

                if (context == null || !_overrides.GetLongestMatch(context, out var minimumLevel))
                {
                    return _default;
                }

                return minimumLevel;
            }

            private string? GetSourceContext(LogEvent logEvent)
            {
                if (!logEvent.Properties.TryGetValue(Constants.SourceContextPropertyName, out var sourceContextProperty))
                {
                    return null;
                }

                if (sourceContextProperty is ScalarValue value && value.Value is string context)
                {
                    return context;
                }

                return null;
            }
        }
    }
}