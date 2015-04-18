using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="System.Guid"/>.
    /// </summary>
    public static class GuidExtensions
    {
        #region Static Methods //////////////////////////////////////////////////////////

        /// <summary>
        /// Converts a string representation of <see cref="System.Guid"/> to a <c>System.Guid</c>.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool TryParse(this Guid guid, string value)
        {
            if (value.IsGuid())
            {
                guid = value.ToGuid();
                return true;
            }
            return false;
        }
        #endregion
    }
}
