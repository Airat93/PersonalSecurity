namespace PersonalSecurity.Crypto
{
    using System;
    using System.Security;
    using System.Text;

    internal class Password : IDisposable
    {
        private readonly StringBuilder _password;

        public Password(SecureString password)
        {
            _password = new StringBuilder(password.ConvertToString());
        }

        public StringBuilder Value
        {
            get
            {
                return _password;
            }            
        }

        public void Dispose()
        {
            _password.Clear();
        }
    }
}
