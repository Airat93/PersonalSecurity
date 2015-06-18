namespace PersonalSecurity.Crypto
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    internal static class SecureStringExstension
    {
        public static string ConvertToString(this SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                // копируем SecureString в неуправляемую память и получаем указатель на неё
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);

                // конвертация в строку
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                // указываем нулевой указатель
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
