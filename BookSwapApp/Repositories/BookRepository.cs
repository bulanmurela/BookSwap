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
                    (title, author, genre, condition, availability_status, verification_status, owner_username, cover_image) 
                    VALUES (@Title, @Author, @Genre, @Condition, @AvailabilityStatus, @VerificationStatus, @OwnerUsername, @CoverImage)";

                var result = db.Execute(query, new
                {
                    book.Title,
                    book.Author,
                    book.Genre,
                    book.Condition,
                    book.AvailabilityStatus,
                    VerificationStatus = false,
                    OwnerUsername = user.Username,
                    CoverImage = coverImageBytes
                });

                MessageBox.Show($"The book '{book.Title}' has been successfully uploaded and is awaiting verification by the admin.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return result > 0;
            }
        }

        // Method for admin to verify the book and award points to the user
        public bool VerifyBook(int bookId, Admin admin, Book book)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                using (var transaction = db.BeginTransaction())
                {
                    // Step 1: Verify the book
                    var verifyQuery = @"
                        UPDATE public.Books 
                        SET verification_status = true
                        WHERE id = @BookId";

                    var verifyResult = db.Execute(verifyQuery, new { BookId = bookId }, transaction: transaction);

                    if (verifyResult > 0)
                    {
                        // Step 2: Retrieve the book owner's user ID
                        var getUserQuery = @"
                            SELECT owner_username
                            FROM public.Books 
                            WHERE id = @BookId";

                        var ownerUsername = db.QuerySingle<string>(getUserQuery, new { BookId = bookId }, transaction: transaction);

                        // Step 3: Update the user's points in the database
                        var updatePointsQuery = "UPDATE public.user SET points = points + 1 WHERE username = @OwnerUsername";
                        db.Execute(updatePointsQuery, new { OwnerUsername = ownerUsername }, transaction: transaction);

                        transaction.Commit();

                        // Call MarkAsVerified() to update the internal state of the Book object
                        book.MarkAsVerified();

                        MessageBox.Show("The book has been successfully verified by the admin, and 1 point has been added to the user.", "Verification Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }

                    // Rollback if verification failed
                    transaction.Rollback();
                    MessageBox.Show("Verification failed. Please try again.", "Verification Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
    }
}