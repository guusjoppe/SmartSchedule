using Microsoft.Extensions.Hosting;

namespace Boilerplate.Web
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsDevelopmentLike(this IHostEnvironment environment) 
        {
            return environment.EnvironmentName.Equals("test", System.StringComparison.InvariantCultureIgnoreCase)
                || environment.IsDevelopment();
        }
    }
}