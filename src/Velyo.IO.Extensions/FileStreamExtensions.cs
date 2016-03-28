using System.Diagnostics;

namespace System.IO
{
    /// <summary>
    /// Extension methods for <see cref="System.IO.FileStream"/>
    /// </summary>
    [DebuggerStepThrough]
    internal static class FileStreamExtensions
    {
        /// <summary>
        /// Deletes a block of bytes from the file stream.
        /// The block is defined by the offset and count arguments.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="offset">The offset in file stream where delete to starts.</param>
        /// <param name="count">The count of bytes to be deleted from file stream.</param>
        /// <exception cref="ArgumentNullException">Thrown when stream is <c>null.</c></exception>
        /// <exception cref="ArgumentException">Thrown when offset and count describe an invalid range in the file stream.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when offset or count is negative.</exception>
        /// <exception cref="InvalidOperationException">Thrown when cannot seek or write in the file stream.</exception>
        public static void DeleteBlock(this FileStream stream, long offset, int count)
        {
            #region - Exceptions -

            if (stream == null) throw new ArgumentNullException("stream");

            if (offset < 0) throw new ArgumentOutOfRangeException("offset", "offset is negative");
            if (count < 0) throw new ArgumentOutOfRangeException("count", "count is negative");

            if (stream.Length < (offset + count)) throw new ArgumentException("offset and count describe an invalid range in the file stream");

            if (!stream.CanSeek) throw new InvalidOperationException("cannot seek in the stream");
            if (!stream.CanWrite) throw new InvalidOperationException("cannot write in the stream");

            #endregion

            byte[] buffer = new byte[count];
            int length;
            int range;

            stream.Seek(offset + count, SeekOrigin.Begin);

            while ((length = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                range = count + length;
                stream.Seek(-range, SeekOrigin.Current);
                stream.Write(buffer, 0, length);
                stream.Seek(count, SeekOrigin.Current);
            }

            stream.SetLength(stream.Length - count);
        }

        /// <summary>
        /// Inserts block of bytes in the file stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="offset">The file stream offset in which the bytes block will be inserted.</param>
        /// <param name="data">The byte block which will be inserted.</param>
        /// <param name="dataOffset">The zero-based byte offset in array at which to begin copying bytes to the file stream.</param>
        /// <param name="dataCount">The number of bytes from the array to be written to the file stream.</param>
        /// <exception cref="ArgumentNullException">Thrown when stream or data is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when offset or dataOffset is negative - or - offset is greater than the file stream length.</exception>
        /// <exception cref="ArgumentException">Thrown when data is an empty array - or - dataOffset and dataCount describe an invalid range in data array.</exception>
        /// <exception cref="InvalidOperationException">Thrown when cannot seek or write in the file stream.</exception>
        public static void InsertBlock(this FileStream stream, long offset, byte[] data, int dataOffset, int dataCount)
        {
            #region - Exceptions -

            if (stream == null) throw new ArgumentNullException("stream");
            if (data == null) throw new ArgumentNullException("data");

            if (offset < 0) throw new ArgumentOutOfRangeException("offset", "offset is a negative number");
            if (dataOffset < 0) throw new ArgumentOutOfRangeException("dataOffset", "dataOffset is a negative number");
            if (stream.Length < offset) throw new ArgumentOutOfRangeException("offset", "offset is greater than the file stream length");

            if (data.Length == 0) throw new ArgumentException("data is empty", "data");
            if (data.Length < (dataOffset + dataCount)) throw new ArgumentException("dataCount is greater than the data length", "dataCount");

            if (!stream.CanSeek) throw new InvalidOperationException("cannot seek in the stream");
            if (!stream.CanWrite) throw new InvalidOperationException("cannot write in the stream");

            #endregion

            stream.Seek(offset, SeekOrigin.Begin);

            if (stream.Position != stream.Length)
            {
                int readCount;
                int writeCount;
                byte[] readBuffer = new byte[dataCount];
                byte[] writeBuffer = new byte[dataCount];

                readCount = stream.Read(readBuffer, 0, dataCount);
                stream.Seek(-readCount, SeekOrigin.Current);
                stream.Write(data, dataOffset, dataCount);
                Buffer.BlockCopy(readBuffer, 0, writeBuffer, 0, readCount);
                writeCount = readCount;

                while ((readCount = stream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    stream.Seek(-readCount, SeekOrigin.Current);
                    stream.Write(writeBuffer, 0, writeCount);
                    Buffer.BlockCopy(readBuffer, 0, writeBuffer, 0, readCount);
                    writeCount = readCount;
                }

                stream.Write(writeBuffer, 0, writeCount);
            }
            else
            {
                stream.Write(data, dataOffset, dataCount);
            }
        }

        /// <summary>
        /// Inserts a block of bytes in the file stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="offset">The file stream offset in which the byte block will be inserted.</param>
        /// <param name="data">The byte block which will be inserted.</param>
        /// <exception cref="ArgumentNullException">Thrown when stream or data is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when offset or dataOffset is negative - or - offset is greater than the file stream length.</exception>
        /// <exception cref="ArgumentException">Thrown when data is an empty array - or - dataOffset and dataCount describe an invalid range in data array.</exception>
        /// <exception cref="InvalidOperationException">Thrown when cannot seek or write in the file stream.</exception>
        public static void InsertBlock(this FileStream stream, long offset, byte[] data)
        {
            InsertBlock(stream, offset, data, 0, data.Length);
        }

        /// <summary>
        /// Updates a block of bytes in the file stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="offset">The offset of update in file stream.</param>
        /// <param name="count">The count of bytes to be updated in the file stream.</param>
        /// <param name="data">The data array which will replace the old content.</param>
        /// <param name="dataOffset">The offset of byte data.</param>
        /// <param name="dataCount">The count of bytes from the data array.</param>
        /// <exception cref="ArgumentNullException">Thrown when stream or data is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when offset or dataOffset is negative - or - offset is greater than the file stream length.</exception>
        /// <exception cref="ArgumentException">Thrown when data is an empty array - or - dataOffset and dataCount describe an invalid range in data array.</exception>
        /// <exception cref="InvalidOperationException">Thrown when cannot seek or write in the file stream.</exception>
        public static void UpdateBlock(this FileStream stream, long offset, int count, byte[] data, int dataOffset, int dataCount)
        {
            #region - Exceptions -

            if (stream == null) throw new ArgumentNullException("stream");
            if (data == null) throw new ArgumentNullException("data");

            if (offset < 0) throw new ArgumentOutOfRangeException("offset", "offset is a negative number");
            if (dataOffset < 0) throw new ArgumentOutOfRangeException("dataOffset", "dataOffset is a negative number");
            if (stream.Length < offset) throw new ArgumentOutOfRangeException("offset", "offset is greater than the file stream length");

            if (data.Length == 0) throw new ArgumentException("data is empty", "data");
            if (data.Length < (dataOffset + dataCount)) throw new ArgumentException("dataCount is greater than the data length", "dataCount");

            if (!stream.CanSeek) throw new InvalidOperationException("cannot seek in the stream");
            if (!stream.CanWrite) throw new InvalidOperationException("cannot write in the stream");

            #endregion

            if (count != dataCount)
            { // new data size IS NOT equal to the old one
                int margin = Math.Abs(count - dataCount);

                stream.Seek(offset, SeekOrigin.Begin);

                if (dataCount < count)
                { // new data size in less than the old one
                    stream.Write(data, dataOffset, dataCount);
                    DeleteBlock(stream, stream.Position, margin);
                }
                else
                { // new data size in greater than the old one
                    byte[] buffer = new byte[margin];
                    int len;

                    stream.Seek(count, SeekOrigin.Current);
                    len = stream.Read(buffer, 0, margin);
                    stream.Seek(-(dataCount - (margin - len)), SeekOrigin.Current);
                    stream.Write(data, dataOffset, dataCount);

                    InsertBlock(stream, stream.Position, buffer, 0, len);
                }
            }
            else
            { // new data size IS equal to the old one
                stream.Seek(offset, SeekOrigin.Begin);
                stream.Write(data, dataOffset, dataCount);
            }
        }

        /// <summary>
        /// Updates a block of bytes in the file stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="offset">The offset of update in file stream.</param>
        /// <param name="count">The count of bytes to be updated in the file stream.</param>
        /// <param name="data">The data array which will replace the old content.</param>
        /// <exception cref="ArgumentNullException">Thrown when stream or data is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when offset or dataOffset is negative - or - offset is greater than the file stream length.</exception>
        /// <exception cref="ArgumentException">Thrown when data is an empty array - or - dataOffset and dataCount describe an invalid range in data array.</exception>
        /// <exception cref="InvalidOperationException">Thrown when cannot seek or write in the file stream.</exception>
        public static void UpdateBlock(this FileStream stream, long offset, int count, byte[] data)
        {
            UpdateBlock(stream, offset, count, data, 0, data.Length);
        }
    }
}
