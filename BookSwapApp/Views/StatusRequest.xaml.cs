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
            _currentUser = ((App)Application.Current).CurrentUser; 
            _viewModel = new StatusRequestViewModel();
            this.DataContext = _viewModel;

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
            this.DataContext = _viewModel;

            LoadCombinedRequests();
        }

        
        private void LoadCombinedRequests()
        {
            SwapRequestRepository swapRequestRepository = new SwapRequestRepository();

            // Load Sent Requests
            var sentRequests = _swapRequestRepository.GetSentRequests(_currentUser.Username).Select(request =>
            {
                request.RequestType = "Sent";  // Mark type as 'Sent'
                request.IsCompleteVisible = (request.RequestType == "Sent" && request.Status == "Approved") ? Visibility.Visible : Visibility.Collapsed;
                request.IsApproveVisible = Visibility.Collapsed;
                request.IsDenyVisible = Visibility.Collapsed;
                return request;
            }).ToList();

            // Load Received Requests
            var receivedRequests = _swapRequestRepository.GetReceivedRequests(_currentUser.Username).Select(request =>
            {
                request.RequestType = "Requested";  // Mark type as 'Requested'
                request.IsApproveVisible = (request.RequestType == "Requested") ? Visibility.Visible : Visibility.Collapsed;
                request.IsDenyVisible = (request.RequestType == "Requested") ? Visibility.Visible : Visibility.Collapsed;
                request.IsCompleteVisible = Visibility.Collapsed;
                return request;
            }).ToList();

            // Combine both lists into one
            var combinedRequests = sentRequests.Concat(receivedRequests).ToList();

            // Set to ViewModel or directly to the ListView
            _viewModel.CombinedRequests = new ObservableCollection<SwapRequest>(combinedRequests);

            Debug.WriteLine($"Sent requests count: {sentRequests.Count}");
            Debug.WriteLine($"Received requests count: {receivedRequests.Count}");

            StatusRequestsListView.ItemsSource = _viewModel.CombinedRequests;
        }

        private void GoToProfile(object sender, RoutedEventArgs e)
        {
            // Navigate to the Profile page
            _navigationService.NavigateTo(typeof(Profil));
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(HomePage));
        }

    }
}