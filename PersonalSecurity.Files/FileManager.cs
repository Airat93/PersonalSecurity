namespace PersonalSecurity.Files
{
    using System.IO;
    using PersonalSecurity.Crypto;
    using PersonalSecurity.DataAccess;
    using PersonalSecurity.DataAccess.Domain;
    using PersonalSecurity.Files.Utils;
    using FileInfo = PersonalSecurity.DataAccess.Domain.FileInfo;

    public class FileManager : IFileManager
    {
        private readonly IFileInfoRepository _fileInfoRepository;
        private readonly string _encryptedPath;
        private readonly string _localPath;

        public FileManager(IFileInfoRepository fileInfoRepository, string encryptedPath, string localPath)
        {
            CreateDir(encryptedPath);
            CreateDir(localPath);

            _fileInfoRepository = fileInfoRepository;
            _encryptedPath = encryptedPath;
            _localPath = localPath;
        }

        private void CreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public FileInfo CreateFileInfo(
            string fileType,
            CloudType cloudType,
            string fileName = null,
            string fileForm = null)
        {
            var fileInfo = new FileInfo
            {
                Name = fileName ?? GetUniquePath(),
                FileType = fileType,
                CloudType = cloudType,
                FileForm = fileForm
            };

            return fileInfo;
        }

        public void SaveFile(FileInfo fileInfo)
        {
            if (fileInfo.FileType == FileType.File)
            {
                var file = _fileInfoRepository.GetByName(fileInfo.Name);
                if (file == null)
                {
                    _fileInfoRepository.Save(fileInfo);
                }
            }
            else
            {
                while (_fileInfoRepository.GetByName(fileInfo.Name) != null)
                {
                    fileInfo.Name = GetUniquePath();
                }

                _fileInfoRepository.Save(fileInfo);
            }
        }

        public string GetEncryptedFilePath(string fileName)
        {
            return GetUniquePath(_encryptedPath, fileName);
        }

        public string GetLocalFilePath(string fileName)
        {
            return GetUniquePath(_localPath, fileName);
        }

        public string LocalDir
        {
            get
            {
                return _localPath;
            }
        }

        public FileInfo[] GetByFileType(string fileType)
        {
            return _fileInfoRepository.GetByType(fileType);
        }

        private string GetUniquePath(string dir, string fileName)
        {
            var result = fileName;
            var filePath = Path.Combine(dir, result);
            var uniqueCounter = 1;

            while (File.Exists(filePath))
            {
                result = string.Format(
                    "{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(fileName),
                    uniqueCounter,
                    Path.GetExtension(fileName));

                filePath = Path.Combine(dir, result);
                uniqueCounter++;
            }

            return filePath;
        }

        private string GetUniquePath()
        {
            return Hash.Sha256(RandomString.Get());
        }
    }
}
