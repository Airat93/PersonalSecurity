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

namespace PersonalSecurity.Wpf.Personal
{
    using System.Security;
    using PersonalSecurity.Crypto;
    using PersonalSecurity.DataAccess.Domain;
    using PersonalSecurity.Files;
    using PersonalSecurity.PII.Builder;
    using PersonalSecurity.PII.PiiTemplates;
    using PersonalSecurity.Yandex;

    /// <summary>
    /// Interaction logic for PersonalWindow.xaml
    /// </summary>
    public partial class PersonalWindow : Window
    {
        private readonly IPiiBuilder _piiBuilder;
        private readonly IFileManager _fileManager;
        private readonly ICloudApi _cloudApi;

        private readonly SecureString _password;

        public PersonalWindow(IPiiBuilder piiBuilder, IFileManager fileManager, ICloudApi cloudApi, SecureString password)
        {
            _piiBuilder = piiBuilder;
            _fileManager = fileManager;
            _cloudApi = cloudApi;
            _password = password;

            InitializeComponent();
        }

        private void SavePersonal_Click(object sender, RoutedEventArgs e)
        {
            var item = new PersonalTemplate
            {
                LastName = LastNameText.Text,
                FirstName = NameText.Text,
                DateOfBirth = DateOfBirthPicker.DisplayDate.Date
            };

            var str = _piiBuilder.BuildPii(item);

            var stream = Cipher.EncryptText(str, _password);

            var file = _fileManager.CreateFileInfo(FileType.Personal, _cloudApi.Cloud);

            _cloudApi.UploadFile(file, stream, OnUploadComplete);
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
