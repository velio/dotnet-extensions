using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="System.Enum"/>.
    /// </summary>
    [DebuggerStepThrough]
    internal static class EnumExtensions
    {
        static readonly Dictionary<string, string> _DescriptionsTable = new Dictionary<string, string>();


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
    }
}
