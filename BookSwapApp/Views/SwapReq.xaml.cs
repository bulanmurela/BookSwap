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

        private readonly SwapRequestViewModel _swapRequestViewModel;
        private readonly BookRepository _bookRepository;
        private Book _selectedBook;
        private readonly User _currentUser;
        private int _bookId;

        public SwapReq()
        {
            InitializeComponent();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
            _bookRepository = new BookRepository();
        }

        public SwapReq(int bookId, User currentUser) : this()
        {
            _selectedBook = _bookRepository.GetBookDetails(bookId);

            _currentUser = currentUser;
            _swapRequestViewModel = new SwapRequestViewModel();

            if (_selectedBook != null)
            {
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

        private void btnReqSwap_Click(object sender, RoutedEventArgs e)
        {
            // Use the ViewModel to request a swap and pass in current user details
            if (_swapRequestViewModel != null)
            {
                bool success = _swapRequestViewModel.RequestSwap(_selectedBook, _currentUser);

                if (success)
                {
                    MessageBox.Show("Swap request submitted successfully.");
                    _navigationService.NavigateTo(typeof(StatusRequest));
                }
                else
                {
                    MessageBox.Show("Failed to submit swap request.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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