using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;
using BookSwapApp.Models;
using BookSwapApp.Repositories;
using System.Security.Policy;
using System.Windows.Media;

namespace BookSwapApp.Views
{
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
            string password = pwdPassword.Visibility == Visibility.Visible
                ? pwdPassword.Password
                : txtPasswordVisible.Text;

            Admin admin = new Admin();
            if (admin.AdminLogin(username, password))
            {
                MessageBox.Show("Login successful as admin!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.NavigateTo(typeof(BookVerification));
                return;
            }

            var user = _userRepository.Login(username, password);
            if (user != null)
            {
                ((App)Application.Current).CurrentUser = user;

                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                _navigationService.NavigateTo(typeof(HomePage));
            }
            else
            {
                MessageBox.Show("Login failed. Incorrect username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(Signup));
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text == "Username" || textBox.Text == "Password")
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
                    if (textBox.Name == "txtUsername")
                        textBox.Text = "Username";
                    else if (textBox.Name == "txtPassword")
                        textBox.Text = "Password";
        
                    textBox.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            tbPasswordPlaceholder.Visibility = string.IsNullOrEmpty(pwdPassword.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            tbPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            tbPasswordPlaceholder.Visibility = string.IsNullOrEmpty(pwdPassword.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void Placeholder_Clicked(object sender, RoutedEventArgs e)
        {
            pwdPassword.Focus();
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

    }
}
