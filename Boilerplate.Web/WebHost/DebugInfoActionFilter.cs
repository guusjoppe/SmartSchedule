using Boilerplate.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace Boilerplate.Web.WebHost
{
    public class DebugInfoActionFilter : IActionFilter
    {
        private readonly IHostEnvironment _environment;
        private readonly ProjectInfo _projectInfo;

        public DebugInfoActionFilter(IHostEnvironment environment, ProjectInfo projectInfo)
        {
            _environment = environment;
            _projectInfo = projectInfo;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                controller.ViewData["DebugInfo.Environment"] = _environment.EnvironmentName;
                controller.ViewData["DebugInfo.GitBranch"] = _projectInfo.GitVersion.Branch;
                controller.ViewData["DebugInfo.GitCommit"] = _projectInfo.GitVersion.Commit;
                controller.ViewData["DebugInfo.ProjectVersion"] = _projectInfo.ProjectVersion.ToString();
                controller.ViewData["DebugInfo.ProjectName"] = _projectInfo.ProjectName;
            }
        }
    }
}