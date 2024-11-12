using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;
using BookSwapApp.Models;

namespace BookSwapApp.Views
{
    public partial class Profil : Page
    {
        private NavigationService _navigationService;
        private User _currentUser;
        private BookService _bookService;

        public Profil()
        {
            InitializeComponent();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
            _bookService = new BookService();

            _currentUser = ((App)Application.Current).CurrentUser;

            // Tampilkan data pengguna di halaman profil
            if (_currentUser != null)
            {
                txtUsername.Text = _currentUser.Username;
                txtEmail.Text = _currentUser.Email;
                txtAddress.Text = _currentUser.Address ?? "";
                txtPoints.Text = _currentUser.Points.ToString();

                // Ambil dan tampilkan buku yang sudah diverifikasi
                LoadVerifiedBooks();
            }
        }

        private void LoadVerifiedBooks()
        {
            if (_currentUser != null)
            {
                var verifiedBooks = _bookService.GetVerifiedBooksByUser(_currentUser.Username);
                VerifiedBooksListView.ItemsSource = verifiedBooks;
            }
        }

        // Handler untuk button HomePage
        private void btnHomePage_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(HomePage)); // Pastikan HomePage memiliki constructor tanpa parameter
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Show a confirmation message box
            var result = MessageBox.Show("Are you sure you want to logout?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // If the user clicks "Yes", log out and navigate to the login page
                _navigationService.NavigateTo(typeof(Login)); // Assumes Login is the name of the login page class
                ((App)Application.Current).CurrentUser = null; // Clear the current user session
            }
            // If the user clicks "No", do nothing and stay on the profile page
        }
    }
}
