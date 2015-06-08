using System.Windows;

namespace PersonalSecurity.Wpf.PiiView
{
    using PersonalSecurity.PII.PiiTemplates;

    /// <summary>
    /// Interaction logic for PersonalView.xaml
    /// </summary>
    public partial class PersonalView : Window
    {
        public PersonalView(PersonalTemplate personal)
        {
            InitializeComponent();

            Name.Content = personal.FirstName;
            LastName.Content = personal.LastName;
            DateOfBirth.DisplayDate = personal.DateOfBirth.Date;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
