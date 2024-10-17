using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap
{
    internal class User
    {
        private string Username { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private int Points { get; set; }

        private List<Book> uploadedBooks;
        private List<SwapRequest> swapRequests;
        private List<Review> reviewsWritten;

        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
            Points = 0;
            uploadedBooks = new List<Book>();
            swapRequests = new List<SwapRequest>();
            reviewsWritten = new List<Review>();
        }

        public bool Register(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
            Points = 0;

            Console.WriteLine("User registered successfully.");
            return true;
        }

        public bool Login(string username, string password)
        {
            if (Username == username && Password == password)
            {
                Console.WriteLine("Login successful.");
                return true;
            }
            else
            {
                Console.WriteLine("Login failed. Incorrect username or password.");
                return false;
            }
        }

        public void UploadBook(Book book)
        {
            uploadedBooks.Add(book);
            EarnPoints(1);
            Console.WriteLine($"Book '{book.BookTitle}' uploaded successfully.");
        }

        public void RequestSwap(SwapRequest swapRequest)
        {
            swapRequests.Add(swapRequest);
            Console.WriteLine("Swap request created successfully.");
        }

        public void WriteReview(Review review)
        {
            reviewsWritten.Add(review);
            Console.WriteLine("Review written successfully.");
        }

        public void EarnPoints(int points)
        {
            Points += points;
            Console.WriteLine($"{points} points earned. Total points: {Points}");
        }
    }
}
