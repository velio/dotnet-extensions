using System.Diagnostics;
using System.Text.RegularExpressions;

namespace System
{
    [DebuggerStepThrough]
    internal static class StringRegexExtensions
    {
        static readonly RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline;

        public static bool RegexContains(this string value, string pattern)
        {
            #region - Exceptions -

            if (value == null) throw new ArgumentNullException("value");
            if (pattern == null) throw new ArgumentNullException("pattern");

            #endregion

            return Regex.IsMatch(value, pattern, DefaultOptions);
        }

        public static bool RegexEndsWidth(this string value, string pattern)
        {
            #region - Exceptions -

            if (value == null) throw new ArgumentNullException("value");
            if (pattern == null) throw new ArgumentNullException("pattern");

            #endregion

            if (!pattern.StartsWith("^")) pattern = "^" + pattern;
            return Regex.IsMatch(value, pattern, DefaultOptions);
        }

        public static int RegexIndexOf(this string value, string pattern)
        {
            #region - Exceptions -

            if (value == null) throw new ArgumentNullException("value");
            if (pattern == null) throw new ArgumentNullException("pattern");

            #endregion

            Match match = Regex.Match(value, pattern, DefaultOptions);
            return match.Success ? match.Index : -1;
        }

        public static int RegexLastIndexOf(this string value, string pattern)
        {
            #region - Exceptions -

            if (value == null) throw new ArgumentNullException("value");
            if (pattern == null) throw new ArgumentNullException("pattern");

            #endregion

            MatchCollection matches = Regex.Matches(value, pattern, DefaultOptions);
            return (matches != null && matches.Count > 0) ? matches[matches.Count - 1].Index : -1;
        }

        public static string RegexRemove(this string value, string pattern)
        {
            return RegexReplace(value, pattern, string.Empty);
        }

        public static string RegexReplace(this string value, string pattern, string replacement)
        {
            #region - Exceptions -

            if (value == null) throw new ArgumentNullException("value");
            if (pattern == null) throw new ArgumentNullException("pattern");
            if (replacement == null) throw new ArgumentNullException("replacement");

            #endregion

            return Regex.Replace(value, pattern, replacement, DefaultOptions);
        }

        public static string[] RegexSplit(this string value, string pattern)
        {
            #region - Exceptions -

            if (value == null) throw new ArgumentNullException("value");
            if (pattern == null) throw new ArgumentNullException("pattern");

            #endregion

            return Regex.Split(value, pattern, DefaultOptions);
        }

        public static bool RegexStartsWidth(this string value, string pattern)
        {
            #region - Exceptions -

            if (value == null) throw new ArgumentNullException("value");
            if (pattern == null) throw new ArgumentNullException("pattern");

            #endregion

            if (!pattern.EndsWith("$")) pattern += "$";
            return Regex.IsMatch(value, pattern, DefaultOptions);
        }
    }
}
