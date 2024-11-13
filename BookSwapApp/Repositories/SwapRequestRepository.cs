using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using BookSwapApp.Helpers;
using BookSwapApp.Models;

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
                }

                return result > 0;
            }
        }

        // Method for the owner to respond to a swap request
        public bool UpdateSwapRequestStatus(int requestId, string newStatus)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = @"
            UPDATE public.SwapRequest 
            SET status = @Status, response_date = CURRENT_TIMESTAMP 
            WHERE id = @RequestId";

                var result = db.Execute(query, new { Status = newStatus, RequestId = requestId });

                // Jika statusnya 'Process', tambahkan logika tombol 'Complete'
                if (newStatus == "Process")
                {
                    // Ubah status menjadi 'Completed' jika tombol complete diklik
                    var completeQuery = "UPDATE public.SwapRequest SET status = 'Completed' WHERE id = @RequestId";
                    db.Execute(completeQuery, new { RequestId = requestId });
                }

                return result > 0;
            }
        }


        public List<SwapRequest> GetSentRequests(string requesterUsername)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = "SELECT * FROM public.SwapRequest WHERE requester_username = @RequesterUsername";
                return db.Query<SwapRequest>(query, new { RequesterUsername = requesterUsername }).ToList();
            }
        }

        public List<SwapRequest> GetReceivedRequests(string ownerUsername)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = "SELECT * FROM public.SwapRequest WHERE owner_username = @OwnerUsername";
                return db.Query<SwapRequest>(query, new { OwnerUsername = ownerUsername }).ToList();
            }
        }
    }
}