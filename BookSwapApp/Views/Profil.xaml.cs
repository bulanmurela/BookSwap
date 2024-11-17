using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;
using BookSwapApp.Models;
using BookSwapApp.Repositories;

namespace BookSwapApp.Views
{
    public partial class Profil : Page
    {
        private NavigationService _navigationService;
        private User _currentUser;
        private BookService _bookService;
        private UserRepository _userRepository;
        private bool isEditMode = false;

        public Profil()
        {
            InitializeComponent();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
            _bookService = new BookService();
            _userRepository = new UserRepository();
            _currentUser = ((App)Application.Current).CurrentUser;

            if (_currentUser != null)
            {
                txtUsername.Text = _currentUser.Username;
                txtEmail.Text = _currentUser.Email;
                txtAddress.Text = _currentUser.Address ?? "";
                txtPoints.Text = _currentUser.Points.ToString();

                LoadVerifiedBooks();
            }

            txtAddress.IsReadOnly = true;
        }

        private void LoadVerifiedBooks()
        {
            if (_currentUser != null)
            {
                var verifiedBooks = _bookService.GetVerifiedBooksByUser(_currentUser.Username);
                VerifiedBooksListView.ItemsSource = verifiedBooks;
            }
        }

        private void btnHomePage_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(HomePage));
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _navigationService.NavigateTo(typeof(Login)); 
                ((App)Application.Current).CurrentUser = null; 
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (isEditMode)
            {
                _currentUser.Address = txtAddress.Text; 
                txtAddress.IsReadOnly = true;           
                btnEdit.Content = "Edit";              
                isEditMode = false;

                bool isSaved = _userRepository.UpdateUserAddress(_currentUser);
                if (isSaved)
                {
                    MessageBox.Show("Address updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update address.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtAddress.IsReadOnly = false;          
                btnEdit.Content = "Save";               
                isEditMode = true;
            }
        }
    }
}
