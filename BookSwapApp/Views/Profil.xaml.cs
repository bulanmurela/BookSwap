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
    }
}
