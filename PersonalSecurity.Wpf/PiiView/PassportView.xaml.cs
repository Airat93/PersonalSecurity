namespace PersonalSecurity.Wpf.PiiView
{
    using System.Windows;
    using PersonalSecurity.PII.PiiTemplates;

    /// <summary>
    /// Interaction logic for PassportView.xaml
    /// </summary>
    public partial class PassportView : Window
    {
        public PassportView(PassportTemplate passportTemplate)
        {
            InitializeComponent();

            SeriesText.Content = passportTemplate.Series;
            NumberText.Content = passportTemplate.Number;
            IssuedText.Content = passportTemplate.Issued;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
