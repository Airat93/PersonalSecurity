namespace PersonalSecurity.Yandex
{
    using System;
    using PersonalSecurity.DataAccess.Domain;

    public class CloudEventArgs : EventArgs
    {
        public FileInfo FileInfo { get; set; }

        public string ErrorMessage { get; set; }
    }
}
