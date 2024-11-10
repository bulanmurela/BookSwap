using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;
using BookSwapApp.Repositories;
using BookSwapApp.Models;

namespace BookSwapApp.Views
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Page
    {
        private NavigationService _navigationService;
        private UserRepository _userRepository;

        public Signup() : this(new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame))
        {
            // Constructor tanpa parameter
        }

        public Signup(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _userRepository = new UserRepository();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            // Ambil input dari form
            string email = txtEmail.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Validasi sederhana untuk memastikan password sesuai
            if (password != confirmPassword)
            {
                MessageBox.Show("Password doesn't match. Please try again.");
                return;
            }

            // Buat objek User baru
            var user = new User(username, email, "", password); // Address kosong, bisa diisi nanti

            // Panggil metode Register dari UserRepository untuk menyimpan pengguna ke database
            bool isRegistered = _userRepository.Register(user);

            // Tampilkan pesan berhasil/gagal dan arahkan ke halaman login jika berhasil
            if (isRegistered)
            {
                MessageBox.Show("Registered succesfully");
                _navigationService.NavigateTo(typeof(Login)); // Navigasi ke halaman login
            }
            else
            {
                MessageBox.Show("Register failed. Please try again.");
            }
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
