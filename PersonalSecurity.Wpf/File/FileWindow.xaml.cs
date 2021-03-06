﻿namespace PersonalSecurity.Wpf.File
{
    using System.IO;
    using System.Security;
    using System.Windows;
    using Microsoft.Win32;
    using PersonalSecurity.Crypto;
    using PersonalSecurity.Files;
    using PersonalSecurity.Yandex;

    /// <summary>
    /// Interaction logic for FileWindow.xaml
    /// </summary>
    public partial class FileWindow : Window
    {
        private readonly IFileManager _fileManager;
        private readonly ICloudApi _cloudApi;

        private readonly SecureString _password;

        public FileWindow(ICloudApi cloudApi, IFileManager fileManager, SecureString password)
        {
            _cloudApi = cloudApi;
            _fileManager = fileManager;
            _password = password;

            InitializeComponent();
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                // создание в локальной базе информации о загружаемом файле
                var file = _fileManager.CreateFileInfo(
                    DataAccess.Domain.FileType.File,
                    _cloudApi.Cloud,
                    Path.GetFileName(openDialog.FileName),
                    FileTypeText.Text);

                // локальный путь для хранения зашифрованного файла
                var encryptedFilePath = _fileManager.GetEncryptedFilePath(file.Name);

                // шифрование файла
                FileCipher.EncryptFile(openDialog.FileName, encryptedFilePath, _password);

                // загрузка зашифрованного файла на облако
                _cloudApi.UploadFile(file, new FileStream(encryptedFilePath, FileMode.Open),
                    OnUploadComplete);
            }
        }

        private void OnUploadComplete(object sender, CloudEventArgs e)
        {
            if (e.ErrorMessage == null)
            {
                _fileManager.SaveFile(e.FileInfo);
                MessageBox.Show("Данные успешно сохранены");
            }
            else
            {
                MessageBox.Show(e.ErrorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
