namespace PersonalSecurity.PII.PiiTemplates
{
    using System.Collections.Generic;

    public class PasswordTemplate : PiiTemplate
    {
        public PasswordTemplate()
        {
            Credentials = new List<Credentials>();
        }

        public IList<Credentials> Credentials { get; set; }
    }

    public class Credentials
    {
        public string Service { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
