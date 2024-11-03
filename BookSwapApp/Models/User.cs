using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace BookSwapApp
{
    internal class User
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        private string Password { get; set; }
        public int Points { get; private set; }

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

        public virtual bool Register(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;

            Console.WriteLine("Pengguna berhasil terdaftar.");
            return true;
        }

        public virtual bool Login(string username, string password)
        {
            if (Username == username && Password == password)
            {
                Console.WriteLine("Login berhasil.");
                return true;
            }
            else
            {
                Console.WriteLine("Login gagal. Username atau password salah.");
                return false;
            }
        }

        public void UploadBook(Book book)
        {
            uploadedBooks.Add(book);
            EarnPoints(1);
            Console.WriteLine($"Buku '{book.Title}' berhasill diunggah.");
        }

        public void RequestSwap(SwapRequest swapRequest)
        {
            swapRequests.Add(swapRequest);
            Console.WriteLine("Permintaan tukar buku berhasil dibuat.");
        }

        public void WriteReview(Review review)
        {
            reviewsWritten.Add(review);
            Console.WriteLine("Ulasan berhasil ditulis.");
        }

        public void EarnPoints(int points)
        {
            Points += points;
            Console.WriteLine($"{points} poin berhasil ditambahkan. Total poin: {Points}");
        }
    }
}
