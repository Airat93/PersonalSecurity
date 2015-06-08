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

namespace PersonalSecurity.Wpf.Passport
{
    using PersonalSecurity.Crypto;
    using PersonalSecurity.DataAccess.Domain;
    using PersonalSecurity.Files;
    using PersonalSecurity.PII.Builder;
    using PersonalSecurity.PII.PiiTemplates;
    using PersonalSecurity.Yandex;

    /// <summary>
    /// Interaction logic for PassportWindow.xaml
    /// </summary>
    public partial class PassportWindow : Window
    {
        private readonly IPiiBuilder _piiBuilder;
        private readonly IFileManager _fileManager;
        private readonly ICloudApi _cloudApi;

        public PassportWindow(IPiiBuilder piiBuilder, IFileManager fileManager, ICloudApi cloudApi)
        {
            _piiBuilder = piiBuilder;
            _fileManager = fileManager;
            _cloudApi = cloudApi;
            InitializeComponent();

            PassportCopyLoad.Visibility = Visibility.Hidden;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var pii = new PassportTemplate
                          {
                              Series = SeriesText.Text,
                              Number = NumberText.Text,
                              Issued = IssuedText.Text
                          };

            var str = _piiBuilder.BuildPii(pii);
            var stream = Cipher.EncryptText(str, "password");
            var file = _fileManager.CreateFileInfo(FileType.Passport, _cloudApi.Cloud);
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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
