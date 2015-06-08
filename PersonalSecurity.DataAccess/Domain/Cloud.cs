namespace PersonalSecurity.DataAccess.Domain
{
    public class Cloud : DbEntity
    {
        public CloudType CloudType { get; set; }
        public string AccessToken { get; set; }
    }

    public enum CloudType
    {
        Yandex,
        Google
    }
}
