namespace PersonalSecurity.Yandex
{
    using System;
    using System.Text.RegularExpressions;

    public class YandexOauth
    {
        public string AutUrl
        {
            get
            {
                return "https://oauth.yandex.ru/authorize?response_type=token&client_id=e3b0b1a2dc44447db5d010a1916a9d93";
            }
        }

        public string ReturnUrl
        {
            get
            {
                return "personalsecurity://token";
            }
        }

        public string ParseAccessToken(Uri uri)
        {
            return Regex.Match(uri.Fragment, "([a-zA-Z0-9]){32}").Value;
        }
    }
}
