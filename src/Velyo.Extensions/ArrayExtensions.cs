using System.Collections;
using System.Diagnostics;

namespace System
{
    /// <summary>
    /// Extension methods for .NET arrays.
    /// </summary>
    [DebuggerStepThrough]
    internal static class ArrayExtensions
    {
        /// <summary>
        /// Shuffles the specified origin.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="origin">The origin.</param>
        /// <returns></returns>
        public static T[] Shuffle<T>(this T[] origin)
        {

            var matrix = new SortedList();
            var r = new Random();

            for (var x = 0; x <= origin.GetUpperBound(0); x++)
            {
                var i = r.Next();
                while (matrix.ContainsKey(i))
                {
                    i = r.Next();
                }
                matrix.Add(i, origin[x]);
            }

            var output = new T[origin.Length];
            matrix.Values.CopyTo(output, 0);

            return output;
        }
    }
}
