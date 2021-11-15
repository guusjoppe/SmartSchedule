using System;

namespace Boilerplate.Logging
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class GitVersionAttribute : Attribute
    {
        public string Branch { get; }
        public string Commit { get; }
        
        public GitVersionAttribute(string branch, string commit)
        {
            Branch = string.IsNullOrWhiteSpace(branch) ? "unknown" : branch;
            Commit = string.IsNullOrWhiteSpace(commit) ? "unknown" : commit;
        }
    }
}
