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

                var validStatuses = new[] { "Notifying Owner", "Approved", "Denied", "Completed"};

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

        public List<SwapRequest> GetSentRequests(string requesterUsername)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
                SELECT sr.id AS Id,
                       sr.status AS Status,
                       sr.request_date AS RequestDate,
                       b.title AS Title,
                       u.email AS Email,
                       u.address AS Address
                FROM public.SwapRequest sr
                JOIN public.Books b ON sr.book_id = b.id
                JOIN public.User u ON sr.owner_username = u.username
                WHERE sr.requester_username = @RequesterUsername";

                Debug.WriteLine($"Executing GetSentRequests for requesterUsername: {requesterUsername}");

                var results = db.Query<SwapRequest, Book, User, SwapRequest>(
                    query,
                    (swapRequest, book, owner) =>
                    {
                        swapRequest.Book = book;
                        swapRequest.Owner = owner;
                        swapRequest.RequestType = "Sent";  // Menandai tipe request sebagai "Sent"
                        return swapRequest;
                    },
                    new { RequesterUsername = requesterUsername },
                    splitOn: "Title,Email").ToList();

                // Debugging output for CombinedRequests content
                //Debug.WriteLine("SentRequests content:");
                //foreach (var request in results)
                //{
                //    Debug.WriteLine($"Id: {request.Id}, Book: {request.Book.Title}, Type: {request.RequestType}, Status: {request.Status}, Request Date: {request.RequestDate}");
                //}

                return results;
            }
        }

        public List<SwapRequest> GetReceivedRequests(string ownerUsername)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
                SELECT sr.id AS Id,
                       sr.status AS Status,
                       sr.request_date AS RequestDate,
                       b.title AS Title,
                       u.email AS Email,
                       u.address AS Address
                FROM public.SwapRequest sr
                JOIN public.Books b ON sr.book_id = b.id
                JOIN public.User u ON sr.requester_username = u.username
                WHERE sr.owner_username = @OwnerUsername";

                Debug.WriteLine($"Executing GetReceivedRequests for ownerUsername: {ownerUsername}");

                var results = db.Query<SwapRequest, Book, User, SwapRequest>(
                    query,
                    (swapRequest, book, requester) =>
                    {
                        swapRequest.Book = book;
                        swapRequest.Requester = requester;
                        swapRequest.RequestType = "Requested";  // Menandai tipe request sebagai "Requested"
                        return swapRequest;
                    },
                    new { OwnerUsername = ownerUsername },
                    splitOn: "Title,Email").ToList();

                // Debugging output for CombinedRequests content
                //Debug.WriteLine("ReceivedRequests content:");
                //foreach (var request in results)
                //{
                //    Debug.WriteLine($"Id: {request.Id}, Book: {request.Book.Title}, Type: {request.RequestType}, Status: {request.Status}, Request Date: {request.RequestDate}");
                //}

                return results;
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

                var validStatuses = new[] { "Notifying Owner", "Approved", "Denied", "Completed" };
                if (!validStatuses.Contains(newStatus))
                {
                    throw new ArgumentException($"Invalid status value: {newStatus}");
                }

                var result = db.Execute(query, new { Status = newStatus, RequestId = requestId });
                Debug.WriteLine($"Updated SwapRequest. Rows affected: {result}");

                if (result > 0)
                {
                    if (newStatus == "Denied")
                    {
                        // Make the book visible again
                        var getBookIdQuery = @"
                                            SELECT book_id 
                                            FROM public.SwapRequest 
                                            WHERE id = @RequestId";
                        var fetchedBookId = db.QueryFirstOrDefault<int>(getBookIdQuery, new { RequestId = requestId });

                        Debug.WriteLine($"BookId found for requestId {requestId}: {fetchedBookId}");

                        if (fetchedBookId > 0)
                        {
                            var makeVisibleQuery = "UPDATE public.Books SET is_visible = true WHERE id = @BookId";
                            var updateVisibilityResult = db.Execute(makeVisibleQuery, new { BookId = fetchedBookId });

                            // Logging the result of the update visibility operation
                            if (updateVisibilityResult > 0)
                            {
                                Debug.WriteLine($"Successfully set is_visible to true for Denied book with BookId: {fetchedBookId}. Rows affected: {updateVisibilityResult}");
                            }
                            else
                            {
                                Debug.WriteLine($"Failed to update is_visible for BookId: {fetchedBookId}. No rows were affected.");
                            }
                        }
                        else
                        {
                            Debug.WriteLine($"BookId not found for requestId: {requestId}");
                        }


                        // Return the point to the requester
                        var getRequesterQuery = @"
                                                SELECT requester_username 
                                                FROM public.SwapRequest 
                                                WHERE id = @RequestId";
                        var requesterUsername = db.QueryFirstOrDefault<string>(getRequesterQuery, new { RequestId = requestId });
                        Debug.WriteLine($"Requester username found: {requesterUsername}");

                        if (!string.IsNullOrEmpty(requesterUsername))
                        {
                            var returnPointQuery = "UPDATE public.User SET points = points + 1 WHERE username = @RequesterUsername";
                            var updatePointsResult = db.Execute(returnPointQuery, new { RequesterUsername = requesterUsername });
                            Debug.WriteLine($"Returned 1 point to requester. Rows affected: {updatePointsResult}");
                        }
                        else
                        {
                            Debug.WriteLine($"Requester username not found for requestId: {requestId}");
                        }
                    }
                }
                return result > 0;
            }
        }
        
    }
}