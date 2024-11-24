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
            string password = pwdPassword.Password;
            string confirmPassword = pwdConfirmPassword.Password;

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

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox == pwdPassword)
            {
                tbPasswordPlaceholder.Visibility = string.IsNullOrEmpty(passwordBox.Password)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox == pwdConfirmPassword)
            {
                tbConfirmPasswordPlaceholder.Visibility = string.IsNullOrEmpty(passwordBox.Password)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox == pwdPassword && string.IsNullOrEmpty(passwordBox.Password))
            {
                tbPasswordPlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (passwordBox == pwdConfirmPassword && string.IsNullOrEmpty(passwordBox.Password))
            {
                tbConfirmPasswordPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox == pwdPassword)
            {
                tbPasswordPlaceholder.Visibility = string.IsNullOrEmpty(pwdPassword.Password)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            else if (passwordBox == pwdConfirmPassword)
            {
                tbConfirmPasswordPlaceholder.Visibility = string.IsNullOrEmpty(pwdConfirmPassword.Password)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void btnTogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (pwdPassword.Visibility == Visibility.Visible)
            {
                txtPasswordVisible.Text = pwdPassword.Password;
                pwdPassword.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;
                tbPasswordPlaceholder.Visibility = string.IsNullOrEmpty(txtPasswordVisible.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            else
            {
                pwdPassword.Password = txtPasswordVisible.Text;
                txtPasswordVisible.Visibility = Visibility.Collapsed;
                pwdPassword.Visibility = Visibility.Visible;
                tbPasswordPlaceholder.Visibility = string.IsNullOrEmpty(pwdPassword.Password)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void btnToggleConfirmPassword_Click(object sender, RoutedEventArgs e)
        {
            if (pwdConfirmPassword.Visibility == Visibility.Visible)
            {
                txtConfirmPasswordVisible.Text = pwdConfirmPassword.Password;
                pwdConfirmPassword.Visibility = Visibility.Collapsed;
                txtConfirmPasswordVisible.Visibility = Visibility.Visible;
                tbConfirmPasswordPlaceholder.Visibility = string.IsNullOrEmpty(txtConfirmPasswordVisible.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            else
            {
                pwdConfirmPassword.Password = txtConfirmPasswordVisible.Text;
                txtConfirmPasswordVisible.Visibility = Visibility.Collapsed;
                pwdConfirmPassword.Visibility = Visibility.Visible;
                tbConfirmPasswordPlaceholder.Visibility = string.IsNullOrEmpty(pwdConfirmPassword.Password)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void Placeholder_Clicked(object sender, RoutedEventArgs e)
        {
            TextBlock placeholder = sender as TextBlock;

            if (placeholder != null)
            {
                if (placeholder == tbPasswordPlaceholder)
                {
                    pwdPassword.Focus();
                }
                else if (placeholder == tbConfirmPasswordPlaceholder)
                {
                    pwdConfirmPassword.Focus();
                }
            }
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text == "Email" || textBox.Text == "Username")
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

                    textBox.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }
    }
}
