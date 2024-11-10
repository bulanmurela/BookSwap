using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookSwapApp.Services;
using BookSwapApp.Models;

namespace BookSwapApp.ViewModels
{
    internal class SwapRequestViewModel
    {
        private readonly BookService bookService;

        public SwapRequestViewModel()
        {
            bookService = new BookService();
        }

        public void DisplayBookAndOwnerDetails(Book book)
        {
            string details = bookService.GetBookAndOwnerDetails(book);
            Console.WriteLine(details);
        }
    }
}