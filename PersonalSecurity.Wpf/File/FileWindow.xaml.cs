﻿namespace PersonalSecurity.Wpf.File
{
    using System.IO;
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

        public FileWindow(ICloudApi cloudApi, IFileManager fileManager)
        {
            _cloudApi = cloudApi;
            _fileManager = fileManager;

            InitializeComponent();
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FileTypeText.Text))
            {
                MessageBox.Show("Введите тип файла");
                return;
            }

            var openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                var file = _fileManager.CreateFileInfo(
                    DataAccess.Domain.FileType.File,
                    _cloudApi.Cloud,
                    Path.GetFileName(openDialog.FileName),
                    FileTypeText.Text);

                var encryptedFilePath = _fileManager.GetEncryptedFilePath(file.Name);

                FileCipher.EncryptFile(openDialog.FileName, encryptedFilePath, "password");

                _cloudApi.UploadFile(file, new FileStream(encryptedFilePath, FileMode.Open), OnUploadComplete);
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