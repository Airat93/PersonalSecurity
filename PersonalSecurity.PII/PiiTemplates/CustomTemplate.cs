namespace PersonalSecurity.PII.PiiTemplates
{
    using System.Collections.Generic;

    public class CustomTemplate : PiiTemplate
    {
        public IList<CustomItem> Items { get; set; } 
    }

    public class CustomItem
    {
        public string GroupName { get; set; }
        public Dictionary<string, string> Fields { get; set; } 
    }
}
