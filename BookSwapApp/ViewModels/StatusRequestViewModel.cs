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
        public ObservableCollection<SwapRequest> CombinedRequests { get; set; }
        public ICommand CombinedRequestsCommand { get; set; }
        public ICommand CompleteCommand { get; set; }
        public ICommand ApproveCommand { get; set; }
        public ICommand DenyCommand { get; set; }

        // Parameterless constructor (required for XAML)
        public StatusRequestViewModel()
        {
            _currentUser = new User { Username = "defaultUser" }; // Mock user
            _swapRequestRepository = new SwapRequestRepository();

            CombinedRequests = new ObservableCollection<SwapRequest>();

            CompleteCommand = new RelayCommand<SwapRequest>(CompleteRequest);
            ApproveCommand = new RelayCommand<SwapRequest>(ApproveRequest);
            DenyCommand = new RelayCommand<SwapRequest>(DenyRequest);

            LoadCombinedRequests();
        }

        // Constructor that accepts a User object
        public StatusRequestViewModel(User currentUser)
        {
            _currentUser = currentUser;
            _swapRequestRepository = new SwapRequestRepository();

            CombinedRequests = new ObservableCollection<SwapRequest>();

            CompleteCommand = new RelayCommand<SwapRequest>(CompleteRequest);
            ApproveCommand = new RelayCommand<SwapRequest>(ApproveRequest);
            DenyCommand = new RelayCommand<SwapRequest>(DenyRequest);

            LoadCombinedRequests();
        }

        private void LoadCombinedRequests()
        {
            if(_currentUser != null)
            {
                // Ambil sent requests
                var sentRequests = _swapRequestRepository.GetSentRequests(_currentUser.Username);
                foreach (var request in sentRequests)
                {
                    request.RequestType = "Sent";  // Menandai tipe request
                    CombinedRequests.Add(request);
                }

                // Ambil received requests
                var receivedRequests = _swapRequestRepository.GetReceivedRequests(_currentUser.Username);
                foreach (var request in receivedRequests)
                {
                    request.RequestType = "Requested";  // Menandai tipe request
                    CombinedRequests.Add(request);
                }

                OnPropertyChanged(nameof(CombinedRequests)); // Update UI jika data berubah
            }
        }
        private void CompleteRequest(SwapRequest request)
        {
            if (request != null)
            {
                try
                {
                    bool isUpdated = _swapRequestRepository.UpdateSwapRequestStatus(request.Id, "Completed", request.Book.Id);
                    if (isUpdated)
                    {
                        MessageBox.Show("Request marked as Completed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCombinedRequests(); // Reload setelah update
                    }
                    else
                    {
                        MessageBox.Show("Failed to complete the request.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while completing the request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Logika untuk menerima (Approve) request
        private void ApproveRequest(SwapRequest request)
        {
            if (request != null)
            {
                try
                {
                    bool isUpdated = _swapRequestRepository.UpdateSwapRequestStatus(request.Id, "Approved", request.Book.Id);
                    if (isUpdated)
                    {
                        MessageBox.Show("Request approved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCombinedRequests(); // Reload setelah update
                    }
                    else
                    {
                        MessageBox.Show("Failed to approve the request.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while approving the request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Logika untuk menolak (Deny) request
        private void DenyRequest(SwapRequest request)
        {
            if (request != null)
            {
                try
                {
                    bool isUpdated = _swapRequestRepository.UpdateSwapRequestStatus(request.Id, "Denied", request.Book.Id);
                    if (isUpdated)
                    {
                        MessageBox.Show("Request denied successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCombinedRequests(); // Reload setelah update
                    }
                    else
                    {
                        MessageBox.Show("Failed to deny the request.", "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while denying the request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // INotifyPropertyChanged implementation untuk menginformasikan perubahan properti ke UI
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Implementasi RelayCommand untuk ICommand
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
