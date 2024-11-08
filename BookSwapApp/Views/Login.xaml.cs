using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;
using BookSwapApp.Models;
using BookSwapApp.Repositories;
using System.Security.Policy;

namespace BookSwapApp.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly UserRepository _userRepository;
        private NavigationService _navigationService;

        public Login()
        {
            InitializeComponent();
            _userRepository = new UserRepository();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Panggil metode Login di UserRepository
            var user = _userRepository.Login(username, password);

            if (user != null)
            {
                // Tampilkan message box sukses
                MessageBox.Show("Login berhasil!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                // Arahkan ke halaman HomePage setelah login berhasil
                _navigationService.NavigateTo(typeof(HomePage));
            }
            else
            {
                MessageBox.Show("Login gagal. Username atau password salah.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoToSignupPage(object sender, ContextMenuEventArgs e)
        {
            _navigationService.NavigateTo(typeof(Signup));
        }

        private void GoBack(object sender, EventArgs e)
        {
            _navigationService.GoBack();
        }

        private void IconGoBack(object sender, RoutedEventArgs e)
        {
            _navigationService.GoBack();
        }
    }
}
