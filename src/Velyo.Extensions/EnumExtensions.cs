using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="System.Enum"/>.
    /// </summary>
    public static class EnumExtensions
    {
        #region Static Fields ///////////////////////////////////////////////////////////

        static readonly Dictionary<string, string> _DescriptionsTable = new Dictionary<string, string>();

        #endregion

        #region Methods /////////////////////////////////////////////////////////////////

        public static string Description(this Enum value)
        {
            Type enumType = value.GetType();
            string valueString = value.ToString();
            string key = enumType.ToString() + "__" + valueString;
            if (!_DescriptionsTable.ContainsKey(key))
            {
                FieldInfo info = enumType.GetField(valueString);
                if (info != null)
                {
                    var attrs = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if ((attrs != null) && (attrs.Length > 0))
                    {
                        return (_DescriptionsTable[key] = attrs[0].Description);
                    }
                    else
                    {
                        return (_DescriptionsTable[key] = valueString);
                    }
                }
                else
                {
                    return (_DescriptionsTable[key] = valueString);
                }
            }
            return _DescriptionsTable[key];
        }
        #endregion
    }
}
