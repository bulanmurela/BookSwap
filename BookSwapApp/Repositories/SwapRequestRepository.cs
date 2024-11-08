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

        public bool RequestSwap(User requestingUser, Book requestedBook, Book offeredBook)
        {
            using (IDbConnection db = dbHelpers.OpenConnection())
            {
                var query = "INSERT INTO SwapRequests (RequestingUserId, RequestedBookId, OfferedBookId, Status) VALUES (@RequestingUserId, @RequestedBookId, @OfferedBookId, 'Pending')";
                var result = db.Execute(query, new
                {
                    RequestingUserUsername = requestingUser.Username,
                    RequestedBookId = requestedBook.Id,
                    OfferedBookId = offeredBook.Id
                });
                Console.WriteLine("Permintaan tukar buku berhasil dibuat.");
                return result > 0;
            }
        }
    }
}
