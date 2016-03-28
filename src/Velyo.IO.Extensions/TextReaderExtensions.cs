using System.Diagnostics;

namespace System.IO
{
    /// <summary>
    /// Extension methods for <see cref="System.IO.TextReader"/>
    /// </summary>
    [DebuggerStepThrough]
    internal static class TextReaderExtensions
    {
        /// <summary>
        /// Executes an action on each text line of the <code>TextReader</code>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void ForEachLine(this TextReader reader, Action<string> action)
        {
            #region - Exceptions -

            if (reader == null) throw new ArgumentNullException("reader");
            if (action == null) throw new ArgumentNullException("action");

            #endregion

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                action(line);
            }
        }
    }
}
