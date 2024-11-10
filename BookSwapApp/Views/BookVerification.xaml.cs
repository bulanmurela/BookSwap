using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookSwapApp.Commands;
using BookSwapApp.Models;
using BookSwapApp.Repositories;

namespace BookSwapApp.Views
{
    /// <summary>
    /// Interaction logic for BookVerification.xaml
    /// </summary>
    public partial class BookVerification : Page
    {
        public ObservableCollection<Book> UnverifiedBooks { get; set; }
        public ICommand VerifyBookCommand { get; private set; }

        public BookVerification()
        {
            InitializeComponent();
            DataContext = this;

            LoadUnverifiedBooks(); 

            VerifyBookCommand = new RelayCommand(parameter => VerifyBook((Book)parameter));
        }
      
        private BitmapImage ConvertToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;

            var bitmapImage = new BitmapImage();
            using (var stream = new MemoryStream(imageData))
            {
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }

        private void LoadUnverifiedBooks()
        {
            BookRepository bookRepository = new BookRepository();
            var unverifiedBooks = bookRepository.GetUnverifiedBooks();

            foreach (var book in unverifiedBooks)
            {
                book.SetCoverImageSource(book.CoverImage);  
            }

            UnverifiedBooks = new ObservableCollection<Book>(unverifiedBooks);
        }

        private void VerifyBook(Book book)
        {
            if (book == null) return;

            Admin admin = new Admin();

            BookRepository bookRepository = new BookRepository();
            bool isVerified = bookRepository.VerifyBook(book.Id, admin, book);

            if (isVerified)
            {
                MessageBox.Show("Book verified successfully!", "Verification", MessageBoxButton.OK, MessageBoxImage.Information);
                UnverifiedBooks.Remove(book); // Remove the verified book from the list
            }
            else
            {
                MessageBox.Show("Failed to verify the book.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
