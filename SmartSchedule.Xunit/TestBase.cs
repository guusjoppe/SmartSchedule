using System;
using JetBrains.Annotations;
using SmartSchedule.Xunit;
using Xunit.Abstractions;

namespace SmartSchedule.Xunit
{
    [PublicAPI]
    public class TestBase : IDisposable
    {
        protected ITestOutputHelper OutputHelper { get; }
        protected ITestLogSession LogCapture { get; }

        public TestBase(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
            LogCapture = LoggingHelper.Capture(outputHelper);
        }

        public virtual void Dispose()
        {
            LogCapture?.Dispose();
        }
    }
}