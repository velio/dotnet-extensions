using System.Diagnostics;
using System.Linq;

namespace System
{
    [DebuggerStepThrough]
    internal static class TypeExtensions
    {
        public static bool IsIn<T>(this T value, params T[] list)
        {
            return list.Contains(value);
        }
    }
}
