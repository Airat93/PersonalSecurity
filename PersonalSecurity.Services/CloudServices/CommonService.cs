namespace PersonalSecurity.Services.CloudServices
{
    using PersonalSecurity.DataAccess;
    using PersonalSecurity.DataAccess.Domain;
    using PersonalSecurity.Files;
    using PersonalSecurity.Yandex;

    public abstract class CommonService
    {
        private readonly YandexDisk _yandex;
        private readonly string _commonPath;

        protected CommonService(ICloudRepository cloudRepository, YandexDisk yandex, string commonPath)
        {
            _yandex = yandex;
            _commonPath = commonPath;
            _yandex = new YandexDisk(cloudRepository.GetByType(CloudType.Yandex).AccessToken);
        }

        protected YandexDisk Yandex
        {
            get
            {
                return _yandex;
            }
        }

        protected string CommonPath
        {
            get { return _commonPath; }
        }

        protected abstract FileManager FileManager { get; }

        public abstract void UploadProcess();
    }
}
