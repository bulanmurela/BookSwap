using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using BookSwapApp.Helpers;
using BookSwapApp.Models;
using System.Windows;
using System.Diagnostics;

namespace BookSwapApp.Repositories
{
    public class SwapRequestRepository
    {
        private readonly DatabaseHelpers dbHelpers = new DatabaseHelpers();

        // Method to create a new swap request
        public bool CreateSwapRequest(SwapRequest request)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
                    INSERT INTO public.SwapRequest 
                    (requester_username, owner_username, book_id, status) 
                    VALUES (@RequesterUsername, @OwnerUsername, @BookId, @Status)";

                var validStatuses = new[] { "Notifying Owner", "Approved", "Denied"};

                if (!validStatuses.Contains(request.Status))
                {
                    throw new ArgumentException($"Invalid status value: {request.Status}");
                }

                if (!validStatuses.Contains(request.Status))
                {
                    throw new ArgumentException($"Invalid status value: {request.Status}");
                }

                // Cek apakah requester memiliki cukup poin
                var pointsQuery = "SELECT points FROM public.User WHERE username = @RequesterUsername";
                var requesterPoints = db.QueryFirstOrDefault<int>(pointsQuery, new { RequesterUsername = request.Requester.Username });

                if (requesterPoints <= 0)
                {
                    MessageBox.Show("You don't have enough point to send a request.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;  
                }

                var result = db.Execute(query, new
                {
                    RequesterUsername = request.Requester.Username,
                    OwnerUsername = request.Owner.Username,
                    BookId = request.Book.Id,
                    Status = "Notifying Owner"
                });

                if (result > 0)
                {
                    // Deduct 1 point from the requester
                    var pointQuery = "UPDATE public.User SET points = points - 1 WHERE username = @RequesterUsername";
                    db.Execute(pointQuery, new { RequesterUsername = request.Requester.Username });

                    // Mark the book as hidden (unavailable) until the swap is accepted or denied
                    var hideBookQuery = "UPDATE public.Books SET is_visible = false WHERE id = @BookId";
                    db.Execute(hideBookQuery, new { BookId = request.Book.Id });
                }

                return result > 0;
            }
        }

        // Method for the owner to respond to a swap request
        public bool UpdateSwapRequestStatus(int requestId, string newStatus, int bookId)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
                    UPDATE public.SwapRequest 
                    SET status = @Status, response_date = CURRENT_TIMESTAMP 
                    WHERE id = @RequestId";

                var result = db.Execute(query, new { Status = newStatus, RequestId = requestId });

                if (result > 0)
                {
                    if (newStatus == "Approved")
                    {
                        // Remove book from the database if the swap is approved
                        var deleteBookQuery = "DELETE FROM public.Books WHERE id = @BookId";
                        db.Execute(deleteBookQuery, new { BookId = bookId });
                    }
                    else if (newStatus == "Denied")
                    {
                        // Make the book visible again if the swap is denied
                        var makeVisibleQuery = "UPDATE public.Books SET is_visible = true WHERE id = @BookId";
                        db.Execute(makeVisibleQuery, new { BookId = bookId });
                    }
                }

                return result > 0;
            }
        }


        public List<SwapRequest> GetSentRequests(string requesterUsername)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
        SELECT sr.id AS Id,
                b.title AS BookTitle,
                u.email AS OwnerEmail,
                u.address AS OwnerAddress,
                sr.status,
                sr.request_date AS RequestDate
        FROM public.SwapRequest sr
        JOIN public.Books b ON sr.book_id = b.id
        JOIN public.User u ON sr.owner_username = u.username
        WHERE sr.requester_username = @RequesterUsername";

                Debug.WriteLine($"Executing GetSentRequests for requesterUsername: {requesterUsername}");

                var results = db.Query<SwapRequest>(query, new { RequesterUsername = requesterUsername }).ToList();


                // Set the request type to "Sent"
                foreach (var request in results)
                {
                    request.RequestType = "Sent";
                }

                // Debugging output for CombinedRequests content
                Debug.WriteLine("SentRequests content:");
                foreach (var request in results)
                {
                    Debug.WriteLine($"Id: {request.Id}, Book: {request.Book.Title}, Type: {request.RequestType}, Status: {request.Status}, Request Date: {request.RequestDate}");
                }

                return results;

            }
        }


        public List<SwapRequest> GetReceivedRequests(string ownerUsername)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
        SELECT sr.id AS Id,
                b.title AS BookTitle,
                u.email AS RequesterEmail,
                u.address AS RequesterAddress,
                sr.status,
                sr.request_date AS RequestDate
        FROM public.SwapRequest sr
        JOIN public.Books b ON sr.book_id = b.id
        JOIN public.User u ON sr.requester_username = u.username
        WHERE sr.owner_username = @OwnerUsername";

                Debug.WriteLine($"Executing GetReceivedRequests for ownerUsername: {ownerUsername}");

                var results = db.Query<SwapRequest>(query, new { OwnerUsername = ownerUsername }).ToList();

                // Set the request type to "Requested"
                foreach (var request in results)
                {
                    request.RequestType = "Requested";
                }

                // Debugging output for CombinedRequests content
                Debug.WriteLine("ReceivedRequests content:");
                foreach (var request in results)
                {
                    Debug.WriteLine($"Id: {request.Id}, Book: {request.Book.Title}, Type: {request.RequestType}, Status: {request.Status}, Request Date: {request.RequestDate}");
                }

                return results;
            }
        }

    }
}