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

            // Display user data on the profile page
            if (_currentUser != null)
            {
                txtUsername.Text = _currentUser.Username;
                txtEmail.Text = _currentUser.Email;
                txtAddress.Text = _currentUser.Address ?? "";
                txtPoints.Text = _currentUser.Points.ToString();

                // Load and display verified books
                LoadVerifiedBooks();
            }

            // Set address field as read-only by default
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

        // Handler for the Home button
        private void btnHomePage_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(HomePage)); // Ensure HomePage has a parameterless constructor
        }

        // Handler for the Logout button
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

        // Handler for the Edit/Save button
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (isEditMode)
            {
                // Save mode: save the updated address and revert button text
                _currentUser.Address = txtAddress.Text; // Update current user's address
                txtAddress.IsReadOnly = true;           // Make address field read-only
                btnEdit.Content = "Edit";               // Change button text back to "Edit"
                isEditMode = false;


                // Save changes to the database
                bool isSaved = _userRepository.UpdateUserAddress(_currentUser); // Save address to database
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
                // Edit mode: make the address field editable and change button text to "Save"
                txtAddress.IsReadOnly = false;          // Make address field editable
                btnEdit.Content = "Save";               // Change button text to "Save"
                isEditMode = true;
            }
        }
    }
}
