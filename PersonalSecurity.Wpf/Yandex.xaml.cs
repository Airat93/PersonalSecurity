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
    using System.Windows.Navigation;
    using PersonalSecurity.DataAccess;
    using PersonalSecurity.DataAccess.Domain;
    using PersonalSecurity.Yandex;

    /// <summary>
    /// Interaction logic for YandexOauth.xaml
    /// </summary>
    public partial class Yandex : Window
    {
        private readonly YandexOauth _yandexOauth = new YandexOauth();

        private readonly CloudRepository _cloudRepository =
            new CloudRepository("Data Source=.; Integrated Security=True; Database=PersonalSecurityLocal;");

        public Yandex()
        {
            InitializeComponent();

            // переходим на страницу регистрации
            Browser.Navigate(_yandexOauth.AutUrl);

            // подписка на событие, происходящее при навигации по некоторому URL
            Browser.Navigating += BrowserOnNavigating;
        }

        private void BrowserOnNavigating(object sender, NavigatingCancelEventArgs navigatingCancelEventArgs)
        {
            // проверяем, содержит ли пришедший URL необходимый CallbackURL
            if (navigatingCancelEventArgs.Uri.AbsoluteUri.Contains(_yandexOauth.ReturnUrl))
            {

                // парсим access token
                var accessToken = _yandexOauth.ParseAccessToken(navigatingCancelEventArgs.Uri);

                // сохраняем его в базу
                _cloudRepository.Save(new Cloud { AccessToken = accessToken, CloudType = CloudType.Yandex });

                // процедура показа главного окна
                ShowMain();
            }
        }

        private void ShowMain()
        {
            Close();
            var main = new MainWindow { ShowActivated = true };
            main.Show();
        }
    }
}
