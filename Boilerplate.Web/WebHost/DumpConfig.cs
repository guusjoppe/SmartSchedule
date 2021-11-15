using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Boilerplate.Web.WebHost
{
    public static class DumpConfig
    {
        public static void ForWebHost(IHost webHost)
        {
            var configuration = webHost.Services.GetRequiredService<IConfiguration>();
            if (configuration.GetValue("AllowConfigDump", false))
            {
                foreach (var (key, value) in configuration.AsEnumerable().Where(pair => pair.Value != null))
                {
                    Console.WriteLine($"{key}: {value}");
                }
            }
        }

        public static bool ForWebHostIfCommandLine(IHost webHost, string[] args)
        {
            if (args.Length != 1 || args[0] != "--dump-config")
            {
                return false;
            }
            
            ForWebHost(webHost);

            return true;
        }
    }
}