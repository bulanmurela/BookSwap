using System.Windows;
using BookSwapApp.Services;
using BookSwapApp.Views;

namespace BookSwapApp
{
    public partial class MainWindow : Window
    {
        private NavigationService _navigationService;

        public MainWindow()
        {
            InitializeComponent();
            _navigationService = new NavigationService(MainFrame);
            _navigationService.NavigateTo(typeof(HomePage), _navigationService);
        }
    }
}
