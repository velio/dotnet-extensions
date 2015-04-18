using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Cryptography;
using System.IO;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="System.String"/>
    /// </summary>
    public static class StringExtensions
    {
        #region - Common -

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
        #endregion

        #region - Regex -

        public static bool RegexContains(this string value, string pattern)
        {
            return Regex.IsMatch(value, pattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static bool RegexEndsWidth(this string value, string pattern)
        {

            if (!pattern.StartsWith("^")) pattern = "^" + pattern;
            return Regex.IsMatch(value, pattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static int RegexIndexOf(this string value, string pattern)
        {

            Match match = Regex.Match(value, pattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return match.Success ? match.Index : -1;
        }

        public static int RegexLastIndexOf(this string value, string pattern)
        {

            MatchCollection matches = Regex.Matches(value, pattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return (matches != null && matches.Count > 0) ? matches[matches.Count - 1].Index : -1;
        }

        public static string RegexRemove(this string value, string pattern)
        {
            return RegexReplace(value, pattern, string.Empty);
        }

        public static string RegexReplace(this string value, string pattern, string replacement)
        {
            return Regex.Replace(value, pattern, replacement,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static string[] RegexSplit(this string value, string pattern)
        {
            return Regex.Split(value, pattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static bool RegexStartsWidth(this string value, string pattern)
        {

            if (!pattern.EndsWith("$")) pattern += "$";
            return Regex.IsMatch(value, pattern,
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }
        #endregion

        #region - Encryption -

        /// <summary>
        /// Decrypt the data String to the original String.  The data must be the base64 String
        /// returned from the EncryptData method.
        /// </summary>
        /// <param name="data">Encrypted data generated from EncryptData method.</param>
        /// <param name="password">Password used to decrypt the String.</param>
        /// <returns>Decrypted String.</returns>
        public static string DecryptData(this string data, string password)
        {

            if (data == null)
                throw new ArgumentNullException("data");
            if (password == null)
                throw new ArgumentNullException("password");
            byte[] encBytes = Convert.FromBase64String(data);
            byte[] decBytes = encBytes.DecryptData(password, PaddingMode.ISO10126);
            return Encoding.UTF8.GetString(decBytes);
        }

        /// <summary>
        /// Use AES to encrypt data String. The output String is the encrypted bytes as a base64 String.
        /// The same password must be used to decrypt the String.
        /// </summary>
        /// <param name="data">Clear String to encrypt.</param>
        /// <param name="password">Password used to encrypt the String.</param>
        /// <returns>Encrypted result as Base64 String.</returns>
        public static string EncryptData(this string data, string password)
        {

            if (data == null)
                throw new ArgumentNullException("data");
            if (password == null)
                throw new ArgumentNullException("password");
            byte[] encBytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(encBytes.EncryptData(password, PaddingMode.ISO10126));
        }
        #endregion
    }
}