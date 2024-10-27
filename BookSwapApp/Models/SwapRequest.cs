using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwapApp
{
    internal abstract class SwapRequest
    {
        public string RequestID { get; private set; }
        public string RequestDate { get; private set; }
        public string Status { get; protected set; }

        protected SwapRequest()
        {
            RequestID = Guid.NewGuid().ToString();
            RequestDate = DateTime.Now.ToString("dd/MM/yyyy");
            Status = "Requested";
        }

        public virtual string UpdateStatus(string newStatus)
        {
            Status = newStatus;
            return Status;
        }

        public virtual string GetRequestDetails()
        {
            return $"Request ID: {RequestID}, Date: {RequestDate}, Status: {Status}";
        }
    }
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