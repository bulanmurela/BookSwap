using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using BookSwapApp.Models;

namespace BookSwapApp.Services
{
    internal class BookService
    {
        public string GetBookAndOwnerDetails(Book book)
        {
            if (book.Owner == null)
            {
                return "Pemilik buku tidak tersedia.";
            }

            // Tampilkan detail buku dan pemilik
            return $"Book Details:\n" +
                   $"- Title: {book.Title}\n" +
                   $"- Author: {book.Author}\n" +
                   $"- Genre: {book.Genre}\n" +
                   $"- Condition: {book.Condition}\n" +
                   $"\nOwner Details:\n" +
                   $"- Username: {book.Owner.Username}\n" +
                   $"- Email: {book.Owner.Email}\n" +
                   $"- Address: {book.Owner.Address}";
        }
    }
}