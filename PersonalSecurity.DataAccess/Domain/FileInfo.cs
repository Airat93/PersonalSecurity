namespace PersonalSecurity.DataAccess.Domain
{
    using System;

    public class FileInfo : DbEntity
    {
        public string Name { get; set; }

        public CloudType CloudType { get; set; }

        public string FileType { get; set; }

        public string FileForm { get; set; }

        public string CloudTypeCode
        {
            get
            {
                return CloudType.ToString();
            }
            set
            {
                CloudType = (CloudType)Enum.Parse(typeof(CloudType), value);
            }
        }
    }

    public static class FileType
    {
        public static readonly string Personal = "Personal";
        public static readonly string Passwords = "Passwords";
        public static readonly string Passport = "Passport";
        public static readonly string File = "File";
    }

}
