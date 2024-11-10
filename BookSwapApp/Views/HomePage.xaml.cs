using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Models;
using BookSwapApp.Services;

namespace BookSwapApp.Views
{
    public partial class HomePage : Page
    {
        private NavigationService _navigationService;
        private readonly User currentUser;
        public HomePage() : this(new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame))
        {
        }

        public HomePage(User user, NavigationService navigationService) : this(navigationService)
        {
            currentUser = user;
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

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(SwapReq));
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            var currentUser = app.CurrentUser;

            if (currentUser == null)
            {
                MessageBox.Show("User information is missing. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _navigationService.NavigateTo(typeof(Login)); // Redirect to login if currentUser is null
                return;
            }

            // Navigate to UploadBook with currentUser
            _navigationService.NavigateTo(typeof(UploadBook), currentUser, _navigationService);
        }
    }
}