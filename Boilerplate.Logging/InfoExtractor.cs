using System.Linq;
using System.Reflection;

namespace Boilerplate.Logging
{
    public static class InfoExtractor
    {
        public static GitVersionAttribute? GetGitVersion(this Assembly assembly)
        {
            return assembly.GetCustomAttributes<GitVersionAttribute>().FirstOrDefault();
        }

        public static ProjectInfo GetProjectInfo(this Assembly assembly)
        {
            return new ProjectInfo(
                assembly.GetGitVersion() ?? new GitVersionAttribute("unknown", "unknown"),
                assembly.GetName().Version!,
                assembly.GetName().Name!);
        }
    }
}
