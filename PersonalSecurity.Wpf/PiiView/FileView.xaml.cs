using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PersonalSecurity.Wpf.PiiView
{
    using System.Diagnostics;
    using System.IO;
    using PersonalSecurity.Crypto;
    using PersonalSecurity.DataAccess.Domain;
    using PersonalSecurity.Files;
    using PersonalSecurity.PII.Deserializing;
    using PersonalSecurity.Yandex;
    using FileInfo = PersonalSecurity.DataAccess.Domain.FileInfo;

    /// <summary>
    /// Interaction logic for FileView.xaml
    /// </summary>
    public partial class FileView : Window
    {
        private readonly ICloudApi _cloudApi;
        private readonly IFileManager _fileManager;

        public FileView(FileInfo[] files, ICloudApi cloudApi, IFileManager fileManager)
        {
            _cloudApi = cloudApi;
            _fileManager = fileManager;
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
                FileCipher.DecryptFile(path, decryptedPath, "password");

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
