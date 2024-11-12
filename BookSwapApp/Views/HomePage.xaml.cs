using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Models;
using BookSwapApp.Services;
using BookSwapApp.Views;

namespace BookSwapApp.Views
{
    public partial class HomePage : Page
    {
        private NavigationService _navigationService;
        private readonly User currentUser;
        private readonly BookService _bookService;
        public HomePage() : this(new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame))
        {
        }

        public HomePage(User user, NavigationService navigationService) : this(navigationService)
        {
            currentUser = user;
            _bookService = new BookService();
        }

        public HomePage(NavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _bookService = new BookService();
        }

        private void GoToProfile(object sender, ContextMenuEventArgs e)
        {

            _navigationService.NavigateTo(typeof(Profil));
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtInsertKeyword.Text; // Assuming SearchBox is the name of the search input
            var books = _bookService.SearchVerifiedBooks(keyword);

            if (books.Any())
            {
                // Display the list of books, e.g., in a ListView or DataGrid
                BookList.ItemsSource = books; // Assuming BookList is a ListView or similar control in HomePage.xaml
            }
            else
            {
                MessageBox.Show("No books found with the given keyword.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Event handler for when a book is selected from the list
        private void BookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookList.SelectedItem is Book selectedBook)
            {
                int selectedBookId = selectedBook.Id;
                // Navigate to SwapReq page with the selected book details
                _navigationService.NavigateTo(typeof(SwapReq), selectedBookId);
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            var currentUser = app.CurrentUser;

            if (currentUser == null)
            {
                MessageBox.Show("User information is missing. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _navigationService.NavigateTo(typeof(Login)); // Redirect to login if currentUser is null
                return;
            }

            // Navigate to UploadBook with currentUser
            _navigationService.NavigateTo(typeof(UploadBook), currentUser, _navigationService);
        }
    }
}