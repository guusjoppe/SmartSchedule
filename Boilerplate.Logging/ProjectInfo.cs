using System;

namespace Boilerplate.Logging
{
    public class ProjectInfo
    {
        public GitVersionAttribute GitVersion { get; }

        public Version ProjectVersion { get; }

        public string ProjectName { get; }

        public ProjectInfo(GitVersionAttribute gitVersion, Version projectVersion, string projectName)
        {
            GitVersion = gitVersion;
            ProjectVersion = projectVersion;
            ProjectName = projectName;
        }
    }
}