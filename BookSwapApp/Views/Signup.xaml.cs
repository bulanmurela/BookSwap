using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;

namespace BookSwapApp.Views
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Page
    {
        private NavigationService _navigationService;

        public Signup()
        {
            InitializeComponent();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(Signup));
        }

        private void GoToLoginPage(object sender, ContextMenuEventArgs e)
        {
            _navigationService.NavigateTo(typeof(Login));
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            _navigationService.GoBack();
        }

        private void IconGoBack(object sender, RoutedEventArgs e)
        {
            _navigationService.GoBack();
        }
    }
}
