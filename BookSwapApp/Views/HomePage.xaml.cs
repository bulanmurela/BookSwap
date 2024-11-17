using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        private void GoToProfile(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(Profil));
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtInsertKeyword.Text; 
            var books = _bookService.SearchVerifiedBooks(keyword);

            if (books.Any())
            {
                BookList.ItemsSource = books; 
            }
            else
            {
                MessageBox.Show("No books found with the given keyword.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookList.SelectedItem is Book selectedBook)
            {
                int selectedBookId = selectedBook.Id;
                var app = (App)Application.Current;
                var currentUser = app.CurrentUser;

                if (currentUser != null)
                {
                    _navigationService.NavigateTo(typeof(SwapReq), selectedBookId, currentUser);
                }
                else
                {
                    MessageBox.Show("Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            var currentUser = app.CurrentUser;

            if (currentUser == null)
            {
                MessageBox.Show("User information is missing. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _navigationService.NavigateTo(typeof(Login)); 
                return;
            }

            _navigationService.NavigateTo(typeof(UploadBook), currentUser, _navigationService);
        }

        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(StatusRequest));
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text == "Insert book title / author here")
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
                    if (textBox.Name == "txtInsertKeyword")
                        textBox.Text = "Insert book title / author here";
                    textBox.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }
    }
}