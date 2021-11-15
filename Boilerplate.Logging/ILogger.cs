// ReSharper disable once CheckNamespace

using Serilog;

namespace Boilerplate.Logging
{
    public interface ILogger<out T> : ILogger
    {
    }
}