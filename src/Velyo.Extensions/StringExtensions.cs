using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="System.String"/>
    /// </summary>
    [DebuggerStepThrough]
    internal static class StringExtensions
    {
        public static string F(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        public static bool IsGuid(this string value)
        {
            if (value != null)
                return Regex.IsMatch(value, @"^{?([0-9a-fA-F]){8}(-([0-9a-fA-F]){4}){3}-([0-9a-fA-F]){12}}?$",
                    RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            return false;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return (value == null || value.Length == 0);
        }

        public static DateTime ToDateTime(this string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date) ? date : DateTime.MinValue;
        }

        public static Guid ToGuid(this string value)
        {
            return value.IsGuid() ? new Guid(value) : Guid.Empty;
        }

        public static TimeSpan ToTimeSpan(this string time)
        {
            string[] spans = time.Split(':');
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            switch (spans.Length)
            {
                case 1:
                    int.TryParse(spans[0], out seconds);
                    break;
                case 2:
                    int.TryParse(spans[0], out minutes);
                    int.TryParse(spans[1], out seconds);
                    break;
                case 3:
                    int.TryParse(spans[0], out hours);
                    int.TryParse(spans[1], out minutes);
                    int.TryParse(spans[2], out seconds);
                    break;
            }
            return new TimeSpan(hours, minutes, seconds);
        }

        public static string TrimToSize(this string value, int size)
        {
            if (value.Length + 3 > size)
            {
                StringBuilder buffer = new StringBuilder(value);
                buffer.Length = size;
                buffer.Append("...");
                return buffer.ToString();
            }
            return value;
        }
    }
}