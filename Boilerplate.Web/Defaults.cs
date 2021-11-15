using Boilerplate.Logging;
using Boilerplate.Web.WebHost;
using Hellang.Middleware.ProblemDetails;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Boilerplate.Web
{
    [PublicAPI]
    public static class Defaults
    {
        public static void EntryPoint<TStartup>(string[] args) where TStartup : class
        {
            DefaultProgram.EntryPoint<TStartup>(args);
        }

        public static IServiceCollection AddDefaultServices<TStartup>(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment,
            DefaultsConfiguration? defaultsConfiguration = null)
        {
            defaultsConfiguration ??= new DefaultsConfiguration();

            if (defaultsConfiguration.EnableDateTimeService)
            {
                services.AddDateTimeDefaults();
            }

            if (defaultsConfiguration.EnableProblemDetails)
            {
                services.AddProblemDetailsDefaults(configuration, environment);
            }

            if (defaultsConfiguration.EnableProjectInfoInjection)
            {
                services.AddProjectInfo<TStartup>();
                services.AddDebugInfoToContext();
            }

            return services;
        }

        public static IApplicationBuilder UseDefaultMiddleware<TStartup>(this IApplicationBuilder app, DefaultsConfiguration? defaultsConfiguration = null)
        {
            defaultsConfiguration ??= new DefaultsConfiguration();
            
            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var environment = app.ApplicationServices.GetRequiredService<IHostEnvironment>();

            if (defaultsConfiguration.EnableProxySupport)
            {
                app.UseProxySettings(configuration);
            }

            if (defaultsConfiguration.EnableProjectInfoInjection)
            {
                app.UseProjectInfoHeaders<TStartup>(environment);
            }

            if (defaultsConfiguration.EnableDevelopmentExceptionPage)
            {
                if (environment.IsDevelopmentLike())
                {
                    app.UseDeveloperExceptionPage();
                }
            }
            
            var serilogConfiguration = configuration.GetSection("Serilog").Get<SerilogConfiguration>();

            if (serilogConfiguration.RequestLogging.Enabled)
            {
                app.UseSerilogRequestLogging();
            }

            if (defaultsConfiguration.EnableProblemDetails)
            {
                app.UseProblemDetails();
            }

            if (defaultsConfiguration.EnableLocalization)
            {
                app.UseRequestLocalizationDefaults();
            }

            return app;
        }
    }
}