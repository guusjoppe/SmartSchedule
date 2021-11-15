using System;
using Boilerplate.Logging;
using Boilerplate.Web;
using JetBrains.Annotations;

namespace SmartSchedule.Xunit
{
    [PublicAPI]
    public class MockDateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset Now => (ConstantDateTime ?? DateTimeOffset.Now) + Offset;
        public DateTimeOffset UtcNow => Now.ToUniversalTime();

        public DateTimeOffset? ConstantDateTime { get; set; }
        public TimeSpan Offset { get; set; } = TimeSpan.Zero;

        public static MockDateTimeProvider Constant(DateTimeOffset? dateTime = null) => new MockDateTimeProvider { ConstantDateTime = dateTime ?? DateTimeOffset.Now };
    }
}