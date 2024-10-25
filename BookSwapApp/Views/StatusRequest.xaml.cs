using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;

namespace BookSwapApp.Views
{
    public partial class StatusRequest : Page
    {
        private NavigationService _navigationService;

        public StatusRequest()
        {
            InitializeComponent();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
        }

        private void GoToHome(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(HomePage));
        }

        private void GoBack(object sender, ContextMenuEventArgs e)
        {
            _navigationService.GoBack();
        }

        private void IconGoBack(object sender, ContextMenuEventArgs e)
        {
            _navigationService.GoBack();
        }

        private void GoToProfile(object sender, ContextMenuEventArgs e)
        {
            _navigationService.NavigateTo(typeof(Profil));
        }

        // Sama jika ada parameter:
        //public StatusRequest(string requestData)
        //{
        //    InitializeComponent();
        //    // Gunakan 'requestData' sesuai kebutuhan
        //}
    }
}
