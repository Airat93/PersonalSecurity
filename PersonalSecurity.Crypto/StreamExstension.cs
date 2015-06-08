namespace PersonalSecurity.Crypto
{
    using System.IO;

    public static class StreamExstension
    {
        public static Stream ToStream(this string str)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);

            streamWriter.Write(str);
            streamWriter.Flush();
            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
