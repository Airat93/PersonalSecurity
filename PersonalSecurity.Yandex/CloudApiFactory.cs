namespace PersonalSecurity.Yandex
{
    using PersonalSecurity.DataAccess.Domain;

    public static class CloudApiFactory
    {
        public static ICloudApi CreateCloudApi(CloudType cloudType, string accessToken)
        {
            if (cloudType == CloudType.Yandex)
            {
                return new YandexDisk(accessToken);
            }

            return null;
        }
    }
}
