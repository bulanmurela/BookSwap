using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Services;

namespace BookSwapApp.Views
{
    public partial class HomePage : Page
    {
        private NavigationService _navigationService;

        public HomePage(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(SwapReq));
        }
    }
}
