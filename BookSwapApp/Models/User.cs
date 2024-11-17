using Dapper;
using BookSwapApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BCrypt.Net;

namespace BookSwapApp.Models
{
    public class User
    {
        private readonly DatabaseHelpers dbHelpers = new DatabaseHelpers();

        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        private string Password { get; set; }
        public int Points { get; private set; }

        private List<Book> uploadedBooks;
        private List<SwapRequest> swapRequests;

        public User() { }

        internal string GetHashedPassword()
        {
            return Password;
        }

        public User(string username, string email, string address, string plainPassword)
        {
            Username = username;
            Email = email;
            Address = address;
            Password = HashPassword(plainPassword);
            Points = 0;
            uploadedBooks = new List<Book>();
            swapRequests = new List<SwapRequest>();
        }

        public static string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public bool VerifyPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, Password);
        }

        public void EarnPoints(int points)
        {
            Points += points;
        }
    }
}
