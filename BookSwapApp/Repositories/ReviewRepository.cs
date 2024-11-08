using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using BookSwapApp.Helpers;
using BookSwapApp.Models;

namespace BookSwapApp.Repositories
{
    public class ReviewRepository
    {
        private readonly DatabaseHelpers dbHelpers = new DatabaseHelpers();

        public bool WriteReview(User user, Book book, string content, int rating)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = "INSERT INTO Reviews (UserUsername, BookId, Content, Rating) VALUES (@UserUsername, @BookId, @Content, @Rating)";
                var result = db.Execute(query, new
                {
                    UserUsername = user.Username,
                    BookId = book.Id,
                    Content = content,
                    Rating = rating
                });
                Console.WriteLine("Ulasan berhasil ditulis.");
                return result > 0;
            }
        }
    }
}
