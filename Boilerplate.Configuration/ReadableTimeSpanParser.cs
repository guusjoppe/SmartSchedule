using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Boilerplate.Configuration
{
    [PublicAPI]
    public static class ReadableTimeSpanParser
    {
        public static readonly Regex TimeSpanParsingExpression = new Regex(@"(\d*(?:\.\d*)?)\s*([smhdw])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Parses a string as a time span, formatted in a simple to read way. It is a simple concatenation of a number and a unit.
        /// Supported units are second s, minute m, hour h and day d. E.g. 1d or 5h.
        /// </summary>
        /// <param name="timeSpanString">The readable time span.</param>
        /// <returns>A parsed timespan</returns>
        /// <exception cref="ArgumentNullException"><paramref name="timeSpanString"/> is required.</exception>
        /// <exception cref="ArgumentException"><paramref name="timeSpanString"/> must have a valid value.</exception>
        public static TimeSpan ParseTimeSpan(string timeSpanString)
        {
            if (timeSpanString == null)
            {
                throw new ArgumentNullException(nameof(timeSpanString));
            }

            if (string.IsNullOrWhiteSpace(timeSpanString))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(timeSpanString));
            }

            var match = TimeSpanParsingExpression.Match(timeSpanString);
            if (!match.Success)
            {
                throw new ArgumentException("Value does not match expected timespan pattern. Example: 1d or 5h", nameof(timeSpanString));
            }

            var amount = double.Parse(match.Groups[1].Value);

            switch (match.Groups[2].Value.ToLower())
            {
                case "s": return TimeSpan.FromSeconds(amount);
                case "m": return TimeSpan.FromMinutes(amount);
                case "h": return TimeSpan.FromHours(amount);
                case "d": return TimeSpan.FromDays(amount);
                case "w": return TimeSpan.FromDays(amount * 7);
                default: throw new ArgumentException("Time unit is not supported. Supported: s (seconds), m (minutes), h (hours), d (days)", nameof(timeSpanString));
            }
        }

        /// <summary>
        /// Tries parsing a string as a time span, formatted in a simple to read way. It is a simple concatenation of a number and a unit.
        /// Supported units are second s, minute m, hour h and day d. E.g. 1d or 5h.
        /// </summary>
        /// <param name="timeSpanString">The readable time span.</param>
        /// <param name="timeSpan">The parsed timespan, or. <code>default(TimeSpan)</code></param>
        /// <returns>Whether parsing succeeded.</returns>
        public static bool TryParseTimeSpan(string timeSpanString, out TimeSpan timeSpan)
        {
            try
            {
                timeSpan = ParseTimeSpan(timeSpanString);
                return true;
            }
            catch
            {
                timeSpan = default;
                return false;
            }
        }

        /// <summary>
        /// Tries parsing a string as a time span, formatted in a simple to read way. It is a simple concatenation of a number and a unit.
        /// Supported units are second s, minute m, hour h and day d. E.g. 1d or 5h.
        /// </summary>
        /// <param name="timeSpanString">The readable time span</param>
        /// <param name="timeSpan">The parsed timespan, or <code>default(TimeSpan)</code></param>
        /// <param name="error">The parsing error</param>
        /// <returns>Whether parsing succeeded</returns>
        public static bool TryParseTimeSpan(string timeSpanString, out TimeSpan timeSpan, out string? error)
        {
            try
            {
                timeSpan = ParseTimeSpan(timeSpanString);
                error = null;
                return true;
            }
            catch (Exception exception)
            {
                timeSpan = default;
                error = exception.Message;
                return false;
            }
        }

        /// <summary>
        /// Tries parsing a string as a time span, formatted in a simple to read way. It is a simple concatenation of a number and a unit.
        /// Supported units are second s, minute m, hour h and day d. E.g. 1d or 5h
        /// </summary>
        /// <param name="timeSpanString">The readable time span</param>
        /// <param name="default">The default in case parsing fails</param>
        /// <returns>A parsed timespan, or @<paramref name="default"/> if parsing failed</returns>
        public static TimeSpan TryParseTimeSpan(string timeSpanString, TimeSpan @default = default)
        {
            return TryParseTimeSpan(timeSpanString, out var timeSpan) ? timeSpan : @default;
        }
    }
}
