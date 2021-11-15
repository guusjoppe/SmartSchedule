namespace Boilerplate.Web.WebHost
{
    internal class AzureKeyVaultConfiguration
    {
        public bool Enabled { get; set; }
        public string? VaultEndpoint { get; set; }
    }
}