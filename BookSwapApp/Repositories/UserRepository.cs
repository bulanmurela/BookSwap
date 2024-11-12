using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using BookSwapApp.Helpers;
using System.Security.Policy;
using BookSwapApp.Models;

namespace BookSwapApp.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseHelpers dbHelpers = new DatabaseHelpers();

        // Metode untuk mengambil pengguna berdasarkan username
        public User GetUserByUsername(string username)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = "SELECT * FROM public.User WHERE Username = @Username";
                return db.QueryFirstOrDefault<User>(query, new { Username = username });
            }
        }

        // Metode untuk menambahkan pengguna baru dengan password yang di-hash
        public bool Register(User user)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = "INSERT INTO public.User (Username, Email, Password, Address, Points) VALUES (@Username, @Email, @Password, @Address, @Points)";
                var result = db.Execute(query, new
                {
                    user.Username,
                    user.Email,
                    Password = user.GetHashedPassword(), // Hash password sebelum disimpan
                    user.Address,
                    user.Points
                });
                return result > 0;
            }
        }

        // Metode Login untuk memverifikasi username dan password
        public virtual User Login(string username, string plainPassword)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = "SELECT * FROM public.User WHERE Username = @Username";
                var user = db.QueryFirstOrDefault<User>(query, new { Username = username });

                // Verifikasi password yang diinput dengan password yang di-hash
                if (user != null && user.VerifyPassword(plainPassword))
                {
                    Console.WriteLine("Login berhasil.");
                    return user;
                }
                else
                {
                    Console.WriteLine("Login gagal. Username atau password salah.");
                    return null;
                }


            }
        }

        public bool EarnPoints(User user, int points)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                user.EarnPoints(points); // Perbarui poin pada objek `User`
                var query = "UPDATE public.User SET Points = @Points WHERE Id = @UserId";
                var result = db.Execute(query, new
                {
                    Points = user.Points,
                    UserUsername = user.Username
                });
                Console.WriteLine($"{points} poin berhasil ditambahkan. Total poin: {user.Points}");
                return result > 0;
            }
        }

        public bool UpdateUserAddress(User user)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = "UPDATE public.User SET Address = @Address WHERE Username = @Username";
                var result = db.Execute(query, new
                {
                    Address = user.Address,
                    Username = user.Username
                });
                return result > 0; // Returns true if at least one row was updated
            }
        }

    }
}