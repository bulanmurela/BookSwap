using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookSwapApp.Services;
using BookSwapApp.Models;
using BookSwapApp.Repositories;
using System.Windows;

namespace BookSwapApp.ViewModels
{
    internal class SwapRequestViewModel
    {
        private readonly BookService bookService;
        private readonly SwapRequestRepository _swapRequestRepository;

        public List<SwapRequest> SentRequests { get; private set; }
        public List<SwapRequest> ReceivedRequests { get; private set; }

        public SwapRequestViewModel()
        {
            bookService = new BookService();
            SentRequests = new List<SwapRequest>();
            ReceivedRequests = new List<SwapRequest>();
            _swapRequestRepository = new SwapRequestRepository();

        }

        public void LoadSwapRequests(User currentUser)
        {
            try
            {
                SentRequests = _swapRequestRepository.GetSentRequests(currentUser.Username);
                // Fetch received requests, and include requester details (email and address)
                ReceivedRequests = _swapRequestRepository.GetReceivedRequests(currentUser.Username).Select(request => {
                    request.RequesterEmail = request.Requester.Email; // Add Requester Email
                    request.RequesterAddress = request.Requester.Address; // Add Requester Address
                    return request;
                }).ToList();
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
                MessageBox.Show("No book selected or user information is missing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Check if the owner information is missing
            if (selectedBook.Owner == null)
            {
                MessageBox.Show("Owner information is missing for the selected book.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Check if the owner and requester are the same user
            if (selectedBook.Owner.Username == currentUser.Username)
            {
                MessageBox.Show("You cannot request to swap your own book.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var swapRequest = new SwapRequest
            {
                Requester = currentUser,
                Owner = selectedBook.Owner,
                Book = selectedBook,
                Status = "Pending",
                RequestDate = DateTime.Now // Set request date
            };

            // Attempt to create the swap request
            bool isCreated = _swapRequestRepository.CreateSwapRequest(swapRequest);

            if (!isCreated)
            {
                MessageBox.Show("Failed to create swap request.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            MessageBox.Show("Swap request successfully created.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }


        public bool ApproveSwapRequest(SwapRequest request)
        {
            try
            {
                request.Status = "Approved";
                request.ResponseDate = DateTime.Now;

                bool success = _swapRequestRepository.UpdateSwapRequestStatus(request.Id, request.Status);

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

                bool success = _swapRequestRepository.UpdateSwapRequestStatus(request.Id, request.Status);

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
