using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;
using BookSwapApp.Models;
using BookSwapApp.ViewModels;
using BookSwapApp.Repositories;
using System.Windows.Automation;
using System.Windows.Media.Imaging;

namespace BookSwapApp.Views
{
    public partial class SwapReq : Page
    {
        private NavigationService _navigationService;
        private Book _selectedBook;
        private readonly BookRepository _bookRepository;
        private int _bookId;

        public SwapReq()
        {
            InitializeComponent();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
            _bookRepository = new BookRepository();
        }

        public SwapReq(int bookId) : this()
        {
            _selectedBook = _bookRepository.GetBookDetails(bookId);
            if (_selectedBook != null)
            {
                AssignOwnerToBook();
                DisplayBookDetails();
            }
            else
            {
                MessageBox.Show("Book not found or not verified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayBookDetails()
        {
            if (_selectedBook != null)
            {
                txtTitle.Text = _selectedBook.Title;
                txtAuthor.Text = _selectedBook.Author;
                txtGenre.Text = _selectedBook.Genre;
                txtCondition.Text = _selectedBook.Condition;
                imgBookCover.Source = _selectedBook.CoverImageSource;
                txtOwnerEmail.Text = _selectedBook.OwnerEmail ?? "No email available";
                txtOwnerAddress.Text = _selectedBook.OwnerAddress ?? "No address available";
            }
        }

        private void AssignOwnerToBook()
        {
            if (_selectedBook != null && _selectedBook.Owner == null)
            {
                var owner = new User
                {
                    Username = _selectedBook.OwnerUsername,
                    Email = _selectedBook.OwnerEmail,
                    Address = _selectedBook.OwnerAddress
                };
                _selectedBook.AssignOwner(owner);
            }
        }

        private void btnReqSwap_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(StatusRequest));
        }

        private void GoToProfile(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(Profil));
        }

        private void GoToHome(object sender, ContextMenuEventArgs e)
        {
            _navigationService.NavigateTo(typeof(HomePage));
        }

        private void GoBack(object sender, ContextMenuEventArgs e)
        {
            _navigationService.GoBack();
        }

        private void IconGoBack(object sender, ContextMenuEventArgs e)
        {
            _navigationService.GoBack();
        }
    }
}
