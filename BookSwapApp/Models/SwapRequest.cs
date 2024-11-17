using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookSwapApp.Models
{
    public class SwapRequest
    {
        public int Id { get; set; }
        public string RequestType { get; set; }
        public User Requester { get; set; }
        public User Owner { get; set; }
        public Book Book { get; set; } 

        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }

        private List<SwapRequest> swapRequests;

        public Visibility IsCompleteVisible { get; set; }
        public Visibility IsApproveVisible { get; set; }
        public Visibility IsDenyVisible { get; set; }
    

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
            if (newStatus == "Notifying Owner" || newStatus == "Approved" || newStatus == "Denied" || newStatus == "Completed")
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

        public void CompleteRequest()
        {
            Status = "Completed";
            ResponseDate = DateTime.Now;
        }
    }
}