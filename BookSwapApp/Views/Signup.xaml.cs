using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;
using BookSwapApp.Repositories;
using BookSwapApp.Models;
using System.Windows.Media;

namespace BookSwapApp.Views
{
    public partial class Signup : Page
    {
        private NavigationService _navigationService;
        private UserRepository _userRepository;

        public Signup() : this(new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame)) {} 

        public Signup(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _userRepository = new UserRepository();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (password != confirmPassword)
            {
                MessageBox.Show("Password doesn't match. Please try again.");
                return;
            }

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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(Login));
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text == "Email" || textBox.Text == "Username" || textBox.Text == "Password" || textBox.Text == "Confirm Password")
                {
                    textBox.Text = "";
                    textBox.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox.Name == "txtEmail")
                        textBox.Text = "Email";
                    else if (textBox.Name == "txtUsername")
                        textBox.Text = "Username";
                    else if (textBox.Name == "txtPassword")
                        textBox.Text = "Password";
                    else if (textBox.Name == "txtConfirmPassword")
                        textBox.Text = "Confirm Password";

                    textBox.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }

    }
}
