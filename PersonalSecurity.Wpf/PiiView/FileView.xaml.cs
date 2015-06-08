using System.Linq;
namespace PersonalSecurity.Wpf.PiiView
{
    using System.Diagnostics;
    using System.IO;
    using System.Security;
    using System.Windows;
    using PersonalSecurity.Crypto;
    using PersonalSecurity.DataAccess.Domain;
    using PersonalSecurity.Files;
    using PersonalSecurity.Yandex;
    using FileInfo = PersonalSecurity.DataAccess.Domain.FileInfo;

    /// <summary>
    /// Interaction logic for FileView.xaml
    /// </summary>
    public partial class FileView : Window
    {
        private readonly ICloudApi _cloudApi;
        private readonly IFileManager _fileManager;

        private readonly SecureString _password;

        public FileView(FileInfo[] files, ICloudApi cloudApi, IFileManager fileManager, SecureString password)
        {
            _cloudApi = cloudApi;
            _fileManager = fileManager;
            _password = password;
            InitializeComponent();

            GridItems.ItemsSource = files.Select(x => new FileItem { Name = x.Name, Type = x.FileForm });
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var item = GridItems.SelectedItem as FileItem;
            if (item != null)
            {
                var path = _fileManager.GetEncryptedFilePath(item.Name);
                using (var stream = _cloudApi.DownloadFile(FileType.File, item.Name))
                {
                    using (var file = File.Create(path))
                    {
                        stream.CopyTo(file);
                    }
                }

                var decryptedPath = _fileManager.GetLocalFilePath(item.Name);
                FileCipher.DecryptFile(path, decryptedPath, _password);

                LaunchDirectory(_fileManager.LocalDir);
                LaunchFile(decryptedPath);
            }
        }

        private void LaunchDirectory(string path)
        {
            var process = new Process { StartInfo = { FileName = path } };
            process.Start();
        }

        private void LaunchFile(string path)
        {
            var process = new Process { StartInfo = { FileName = path } };
            process.Start();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class FileItem
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
