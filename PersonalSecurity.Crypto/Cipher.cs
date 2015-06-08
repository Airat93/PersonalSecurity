namespace PersonalSecurity.Crypto
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class Cipher
    {
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] InitVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

        private static readonly byte[] Salt = Encoding.ASCII.GetBytes("fewkfejfkwe435fdfk");

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int Keysize = 256;

        public static Stream EncryptText(string plainText, string passPhrase)
        {
            var bytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, Salt))
            {
                byte[] keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, InitVectorBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(bytes, 0, bytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var text = Convert.ToBase64String(memoryStream.ToArray());
                                return text.ToStream();
                            }
                        }
                    }
                }
            }
        }

        public static string DecryptText(string cipherText, string passPhrase)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, Salt))
            {
                byte[] keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, InitVectorBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (
                                var cryptoStream = new CryptoStream(
                                    memoryStream,
                                    decryptor,
                                    CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}