using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Services;

namespace BookSwapApp.Views
{
    public partial class HomePage : Page
    {
        private NavigationService _navigationService;

        public HomePage() : this(new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame))
        {
            // Constructor tanpa parameter
        }

        public HomePage(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
        }

        private void GoToProfile(object sender, ContextMenuEventArgs e)
        {

            _navigationService.NavigateTo(typeof(Profil));
        }

        private void GoToLoginPage(object sender, ContextMenuEventArgs e)
        {
            _navigationService.NavigateTo(typeof(Login));
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(SwapReq));
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(UploadBook));
        }
    }
}
