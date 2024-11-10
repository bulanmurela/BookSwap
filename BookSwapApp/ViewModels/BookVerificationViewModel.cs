using BookSwapApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using BookSwapApp.Models;
using BookSwapApp.Commands;

namespace BookSwapApp.ViewModels
{
    public class BookVerificationViewModel : ViewModelBase
    {
        private readonly BookRepository _bookRepository;
        public ObservableCollection<Book> Books { get; set; }
        public ICommand VerifyBookCommand { get; }

        public BookVerificationViewModel()
        {
            _bookRepository = new BookRepository();
            Books = new ObservableCollection<Book>(_bookRepository.GetUnverifiedBooks());

            VerifyBookCommand = new RelayCommand(ExecuteVerifyBook);
        }

        private void ExecuteVerifyBook(object parameter)
        {
            if (parameter is Book book)
            {
                VerifyBook(book);
            }
        }

        private void VerifyBook(Book book)
        {
            if (book == null) return;

            Admin admin = new Admin();
            bool success = _bookRepository.VerifyBook(book.Id, admin, book);

            if (success)
            {
                book.VerificationStatus = true;
                MessageBox.Show("Book has been verified successfully.");
                Books.Remove(book);
            }
            else
            {
                MessageBox.Show("Book verification failed.");
            }
        }
    }
}
