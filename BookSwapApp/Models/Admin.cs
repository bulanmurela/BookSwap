using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookSwapApp.Models;

namespace BookSwapApp
{  
    public class Admin : User
    {
        private static readonly string fixedUsername = "admin";
        private static readonly string fixedPassword = "adminBS";

        public Admin() : base(fixedUsername, "adminBookSwap@bookswap.com", "Admin Address", fixedPassword)
        {
        }

        public bool AdminLogin(string username, string password) 
        {
            if (username == fixedUsername && password == fixedPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void VerifyBook(Book book, User user)
        {
            book.Verify(this); 
            user.EarnPoints(1); 
            Console.WriteLine($"Buku '{book.Title}' berhasil diverifikasi oleh admin, dan 1 poin diberikan kepada pengguna '{user.Username}'.");
        }
    }
}
