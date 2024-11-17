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
            string password = txtPassword.Text;

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
    }
}
