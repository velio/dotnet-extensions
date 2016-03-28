using System.Diagnostics;

namespace System
{
    /// <summary>
    /// Extensions methods for <see cref="System.Int32"/>.
    /// </summary>
    [DebuggerStepThrough]
    internal static class Int32Extensions
    {
        /// <summary>
        /// Froms the hex string.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static int FromHexString(this string val)
        {
            return Convert.ToInt32(val, 16);
        }

        /// <summary>
        /// Toes the hex string.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static string ToHexString(this int val)
        {
            return string.Format("{0:x2}", val);
        }
    }
}
