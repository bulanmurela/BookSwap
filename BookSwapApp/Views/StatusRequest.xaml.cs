using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;
using BookSwapApp.Repositories;
using BookSwapApp.Models;
using BookSwapApp.ViewModels;
using System.Linq;

namespace BookSwapApp.Views
{
    public partial class StatusRequest : Page
    {
        private NavigationService _navigationService;
        private readonly SwapRequestRepository _swapRequestRepository;
        private readonly User _currentUser;

        //public StatusRequest()
        //{
        //    InitializeComponent();
        //    this.DataContext = new StatusRequestViewModel(); // Setting DataContext in code-behind
        //}

        public StatusRequest(User currentUser)
        {
            //InitializeComponent(); // Ensure UI components are initialized
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
            _swapRequestRepository = new SwapRequestRepository();
            _currentUser = currentUser;
            this.DataContext = new StatusRequestViewModel(currentUser);

            LoadSwapRequests();
        }

        private void LoadSwapRequests()
        {
            // Load Sent Requests
            var sentRequests = _swapRequestRepository.GetSentRequests(_currentUser.Username);
            SentRequestsGrid.ItemsSource = sentRequests;

            // Load Received Requests with email and address info
            var receivedRequests = _swapRequestRepository.GetReceivedRequests(_currentUser.Username).Select(request =>
            {
                // Assuming you have logic to load requester email and address
                if (request.Requester != null)
                {
                    request.RequesterEmail = request.Requester.Email;
                    request.RequesterAddress = request.Requester.Address;
                }
                return request;
            }).ToList();
            ReceivedRequestsGrid.ItemsSource = receivedRequests;
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
        //public StatusRequest(User currentUser)
        //{
        //    _swapRequestRepository = new SwapRequestRepository();

        //    // Populate Sent Requests
        //    var sentRequests = _swapRequestRepository.GetSentRequests(currentUser.Username);
        //    SentRequestsGrid.ItemsSource = sentRequests;

        //    // Populate Received Requests with email and address info
        //    var receivedRequests = _swapRequestRepository.GetReceivedRequests(currentUser.Username).Select(request => {
        //        request.RequesterEmail = request.Requester.Email;
        //        request.RequesterAddress = request.Requester.Address;
        //        return request;
        //    }).ToList();
        //    ReceivedRequestsGrid.ItemsSource = receivedRequests;
        //}
    }
}