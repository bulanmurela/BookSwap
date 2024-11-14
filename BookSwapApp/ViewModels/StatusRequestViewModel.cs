using System.Collections.ObjectModel;
using System.Windows.Input;
using BookSwapApp.Models;
using BookSwapApp.Repositories;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BookSwapApp.ViewModels
{
    public class StatusRequestViewModel : INotifyPropertyChanged
    {
        private readonly SwapRequestRepository _swapRequestRepository;
        private readonly User _currentUser;

        // Observable collections for data binding
        public ObservableCollection<SwapRequest> _sentRequests { get; set; }
        public ObservableCollection<SwapRequest> _receivedRequests { get; set; }

        // Public properties for binding
        public ObservableCollection<SwapRequest> SentRequests
        {
            get => _sentRequests;
            set
            {
                if (_sentRequests != value)
                {
                    _sentRequests = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<SwapRequest> ReceivedRequests
        {
            get => _receivedRequests;
            set
            {
                if (_receivedRequests != value)
                {
                    _receivedRequests = value;
                    OnPropertyChanged();
                }
            }
        }

        // Parameterless constructor (required for XAML)
        public StatusRequestViewModel()
        {
            // Initialize with mock user or handle this case properly
            _currentUser = new User { Username = "defaultUser" }; // For testing
            _swapRequestRepository = new SwapRequestRepository();

            SentRequests = new ObservableCollection<SwapRequest>();
            ReceivedRequests = new ObservableCollection<SwapRequest>();

            LoadSwapRequests();
        }

        // Constructor that accepts a User object
        public StatusRequestViewModel(User currentUser)
        {
            _currentUser = currentUser;
            _swapRequestRepository = new SwapRequestRepository();

            // Initialize collections
            _sentRequests = new ObservableCollection<SwapRequest>();
            _receivedRequests = new ObservableCollection<SwapRequest>();

            // Load data into observable collections
            LoadSwapRequests();
        }

        private void LoadSwapRequests()
        {
            if (_currentUser != null)
            {
                var sentRequests = _swapRequestRepository.GetSentRequests(_currentUser.Username);
                SentRequests.Clear();
                foreach (var request in sentRequests)
                    SentRequests.Add(request);

                var receivedRequests = _swapRequestRepository.GetReceivedRequests(_currentUser.Username);
                ReceivedRequests.Clear();
                foreach (var request in receivedRequests)
                    ReceivedRequests.Add(request);
            }
        }



        // Command to accept a swap request
        public ICommand AcceptCommand => new RelayCommand<SwapRequest>(AcceptRequest);

        // Command to deny a swap request
        public ICommand DenyCommand => new RelayCommand<SwapRequest>(DenyRequest);

        // Logic to handle accepting a swap request
        private void AcceptRequest(SwapRequest request)
        {
            if (request != null)
            {
                try
                {
                    // Update the request status to 'Approved'
                    bool isUpdated = _swapRequestRepository.UpdateSwapRequestStatus(request.Id, "Approved", request.Book.Id);
                    if (isUpdated)
                    {
                        LoadSwapRequests(); // Reload the requests after updating
                    }
                    else
                    {
                        // Notifikasi jika gagal
                        MessageBox.Show("Failed to update the request status to Approved.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while accepting the request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Logic to handle denying a swap request
        private void DenyRequest(SwapRequest request)
        {
            if (request != null)
            {
                try
                {
                    // Update the request status to 'Denied'
                    bool isUpdated = _swapRequestRepository.UpdateSwapRequestStatus(request.Id, "Denied", request.Book.Id);
                    if (isUpdated)
                    {
                        LoadSwapRequests(); // Reload the requests after updating
                    }
                    else
                    {
                        // Notifikasi jika gagal
                        MessageBox.Show("Failed to update the request status to Approved.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while accepting the request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // INotifyPropertyChanged implementation for data binding
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Basic implementation of a RelayCommand class for ICommand
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
