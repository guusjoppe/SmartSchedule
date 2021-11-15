using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Serilog;

namespace Boilerplate.Logging
{
    [PublicAPI]
    public static class LogAndMeasureTimeExtensions
    {
        public static void Run(this LogAndMeasureTime measure, ILogger logger, Action action)
        {
            measure.Run(logger, () =>
            {
                action();
                return 0;
            });
        }

        public static async Task RunAsync(this LogAndMeasureTime measure, ILogger logger, Func<Task> action)
        {
            await measure.RunAsync(logger, async () =>
            {
                await action();
                return 0;
            });
        }
    }
}