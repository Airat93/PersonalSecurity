namespace PersonalSecurity.Yandex
{
    using System;
    using System.IO;
    using PersonalSecurity.DataAccess.Domain;
    using FileInfo = PersonalSecurity.DataAccess.Domain.FileInfo;

    public interface ICloudApi
    {
        void UploadFile(FileInfo fileInfo, Stream stream, EventHandler<CloudEventArgs> completeCallback);

        Stream DownloadFile(string folder, string fileName);

        CloudType Cloud { get; }
    }
}
