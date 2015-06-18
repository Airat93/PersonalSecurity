namespace PersonalSecurity.Crypto
{
    using System;
    using System.Security;
    using System.Text;

    internal class Password : IDisposable
    {
        private readonly StringBuilder _strPass;

        public Password(SecureString password)
        {
            _strPass = new StringBuilder(password.ConvertToString());
        }

        public StringBuilder Value
        {
            get
            {
                return _strPass;
            }            
        }

        public void Dispose()
        {
            // очищаем StringBuilder от пароля
            _strPass.Clear();
        }
    }
}
