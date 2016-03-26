namespace System.IO
{
    /// <summary>
    /// Extension methods for <see cref="System.IO.FileInfo" />
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Executes an action on each text line of the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="System.ArgumentNullException">file</exception>
        public static void ForEachLine(this FileInfo file, Action<string> action)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (action == null) throw new ArgumentNullException("action");

            using (StreamReader reader = file.OpenText())
            {
                reader.ForEachLine(action);
            }
        }
    }
}
