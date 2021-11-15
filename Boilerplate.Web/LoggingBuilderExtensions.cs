using Boilerplate.Logging;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Boilerplate.Web
{
    [PublicAPI]
    public static class LoggingBuilderExtensions
    {
        public static IHostBuilder UseBoilerplateLogging(this IHostBuilder hostBuilder, GitVersionAttribute? gitVersion)
        {
            return hostBuilder.UseSerilog((context, configuration) =>
            {
                var combinedConfigBuilder = new ConfigurationBuilder();
                combinedConfigBuilder.AddConfiguration(context.Configuration);
                
                SetupLoggerConfiguration.Configure(configuration, combinedConfigBuilder.Build(), gitVersion);
            }).ConfigureServices((context, services) =>
            {
                services.AddSingleton(s => Log.Logger);
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            });
        }
    }
}