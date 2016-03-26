namespace System.IO
{
    /// <summary>
    /// Extension methods for <see cref="System.IO.TextReader"/>
    /// </summary>
    public static class TextReaderExtensions
    {
        /// <summary>
        /// Executes an action on each text line of the <code>TextReader</code>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void ForEachLine(this TextReader reader, Action<string> action)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (action == null) throw new ArgumentNullException("action");

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                action(line);
            }
        }
    }
}
