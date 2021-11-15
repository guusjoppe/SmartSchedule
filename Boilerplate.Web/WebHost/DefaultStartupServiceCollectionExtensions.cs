using System;
using System.Net.Http;
using Boilerplate.Logging;
using Hellang.Middleware.ProblemDetails;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Boilerplate.Web.WebHost
{
    [PublicAPI]
    public static class DefaultStartupServiceCollectionExtensions
    {
        public static IServiceCollection AddDebugInfoToContext(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddOptions<MvcOptions>().Configure<IHostEnvironment, ProjectInfo>((o, env, info) =>
                {
                    if (env.IsDevelopmentLike())
                    {
                        o.Filters.Add(new DebugInfoActionFilter(env, info));
                    }
                }).Services;

        public static IServiceCollection AddDateTimeDefaults(this IServiceCollection services) => services
            .AddTransient<IDateTimeProvider, DateTimeProvider>();

        public static IServiceCollection AddProblemDetailsDefaults(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment) => services
            .AddProblemDetails(o =>
            {
                // https://github.com/khellang/Middleware/blob/a8a477c41e26aee93a4e12dd9f963ae0d84aa499/samples/ProblemDetails.Sample/Program.cs
                o.IncludeExceptionDetails = ctx => environment.IsDevelopment();

                o.Map<NotImplementedException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status501NotImplemented));
                o.Map<HttpRequestException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status503ServiceUnavailable));
                o.Map<Exception>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
            })
            .Configure<ApiBehaviorOptions>(o =>
            {
                // https://lurumad.github.io/problem-details-an-standard-way-for-specifying-errors-in-http-api-responses-asp.net-core
                o.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Type = "https://httpstatuses.com/400",
                        Detail = "Please refer to the errors property for additional details.",
                    };
                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes =
                        {
                            "application/problem+json",
                            "application/problem+xml",
                            "application/json",
                        },
                    };
                };
            });

        public static IServiceCollection AddProjectInfo<TStartup>(this IServiceCollection services) => services
            .AddSingleton(typeof(TStartup).Assembly.GetProjectInfo());
    }
}