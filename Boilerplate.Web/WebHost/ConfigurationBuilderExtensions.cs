using System;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;

namespace Boilerplate.Web.WebHost
{
    public static class ConfigurationBuilderExtensions
    {
        public static IHostBuilder UseBoilerplateConfiguration(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                using var defaultAppSettingsStream = typeof(ConfigurationBuilderExtensions).Assembly.GetManifestResourceStream("Boilerplate.Web.appsettings.Default.json");
                if (defaultAppSettingsStream == null)
                {
                    throw new InvalidOperationException("Could not load default appsettings from assembly resources");
                }
                
                // we need default app settings to be the first application. todo: make pretty
                var tempBuilder = new ConfigurationBuilder();
                tempBuilder.AddJsonStream(defaultAppSettingsStream);
                configurationBuilder.Sources.Insert(0, new ChainedConfigurationSource() { Configuration = tempBuilder.Build() } );
                configurationBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json");

                AddEnvironmentVariables(configurationBuilder);
                AddLocalAppSettings(context, configurationBuilder);
                AddExtraAppSettings(configurationBuilder);
                AddKeyVault(configurationBuilder);
            });
        }

        private static void AddExtraAppSettings(IConfigurationBuilder configurationBuilder)
        {
            var configurationRoot = configurationBuilder.Build();
            var configFiles = configurationRoot.GetSection("ConfigFiles").Get<ConfigFilesConfiguration>();
            if (configFiles == null)
            {
                return;
            }

            foreach (var jsonFiles in configFiles.Json)
            {
                Console.WriteLine($"Loading extra config {jsonFiles}");
                configurationBuilder.AddJsonFile(jsonFiles);
            }
        }

        private static void AddLocalAppSettings(HostBuilderContext context, IConfigurationBuilder configurationBuilder)
        {
            if (!context.HostingEnvironment.IsDevelopment())
            {
                return;
            }

            Console.WriteLine($"Loading extra config appsettings.Local.json");
            configurationBuilder.AddJsonFile("appsettings.Local.json", true);
        }

        private static void AddEnvironmentVariables(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddEnvironmentVariables();
        }

        private static void AddKeyVault(IConfigurationBuilder configurationBuilder)
        {
            var azureKeyVaultConfiguration =
                configurationBuilder.Build().GetSection("AzureKeyVault").Get<AzureKeyVaultConfiguration>() ??
                configurationBuilder.Build().GetSection("KeyVault").Get<AzureKeyVaultConfiguration>();

            if (azureKeyVaultConfiguration?.Enabled != true)
            {
                return;
            }

            // validate
            if (string.IsNullOrWhiteSpace(azureKeyVaultConfiguration.VaultEndpoint))
            {
                throw new Exception("AzureKeyVault:Enabled is true but AzureKeyVault:VaultEndpoint is not set");
            }
                
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            configurationBuilder.AddPrebuiltConfiguration(c => c.AddAzureKeyVault(
                azureKeyVaultConfiguration.VaultEndpoint!, keyVaultClient, new DefaultKeyVaultSecretManager()));
        }

        private static IConfigurationBuilder AddPrebuiltConfiguration(this IConfigurationBuilder configurationBuilder, Action<IConfigurationBuilder> prebuiltBuilderAction)
        {
            var prebuiltBuilder = new ConfigurationBuilder();
            prebuiltBuilderAction(prebuiltBuilder);
            return configurationBuilder.AddConfiguration(prebuiltBuilder.Build());
        }
    }
}