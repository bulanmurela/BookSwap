using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwapApp
{  
    internal class Admin : User
    {
        public Admin(string username, string email, string password) : base(username, email, password)
        {
        }

        public override bool Login(string username, string password) 
        {
            if (base.Login(username, password))
            {
                Console.WriteLine("Login berhasil sebagai admin!");
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
        }
    }
}
