namespace PersonalSecurity.Files.Utils
{
    using System;
    using System.Text;

    public static class RandomString
    {
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        public static string Get()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < Random.Next(5, 20); i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
