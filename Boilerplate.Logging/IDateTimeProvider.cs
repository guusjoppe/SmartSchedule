using System;

namespace Boilerplate.Logging
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now { get; }
        DateTimeOffset UtcNow { get; }
    }
}