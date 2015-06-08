namespace PersonalSecurity.Crypto
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class FileCipher
    {
        private static readonly byte[] Salt = Encoding.ASCII.GetBytes("fewkfejfkwe435fdfk");

        private const int Iterations = 1024;

        public static void EncryptFile(string sourceFilename, string destinationFilename, string password)
        {
            var aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            var key = new Rfc2898DeriveBytes(password, Salt, Iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            var transform = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var destination = new FileStream(destinationFilename, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (var cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write))
                {
                    using (var source = new FileStream(sourceFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        source.CopyTo(cryptoStream);
                    }
                }
            }
        }

        public static void DecryptFile(string sourceFilename, string destinationFilename, string password)
        {
            var aes = new AesManaged();
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            // NB: Rfc2898DeriveBytes initialization and subsequent calls to   GetBytes   must be eactly the same, including order, on both the encryption and decryption sides.
            var key = new Rfc2898DeriveBytes(password, Salt, Iterations);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            var transform = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var destination = new FileStream(destinationFilename, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (var cryptoStream = new CryptoStream(destination, transform, CryptoStreamMode.Write))
                {
                    try
                    {
                        using (var source = new FileStream(sourceFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            source.CopyTo(cryptoStream);
                        }
                    }
                    catch (CryptographicException exception)
                    {
                        if (exception.Message == "Padding is invalid and cannot be removed.")
                            throw new ApplicationException("Universal Microsoft Cryptographic Exception (Not to be believed!)", exception);
                        throw;
                    }
                }
            }
        }
    }
}
