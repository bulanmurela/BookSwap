using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;
using BookSwapApp.Repositories;
using BookSwapApp.Models;

namespace BookSwapApp.Views
{
    public partial class StatusRequest : Page
    {
        private NavigationService _navigationService;
        private readonly SwapRequestRepository _swapRequestRepository;

        public StatusRequest()
        {
            InitializeComponent();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
        }

        private void GoToHome(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(HomePage));
        }

        private void GoBack(object sender, ContextMenuEventArgs e)
        {
            _navigationService.GoBack();
        }

        private void IconGoBack(object sender, ContextMenuEventArgs e)
        {
            _navigationService.GoBack();
        }

        private void GoToProfile(object sender, ContextMenuEventArgs e)
        {
            _navigationService.NavigateTo(typeof(Profil));
        }
        public StatusRequest(User currentUser)
        {
            _swapRequestRepository = new SwapRequestRepository();

            // Populate Sent Requests
            var sentRequests = _swapRequestRepository.GetSentRequests(currentUser.Username);
            SentRequestsGrid.ItemsSource = sentRequests;

            // Populate Received Requests with email and address info
            var receivedRequests = _swapRequestRepository.GetReceivedRequests(currentUser.Username).Select(request => {
                request.RequesterEmail = request.Requester.Email;
                request.RequesterAddress = request.Requester.Address;
                return request;
            }).ToList();
            ReceivedRequestsGrid.ItemsSource = receivedRequests;
        }
    }
}