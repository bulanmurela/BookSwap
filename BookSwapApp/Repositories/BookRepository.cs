using BookSwapApp.Helpers;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BookSwapApp.Models;

namespace BookSwapApp.Repositories
{
    public class BookRepository
    {
        private readonly DatabaseHelpers dbHelpers = new DatabaseHelpers();

        public bool UploadBook(User user, Book book)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = "INSERT INTO Books (Title, Author, Genre, Condition, AvailabilityStatus, VerificationStatus, UserId) VALUES (@Title, @Author, @Genre, @Condition, @AvailabilityStatus, @VerificationStatus, @UserId)";
                var result = db.Execute(query, new
                {
                    book.Title,
                    book.Author,
                    book.Genre,
                    book.Condition,
                    book.AvailabilityStatus,
                    book.VerificationStatus
                });

                // Tambahkan poin ke pengguna jika berhasil mengunggah buku
                if (result > 0)
                {
                    user.EarnPoints(1); // Menambah poin lokal
                    Console.WriteLine($"Buku '{book.Title}' berhasil diunggah.");
                }
                return result > 0;
            }
        }
    }
}
