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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonalSecurity.Wpf
{
    using Microsoft.Practices.Unity;
    using PersonalSecurity.DataAccess;
    using PersonalSecurity.DataAccess.Domain;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICloudRepository _cloudRepository;

        public MainWindow()
        {
            _cloudRepository = App.Container.Resolve<ICloudRepository>();

            InitializeComponent();

            this.Activated += OnActivated;
        }

        private void OnActivated(object sender, EventArgs eventArgs)
        {
            var clouds = _cloudRepository.GetAll();

            if (clouds.All(x => x.CloudType != CloudType.Yandex))
            {
                Yandex.IsEnabled = false;
            }

            if (clouds.All(x => x.CloudType != CloudType.Google))
            {
                Google.IsEnabled = false;
            }
        }

        private void CloudServices_Click(object sender, RoutedEventArgs e)
        {
            var cloud = new Clouds();
            Close();
            cloud.ShowDialog();
        }

        private void Yandex_Click(object sender, RoutedEventArgs e)
        {
            var personalInfo = new PersonalInfo(CloudType.Yandex);

            Close();
            personalInfo.Show();
        }
    }
}
