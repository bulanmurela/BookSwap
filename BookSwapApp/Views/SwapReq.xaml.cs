using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;

namespace BookSwapApp.Views
{
    public partial class SwapReq : Page
    {
        private NavigationService _navigationService;

        public SwapReq()
        {
            InitializeComponent();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
        }

        private void btnReqSwap_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(StatusRequest));
        }
    }
}
