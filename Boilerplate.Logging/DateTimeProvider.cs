using System;

namespace Boilerplate.Logging
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset Now => DateTimeOffset.Now;
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}