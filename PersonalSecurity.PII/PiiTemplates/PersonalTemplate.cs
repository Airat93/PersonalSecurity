namespace PersonalSecurity.PII.PiiTemplates
{
    using System;

    public class PersonalTemplate : PiiTemplate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
