namespace PersonalSecurity.PII.PiiTemplates
{
    using System;

    public class JobTemplate : PiiTemplate
    {
        public string Post { get; set; }
        public Company Company { get; set; }
    }

    public class Company
    {
        public string Name { get; set; }
        public DateTime DateOfFoundation { get; set; }
    }
}
