using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace System
{
    [DebuggerStepThrough]
    internal static class ByteExtensions
    {
        /// <summary>
        /// Decrypts the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="password">The password.</param>
        /// <param name="paddingMode">The padding mode.</param>
        /// <returns></returns>
        public static byte[] DecryptData(this byte[] data, String password, PaddingMode paddingMode)
        {

            if (data == null || data.Length == 0)
                throw new ArgumentNullException("data");
            if (password == null)
                throw new ArgumentNullException("password");
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, Encoding.UTF8.GetBytes("Salt"));
            RijndaelManaged rm = new RijndaelManaged();
            rm.Padding = paddingMode;
            ICryptoTransform decryptor = rm.CreateDecryptor(pdb.GetBytes(16), pdb.GetBytes(16));
            using (MemoryStream msDecrypt = new MemoryStream(data))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            {
                // Decrypted bytes will always be less then encrypted bytes, so len of encrypted data will be big enouph for buffer.
                Byte[] fromEncrypt = new Byte[data.Length];                // Read as many bytes as possible.
                int read = csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                if (read < fromEncrypt.Length)
                {
                    // Return a Byte array of proper size.
                    Byte[] clearBytes = new Byte[read];
                    Buffer.BlockCopy(fromEncrypt, 0, clearBytes, 0, read);
                    return clearBytes;
                }
                return fromEncrypt;
            }
        }

        /// <summary>
        /// Encrypts the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="password">The password.</param>
        /// <param name="paddingMode">The padding mode.</param>
        /// <returns></returns>
        public static byte[] EncryptData(this byte[] data, string password, PaddingMode paddingMode)
        {

            if (data == null || data.Length == 0)
                throw new ArgumentNullException("data");
            if (password == null)
                throw new ArgumentNullException("password");
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, Encoding.UTF8.GetBytes("Salt"));
            RijndaelManaged rm = new RijndaelManaged();
            rm.Padding = paddingMode;
            ICryptoTransform encryptor = rm.CreateEncryptor(pdb.GetBytes(16), pdb.GetBytes(16));
            using (MemoryStream msEncrypt = new MemoryStream())
            using (CryptoStream encStream = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                encStream.Write(data, 0, data.Length);
                encStream.FlushFinalBlock();
                return msEncrypt.ToArray();
            }
        }
    }
}
