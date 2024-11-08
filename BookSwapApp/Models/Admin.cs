using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookSwapApp
{  
    public class Admin : User
    {
        private static readonly string fixedUsername = "adminBookSwap";
        private static readonly string fixedPassword = "onlyadminBookSwap123";

        public Admin() : base(fixedUsername, "adminBookSwap@bookswap.com", "Admin Address", fixedPassword)
        {
        }

        public bool AdminLogin(string username, string password) 
        {
            if (username == fixedUsername && password == fixedPassword)
            {
                Console.WriteLine("Login berhasil sebagai admin!");
                return true;
            }
            else
            {
                Console.WriteLine("Login gagal. Username atau password salah untuk admin.");
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
