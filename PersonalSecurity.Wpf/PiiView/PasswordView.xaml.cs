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

namespace PersonalSecurity.Wpf.PiiView
{
    using System.Collections.ObjectModel;
    using PersonalSecurity.PII.PiiTemplates;

    /// <summary>
    /// Interaction logic for PasswordView.xaml
    /// </summary>
    public partial class PasswordView : Window
    {
        public PasswordView(PasswordTemplate passwords)
        {
            InitializeComponent();

            DataGrid.ItemsSource = passwords.Credentials;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
