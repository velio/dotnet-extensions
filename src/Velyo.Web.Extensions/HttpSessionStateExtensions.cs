using System;
using System.IO;
using System.Linq;
using System.Web;

namespace System.Web.SessionState
{
    /// <summary>
    /// Extension methods for <see cref="HttpSessionState"/>
    /// </summary>
    public static class HttpSessionStateExtensions
    {
        /// <summary>
        /// Tries to get a value from a session, if exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">The session.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        static public bool TryGet<T>(this HttpSessionState session, string key, out T value)
        {
            bool flag = false;
            value = default(T);

            if (session != null)
            {
                object objectValue = session[key];
                if (flag = (objectValue != null && objectValue is T)) value = (T)objectValue;
            }

            return flag;
        }
    }
}