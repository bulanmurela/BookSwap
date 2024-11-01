using System.Windows;
using BookSwapApp.Helpers;
using BookSwapApp.Services;
using BookSwapApp.Views;
using BookSwapApp.Helpers;

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

            var dbHelper = new DatabaseHelpers();
            bool isConnected = dbHelper.TestConnection();
            if (isConnected)
            {
                MessageBox.Show("Database terkoneksi!", "Sukses");
            }
            else
            {
                MessageBox.Show("Gagal terhubung ke database.", "Kesalahan");
            }
        }
    }
}
