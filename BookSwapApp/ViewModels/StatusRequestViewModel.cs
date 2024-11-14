using System.Collections.ObjectModel;
using System.Windows.Input;
using BookSwapApp.Models;
using BookSwapApp.Repositories;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookSwapApp.ViewModels
{
    public class StatusRequestViewModel : INotifyPropertyChanged
    {
        private readonly SwapRequestRepository _swapRequestRepository;
        private readonly User _currentUser;

        // Observable collections for data binding
        private ObservableCollection<SwapRequest> _sentRequests;
        private ObservableCollection<SwapRequest> _receivedRequests;

        // Public properties for binding
        public ObservableCollection<SwapRequest> SentRequests
        {
            get => _sentRequests;
            set
            {
                _sentRequests = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SwapRequest> ReceivedRequests
        {
            get => _receivedRequests;
            set
            {
                _receivedRequests = value;
                OnPropertyChanged();
            }
        }

        // Parameterless constructor (required for XAML)
        public StatusRequestViewModel() { }

        // Constructor that accepts a User object
        public StatusRequestViewModel(User currentUser)
        {
            _currentUser = currentUser;
            _swapRequestRepository = new SwapRequestRepository();

            // Initialize collections
            SentRequests = new ObservableCollection<SwapRequest>();
            ReceivedRequests = new ObservableCollection<SwapRequest>();

            // Load data into observable collections
            LoadSwapRequests();
        }

        private void LoadSwapRequests()
        {
            if (_currentUser != null)
            {
                // Load Sent Requests
                var sentRequests = _swapRequestRepository.GetSentRequests(_currentUser.Username);
                SentRequests = new ObservableCollection<SwapRequest>(sentRequests);

                // Load Received Requests
                var receivedRequests = _swapRequestRepository.GetReceivedRequests(_currentUser.Username);
                ReceivedRequests = new ObservableCollection<SwapRequest>(receivedRequests);
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
                // Update the request status to 'Approved'
                bool isUpdated = _swapRequestRepository.UpdateSwapRequestStatus(request.Id, "Approved", request.Book.Id);
                if (isUpdated)
                {
                    // Remove the request from the list after approval
                    ReceivedRequests.Remove(request);
                }
            }
        }

        // Logic to handle denying a swap request
        private void DenyRequest(SwapRequest request)
        {
            if (request != null)
            {
                // Update the request status to 'Denied'
                bool isUpdated = _swapRequestRepository.UpdateSwapRequestStatus(request.Id, "Denied", request.Book.Id);
                if (isUpdated)
                {
                    // Remove the request from the list after denial
                    ReceivedRequests.Remove(request);
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
