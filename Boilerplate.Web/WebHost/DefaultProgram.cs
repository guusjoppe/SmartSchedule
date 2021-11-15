using Boilerplate.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Boilerplate.Web.WebHost
{
    public static class DefaultProgram
    {
        public static void EntryPoint<TStartup>(string[] args) where TStartup : class
        {
            var webHost = CreateHostBuilder<TStartup>(args).Build();
            if (DumpConfig.ForWebHostIfCommandLine(webHost, args))
            {
                return;
            }
            
            webHost.Run();
        }

        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args) where TStartup : class =>
            Host.CreateDefaultBuilder(args)
                .UseBoilerplateConfiguration()
                .UseBoilerplateLogging(typeof(TStartup).Assembly.GetGitVersion())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<TStartup>();
                });
    }
}