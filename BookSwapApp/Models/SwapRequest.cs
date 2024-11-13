using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public Visibility SwapActionVisibility
        {
            get
            {
                return (Status == "Approved" || Status == "Completed") ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public SwapRequest() { }

        public SwapRequest(User requester, User owner, Book book)
        {
            Requester = requester;
            Owner = owner;
            Book = book;
            Status = "Notifying Owner";
            RequestDate = DateTime.Now;
        }

        public void UpdateStatus(string newStatus)
        {
            // Ensure that only valid statuses are allowed
            if (newStatus == "Notifying Owner" || newStatus == "Approved" || newStatus == "Denied")
            {
                Status = newStatus;
            }
            else
            {
                throw new ArgumentException("Invalid status value.");
            }
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