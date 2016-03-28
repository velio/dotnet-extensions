using System;
using System.Reflection;
using System.Diagnostics;

namespace Artem
{
    /// <summary>
    /// Extension methods for <see cref="System.Exception"/>
    /// </summary>
    [DebuggerStepThrough]
    internal static class ExceptionExtensions
    {
        /// <summary>
        /// Preserves the original stack trace on re-throw Exception.
        /// We get the full call stack information where the exceptions were thrown.
        /// </summary>
        /// <param name="exception">The exception stack trace to be preserved before re-throw.</param>
        public static void PreserveStackTrace(this Exception exception)
        {
            MethodInfo preserveStackTrace = typeof(Exception).GetMethod(
                "InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);
            preserveStackTrace.Invoke(exception, null);
        }
    }
}
