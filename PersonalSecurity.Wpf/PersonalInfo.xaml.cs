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

namespace PersonalSecurity.Wpf
{
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Security;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Win32;
    using PersonalSecurity.Crypto;
    using PersonalSecurity.DataAccess;
    using PersonalSecurity.DataAccess.Domain;
    using PersonalSecurity.Files;
    using PersonalSecurity.PII;
    using PersonalSecurity.PII.Builder;
    using PersonalSecurity.PII.Deserializing;
    using PersonalSecurity.PII.PiiTemplates;
    using PersonalSecurity.Wpf.Annotations;
    using PersonalSecurity.Wpf.File;
    using PersonalSecurity.Wpf.Passport;
    using PersonalSecurity.Wpf.Password;
    using PersonalSecurity.Wpf.Personal;
    using PersonalSecurity.Wpf.PiiView;
    using PersonalSecurity.Yandex;

    /// <summary>
    /// Interaction logic for PersonalInfo.xaml
    /// </summary>
    public partial class PersonalInfo : Window
    {
        private readonly IFileManager _fileManager;
        private readonly IPiiDeserializer _piiDeserializer;
        private readonly IPiiBuilder _piiBuilder;
        private readonly ICloudApi _cloudApi;

        private SecureString _password = new SecureString();

        public PersonalInfo(CloudType currentCloud)
        {
            InitializeComponent();

            _fileManager = App.Container.Resolve<IFileManager>();

            var accessToken = App.Container.Resolve<ICloudRepository>().GetByType(currentCloud).AccessToken;
            _cloudApi = CloudApiFactory.CreateCloudApi(currentCloud, accessToken);

            _piiDeserializer = App.Container.Resolve<IPiiDeserializer>();
            _piiBuilder = App.Container.Resolve<IPiiBuilder>();

            CloudName.Content = currentCloud;

            MainGrid.Visibility = Visibility.Hidden;
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow { ShowActivated = true };
            main.Show();
            Close();
        }

        private void LoadPersonal_Click(object sender, RoutedEventArgs e)
        {
            var file = _fileManager.GetByFileType(FileType.Personal).LastOrDefault();
            if (file != null)
            {
                using (var stream = new StreamReader(_cloudApi.DownloadFile(file.FileType, file.Name)))
                {
                    var text = Cipher.DecryptText(stream.ReadToEnd(), _password);

                    var result = _piiDeserializer.Deserialize<PersonalTemplate>(text);
                    var personalView = new PersonalView(result);
                    personalView.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("У вас нет таких данных");
            }
        }

        private void LoadPassword_Click(object sender, RoutedEventArgs e)
        {
            var files = _fileManager.GetByFileType(FileType.Passwords);
            var result = new PasswordTemplate();
            foreach (var file in files)
            {
                using (var stream = new StreamReader(_cloudApi.DownloadFile(file.FileType, file.Name)))
                {
                    var text = Cipher.DecryptText(stream.ReadToEnd(), _password);

                    var passwordTemplate = _piiDeserializer.Deserialize<PasswordTemplate>(text);
                    foreach (var credential in passwordTemplate.Credentials)
                    {
                        result.Credentials.Add(credential);
                    }
                }
            }

            if (files.Length == 0)
            {
                MessageBox.Show("У вас нет таких данных");
            }
            else
            {
                var personalView = new PasswordView(result);
                personalView.ShowDialog();
            }
        }

        private void LoadPassport_Click(object sender, RoutedEventArgs e)
        {
            var file = _fileManager.GetByFileType(FileType.Passport).LastOrDefault();
            if (file != null)
            {
                using (var stream = new StreamReader(_cloudApi.DownloadFile(file.FileType, file.Name)))
                {
                    var text = Cipher.DecryptText(stream.ReadToEnd(), _password);

                    var result = _piiDeserializer.Deserialize<PassportTemplate>(text);

                    var passportView = new PassportView(result);

                    passportView.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("У вас нет таких данных");
            }
        }

        private void DownloadFiles_Click(object sender, RoutedEventArgs e)
        {
            var files = _fileManager.GetByFileType(FileType.File);
            var fileView = new FileView(files, _cloudApi, _fileManager, _password);
            fileView.ShowDialog();
        }

        private void Password_Click(object sender, RoutedEventArgs e)
        {
            var passwordWindow = new PasswordWindow(_piiBuilder, _fileManager, _cloudApi, _password);
            passwordWindow.Show();
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            var fileWindow = new FileWindow(_cloudApi, _fileManager, _password);
            fileWindow.Show();
        }

        private void Passport_Click(object sender, RoutedEventArgs e)
        {
            var passportWindow = new PassportWindow(_piiBuilder, _fileManager, _cloudApi, _password);
            passportWindow.Show();
        }

        private void Personal_Click(object sender, RoutedEventArgs e)
        {
            var personalWindow = new PersonalWindow(_piiBuilder, _fileManager, _cloudApi, _password);
            personalWindow.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PasswordButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordText.Password.ToCharArray().ForEach(x => _password.AppendChar(x));
            PasswordText.Clear();
            _password.MakeReadOnly();

            PasswordPanel.Visibility = Visibility.Hidden;
            MainGrid.Visibility = Visibility.Visible;
        }
    }
}
