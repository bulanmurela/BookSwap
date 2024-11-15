using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;
using BookSwapApp.Repositories;
using BookSwapApp.Models;
using BookSwapApp.ViewModels;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BookSwapApp.Commands;
using System.Diagnostics;

namespace BookSwapApp.Views
{
    public partial class StatusRequest : Page
    {
        private NavigationService _navigationService;
        private readonly SwapRequestRepository _swapRequestRepository;
        private SwapRequest _swapRequest;
        private readonly User _currentUser;
        private StatusRequestViewModel _viewModel;
        public ICommand CombinedRequestsCommand { get; private set; }
        public SwapRequest SwapRequest { get => _swapRequest; set => _swapRequest = value; }

        public StatusRequest()
        {
            InitializeComponent();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
            _swapRequestRepository = new SwapRequestRepository();
            _currentUser = new User { Username = "defaultUser" }; // Default user (ganti sesuai kebutuhan
            _viewModel = new StatusRequestViewModel();
            DataContext = _viewModel;

            CombinedRequestsCommand = new RelayCommand(parameter => LoadCombinedRequests());


        }

        public StatusRequest(User currentUser, SwapRequest swapRequest)
        {
            InitializeComponent();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _swapRequestRepository = new SwapRequestRepository();
            _swapRequest = swapRequest;
            _viewModel = new StatusRequestViewModel(_currentUser, swapRequest);
            CombinedRequestsCommand = new RelayCommand(parameter => LoadCombinedRequests());
            DataContext = _viewModel;

            LoadCombinedRequests();
        }

        // kode di bawah ini sudah ada di ViewModel
        private void LoadCombinedRequests()
        {
            SwapRequestRepository swapRequestRepository = new SwapRequestRepository();
            // Load Sent Requests
            var sentRequests = _swapRequestRepository.GetSentRequests(_currentUser.Username).Select(request =>
            {
                request.RequestType = "Sent";  // Mark type as 'Sent'
                return request;
            }).ToList();

            // Load Received Requests
            var receivedRequests = _swapRequestRepository.GetReceivedRequests(_currentUser.Username).Select(request =>
            {
                request.RequestType = "Requested";  // Mark type as 'Requested'
                return request;
            }).ToList();

            // Combine both lists into one
            var combinedRequests = sentRequests.Concat(receivedRequests).ToList();

            // Set to ViewModel or directly to the ListView
            _viewModel.CombinedRequests = new ObservableCollection<SwapRequest>(combinedRequests);

            Debug.WriteLine($"Sent requests count: {sentRequests.Count}");

            StatusRequestsListView.ItemsSource = combinedRequests;
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