using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Extension methods for .NET arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        #region Static Methods ////////////////////////////////////////////////////////////////////

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
        #endregion
    }
}
