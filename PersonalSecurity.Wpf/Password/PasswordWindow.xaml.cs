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
using System.Windows.Shapes;

namespace PersonalSecurity.Wpf.Password
{
    using System.IO;
    using System.Security;
    using PersonalSecurity.Crypto;
    using PersonalSecurity.DataAccess.Domain;
    using PersonalSecurity.Files;
    using PersonalSecurity.PII.Builder;
    using PersonalSecurity.PII.PiiTemplates;
    using PersonalSecurity.Yandex;

    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        private readonly IPiiBuilder _piiBuilder;
        private readonly IFileManager _fileManager;
        private readonly ICloudApi _cloudApi;

        private readonly PasswordTemplate _passwords = new PasswordTemplate();

        private readonly SecureString _password;

        public PasswordWindow(IPiiBuilder piiBuilder, IFileManager fileManager, ICloudApi cloudApi, SecureString password)
        {
            _piiBuilder = piiBuilder;
            _fileManager = fileManager;
            _cloudApi = cloudApi;
            _password = password;
            InitializeComponent();
        }

        private void SavePassword_Click(object sender, RoutedEventArgs e)
        {
            var piiSer = _piiBuilder.BuildPii(_passwords);

            var stream = Cipher.EncryptText(piiSer, _password);

            var file = _fileManager.CreateFileInfo(FileType.Passwords, _cloudApi.Cloud);

            _cloudApi.UploadFile(file, stream, OnUploadCompleted);
        }

        private void OnUploadCompleted(object sender, CloudEventArgs e)
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

        private void AddAnother_Click(object sender, RoutedEventArgs e)
        {
            _passwords.Credentials.Add(new Credentials
            {
                UserName = LoginText.Text,
                Password = PasswordBox1.Password,
                Service = ServiceNameText.Text
            });

            LoginText.Clear();
            PasswordBox1.Clear();
            ServiceNameText.Clear();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
