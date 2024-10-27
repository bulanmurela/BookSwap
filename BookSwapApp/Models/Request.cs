using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwapApp
{
    internal class Request : SwapRequest
    {
        public string RequestedBook { get; private set; }
        public User RequestingUser { get; private set; }

        public Request(User requestingUser, string requestedBook)
        {
            RequestingUser = requestingUser;
            RequestedBook = requestedBook;
        }

        public override string GetRequestDetails()
        {
            return $"{base.GetRequestDetails()}, Book Requested: {RequestedBook}, Requesting User: {RequestingUser.Username}";
        }
    }
}
