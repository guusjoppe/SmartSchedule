using System.Globalization;
using Boilerplate.Logging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Boilerplate.Web.WebHost
{
    [PublicAPI]
    public static class DefaultStartupApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestLocalizationDefaults(this IApplicationBuilder app)
        {
            return app.UseRequestLocalization(o =>
            {
                o.DefaultRequestCulture = new RequestCulture("nl-NL");
                o.SupportedCultures = new[] { new CultureInfo("nl-NL") };
                o.SupportedUICultures = new[] { new CultureInfo("nl-NL") };
            });
        }

        public static IApplicationBuilder UseProjectInfoHeaders<TStartup>(this IApplicationBuilder app, IHostEnvironment environment)
        {
            if (environment.IsDevelopmentLike())
            {
                app.ApplicationServices.GetRequiredService<ILogger<TStartup>>().Warning("Running in development mode");
                var info = app.ApplicationServices.GetRequiredService<ProjectInfo>();

                app.Use(async (ctx, next) =>
                {
                    ctx.Response.Headers.Add("X-Environment", environment.EnvironmentName);
                    ctx.Response.Headers.Add("X-Git-Branch", info.GitVersion.Branch);
                    ctx.Response.Headers.Add("X-Git-Commit", info.GitVersion.Commit);
                    ctx.Response.Headers.Add("X-Project-Version", info.ProjectVersion.ToString());
                    ctx.Response.Headers.Add("X-Project-Name", info.ProjectName);
                    await next();
                });
            }

            return app;
        }

        public static IApplicationBuilder UseProxySettings(this IApplicationBuilder app, IConfiguration configuration)
        {
            var proxySettings = configuration.GetSection("Proxy").Get<ProxySettings>();
            if (proxySettings?.UseForwardedHeaders == true)
            {
                app.UseForwardedHeaders();
            }

            return app;
        }
    }
}