using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    [DebuggerStepThrough]
    internal static class StringEncryptionExtensions
    {
        /// <summary>
        /// Decrypt the data String to the original String.  The data must be the base64 String
        /// returned from the EncryptData method.
        /// </summary>
        /// <param name="data">Encrypted data generated from EncryptData method.</param>
        /// <param name="password">Password used to decrypt the String.</param>
        /// <returns>Decrypted String.</returns>
        public static string DecryptData(this string data, string password)
        {
            #region - Exceptions -

            if (data == null) throw new ArgumentNullException("data");
            if (password == null) throw new ArgumentNullException("password");

            #endregion

            byte[] encBytes = Convert.FromBase64String(data);
            byte[] decBytes = encBytes.DecryptData(password, PaddingMode.ISO10126);
            return Encoding.UTF8.GetString(decBytes);
        }

        /// <summary>
        /// Use AES to encrypt data String. The output String is the encrypted bytes as a base64 String.
        /// The same password must be used to decrypt the String.
        /// </summary>
        /// <param name="data">Clear String to encrypt.</param>
        /// <param name="password">Password used to encrypt the String.</param>
        /// <returns>Encrypted result as Base64 String.</returns>
        public static string EncryptData(this string data, string password)
        {
            #region - Exceptions -

            if (data == null) throw new ArgumentNullException("data");
            if (password == null) throw new ArgumentNullException("password");

            #endregion

            byte[] encBytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(encBytes.EncryptData(password, PaddingMode.ISO10126));
        }
    }
}
