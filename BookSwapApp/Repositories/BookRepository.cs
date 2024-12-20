﻿using BookSwapApp.Helpers;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BookSwapApp.Models;
using System.Windows;

namespace BookSwapApp.Repositories
{
    public class BookRepository
    {
        private readonly DatabaseHelpers dbHelpers = new DatabaseHelpers();

        public bool UploadBook(User user, Book book, byte[] coverImageBytes)
        {
            if (user == null)
            {
                MessageBox.Show("User information is missing. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
                    INSERT INTO public.Books 
                    (title, author, genre, condition, verification_status, owner_username, cover_image) 
                    VALUES (@Title, @Author, @Genre, @Condition, @VerificationStatus, @OwnerUsername, @CoverImage)";

                var result = db.Execute(query, new
                {
                    book.Title,
                    book.Author,
                    book.Genre,
                    book.Condition,
                    VerificationStatus = false,
                    OwnerUsername = user.Username,
                    CoverImage = coverImageBytes
                });

                return result > 0;
            }
        }

        public List<Book> GetUnverifiedBooks()
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
                    SELECT 
                        id, 
                        title, 
                        author, 
                        cover_image AS CoverImage, 
                        owner_username AS OwnerUsername
                    FROM 
                        public.Books
                    WHERE 
                        verification_status = false";

                var books = db.Query<Book>(query).ToList();

                foreach (var book in books)
                {
                    book.SetCoverImageSource(book.CoverImage);
                }

                return books;
            }
        }

        public bool VerifyBook(int bookId, Admin admin, Book book)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                using (var transaction = db.BeginTransaction())
                {
                    //Verify the book
                    var verifyQuery = @"
                        UPDATE public.Books 
                        SET verification_status = true
                        WHERE id = @BookId";

                    var verifyResult = db.Execute(verifyQuery, new { BookId = bookId }, transaction: transaction);

                    if (verifyResult > 0)
                    {
                        //Retrieve the book owner's user ID
                        var getUserQuery = @"
                            SELECT owner_username
                            FROM public.Books 
                            WHERE id = @BookId";

                        var ownerUsername = db.QuerySingle<string>(getUserQuery, new { BookId = bookId }, transaction: transaction);

                        //Update the user's points in the database
                        var updatePointsQuery = 
                            "UPDATE public.User " +
                            "SET points = points + 1 " +
                            "WHERE username = @OwnerUsername";
                        db.Execute(updatePointsQuery, new { OwnerUsername = ownerUsername }, transaction: transaction);

                        transaction.Commit();

                        book.MarkAsVerified();

                        MessageBox.Show("The book has been successfully verified by the admin, and 1 point has been added to the user.", "Verification Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }

                    transaction.Rollback();
                    MessageBox.Show("Verification failed. Please try again.", "Verification Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
        private User GetUserByUsername(string username)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                return db.QuerySingleOrDefault<User>("SELECT * FROM public.Users WHERE username = @Username", new { Username = username });
            }
        }

        public List<Book> GetVerifiedBooksByUser(string username)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
            SELECT id, title, author, condition
            FROM public.Books
            WHERE verification_status = true AND owner_username = @Username";

                var books = db.Query<Book>(query, new { Username = username }).ToList();
                return books;
            }
        }

        public List<Book> SearchVerifiedBooks(string keyword)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
                SELECT id, title, author, genre, condition
                FROM public.Books
                WHERE verification_status = true AND is_visible=true
                AND (title ILIKE @Keyword OR author ILIKE @Keyword)";

                var books = db.Query<Book>(query, new { Keyword = "%" + keyword + "%" }).ToList();
                return books;
            }
        }

        public Book GetBookDetails(int bookId)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
                            SELECT 
                                b.id, 
                                b.title, 
                                b.author, 
                                b.genre, 
                                b.condition, 
                                b.cover_image AS CoverImage, 
                                b.owner_username AS OwnerUsername, 
                                u.email AS OwnerEmail, 
                                u.address AS OwnerAddress
                            FROM 
                                public.Books b
                            JOIN 
                                public.User u ON b.owner_username = u.username
                            WHERE 
                                b.id = @BookId
                                AND b.verification_status = true";

                var book = db.QuerySingleOrDefault<Book>(query, new { BookId = bookId });

                if (book != null)
                {
                    book.SetCoverImageSource(book.CoverImage);
                }

                return book;
            }
        }

        public User GetBookOwnerByBookId(int bookId)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
            SELECT u.* FROM public.User u
            JOIN public.Books b ON b.owner_username = u.username
            WHERE b.id = @BookId";
                return db.Query<User>(query, new { BookId = bookId }).FirstOrDefault();
            }
        }
    }
}