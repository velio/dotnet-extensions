using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Extensions methods for <see cref="System.Int32"/>.
    /// </summary>
    public static class Int32Extensions
    {
        #region Static Methods //////////////////////////////////////////////////////////

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
        #endregion
    }
}
