using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwapApp.Models
{
    public class SwapRequest
    {
        public int Id { get; set; }
        public User Requester { get; set; }
        public User Owner { get; set; }
        public Book Book { get; set; }
        public string RequesterEmail { get; set; }  // New field
        public string RequesterAddress { get; set; }  // New field
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }

        public SwapRequest() { }

        public SwapRequest(User requester, User owner, Book book)
        {
            Requester = requester;
            Owner = owner;
            Book = book;
            Status = "Notifying Owner";
            RequestDate = DateTime.Now;
        }

        public void ApproveRequest()
        {
            Status = "Approved";
            ResponseDate = DateTime.Now;
        }

        public void DenyRequest()
        {
            Status = "Denied";
            ResponseDate = DateTime.Now;
        }
    }
}