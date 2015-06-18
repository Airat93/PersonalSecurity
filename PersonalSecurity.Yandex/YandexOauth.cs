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
                return "https://oauth.yandex.ru/authorize?response_type=token&client_id=1bce85c938504b8abab435f7c7bfde86";
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
