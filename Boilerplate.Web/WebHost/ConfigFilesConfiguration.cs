using System.Collections.Generic;

namespace Boilerplate.Web.WebHost
{
    internal class ConfigFilesConfiguration
    {
        public ICollection<string> Json { get; set; } = new List<string>();
    }
}