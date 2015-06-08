namespace PersonalSecurity.Files
{
    using PersonalSecurity.DataAccess.Domain;

    public interface IFileManager
    {
        FileInfo CreateFileInfo(string fileType, CloudType cloudType, string fileName = null, string fileForm = null);

        FileInfo[] GetByFileType(string fileType);

        void SaveFile(FileInfo fileInfo);

        string GetEncryptedFilePath(string fileName);

        string GetLocalFilePath(string fileName);

        string LocalDir { get; }
    }
}
