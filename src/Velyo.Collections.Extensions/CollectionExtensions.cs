using System.Diagnostics;

namespace System.Collections
{
    /// <summary>
    /// Extension methods for generic collections.
    /// </summary>
    [DebuggerStepThrough]
    internal static class CollectionExtensions
    {
        /// <summary>
        /// Determines whether the specified collection is empty.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>
        /// 	<c>true</c> if the specified collection is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(this ICollection collection)
        {
            return collection != null && collection.Count == 0;
        }

        /// <summary>
        /// Determines whether the specified value is not empty.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>
        /// 	<c>true</c> if the specified collection is not empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotEmpty(this ICollection collection)
        {
            return !IsEmpty(collection);
        }

        /// <summary>
        /// Determines whether the specified collection is null.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>
        /// 	<c>true</c> if the specified collection is null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull(this ICollection collection)
        {
            return collection == null;
        }

        /// <summary>
        /// Determines whether the specified collection is not null.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>
        /// 	<c>true</c> if the specified collection is not null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNull(this ICollection collection)
        {
            return collection != null;
        }

        /// <summary>
        /// Determines whether the specified collection is null or empty.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>
        /// 	<c>true</c> if the specified collection is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this ICollection collection)
        {
            return (collection == null || collection.Count == 0);
        }

        /// <summary>
        /// Determines whether the specified collection is not null or empty.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>
        /// 	<c>true</c> if the specified collection is not null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNullOrEmpty(this ICollection collection)
        {
            return !IsNullOrEmpty(collection);
        }
    }
}
