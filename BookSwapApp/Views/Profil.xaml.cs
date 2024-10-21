using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;

namespace BookSwapApp.Views
{
    public partial class Profil : Page
    {
        private NavigationService _navigationService;

        public Profil()
        {
            InitializeComponent();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
        }

        // Handler untuk button HomePage
        private void btnHomePage_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(HomePage)); // Pastikan HomePage memiliki constructor tanpa parameter
        }
    }
}
