namespace PersonalSecurity.Crypto
{
    using System.Security.Cryptography;
    using System.Text;

    public static class Hash
    {
        public static string Sha256(string value)
        {
            var sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
