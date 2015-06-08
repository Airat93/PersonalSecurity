namespace PersonalSecurity.Yandex.Utils
{
    using System.Net;

    public static class HttpUtilities
    {
        public static HttpWebRequest CreateRequest(string url, string token)
        {
            var request = WebRequest.CreateHttp(url);
            request.Accept = "*/*";
            request.Headers["Depth"] = "1";
            request.Headers["Authorization"] = "OAuth " + token;
            request.Headers["X-Yandex-SDK-Version"] = "windows, 1.0";
            return request;
        }
    }
}
