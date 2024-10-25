﻿using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;
using BookSwapApp.Services;

namespace BookSwapApp.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private NavigationService _navigationService;

        public Login()
        {
            InitializeComponent();
            _navigationService = new NavigationService(((MainWindow)Application.Current.MainWindow).MainFrame);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateTo(typeof(HomePage));
        }

        private void GoToSignupPage(object sender, ContextMenuEventArgs e)
        {
            _navigationService.NavigateTo(typeof(Signup));
        }

        private void GoBack(object sender, EventArgs e)
        {
            _navigationService.GoBack();
        }

        private void IconGoBack(object sender, RoutedEventArgs e)
        {
            _navigationService.GoBack();
        }
    }
}
