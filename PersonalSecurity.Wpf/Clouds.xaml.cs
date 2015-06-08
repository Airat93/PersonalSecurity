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
    /// <summary>
    /// Interaction logic for Clouds.xaml
    /// </summary>
    public partial class Clouds : Window
    {
        public Clouds()
        {
            InitializeComponent();
        }

        private void Yandex_Click(object sender, RoutedEventArgs e)
        {
            var yandex = new Yandex();
            yandex.ShowDialog();

            Close();
        }
    }
}
