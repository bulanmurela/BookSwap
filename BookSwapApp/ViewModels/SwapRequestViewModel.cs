using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookSwapApp.Services;
using BookSwapApp.Models;
using BookSwapApp.Repositories;

namespace BookSwapApp.ViewModels
{
    internal class SwapRequestViewModel
    {
        private readonly BookService bookService;
        private readonly SwapRequestRepository swapRequestRepo;

        public List<SwapRequest> SentRequests { get; private set; }
        public List<SwapRequest> ReceivedRequests { get; private set; }

        public SwapRequestViewModel()
        {
            bookService = new BookService();
            SentRequests = new List<SwapRequest>();
            ReceivedRequests = new List<SwapRequest>();
        }

        public void LoadSwapRequests(User currentUser)
        {
            try
            {
                SentRequests = swapRequestRepo.GetSentRequests(currentUser.Username);
                ReceivedRequests = swapRequestRepo.GetReceivedRequests(currentUser.Username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading swap requests: {ex.Message}");
            }
        }



        public bool RequestSwap(Book selectedBook, User currentUser)
        {
            if (selectedBook == null || currentUser == null)
            {
                Console.WriteLine("Invalid book or user data.");
                return false;
            }

            // Create a new SwapRequest object
            var newRequest = new SwapRequest(currentUser, selectedBook.Owner, selectedBook);

            try
            {
                return swapRequestRepo.CreateSwapRequest(newRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create swap request: {ex.Message}");
                return false;
            }
        }

        public bool ApproveSwapRequest(SwapRequest request)
        {
            try
            {
                request.Status = "Approved";
                request.ResponseDate = DateTime.Now;

                bool success = swapRequestRepo.UpdateSwapRequestStatus(request.Id, request.Status);

                if (success)
                {
                    LoadSwapRequests(request.Requester); // Reload requests to reflect changes
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error approving swap request: {ex.Message}");
                return false;
            }
        }

        // Deny a swap request
        public bool DenySwapRequest(SwapRequest request)
        {
            try
            {
                request.Status = "Denied";
                request.ResponseDate = DateTime.Now;

                bool success = swapRequestRepo.UpdateSwapRequestStatus(request.Id, request.Status);

                if (success)
                {
                    LoadSwapRequests(request.Requester); // Reload requests to reflect changes
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error denying swap request: {ex.Message}");
                return false;
            }
        }

        public void DisplayBookAndOwnerDetails(Book book)
        {
            string details = bookService.GetBookAndOwnerDetails(book);
        }
    }
}
