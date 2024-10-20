using System;
using System.Windows.Controls;

namespace BookSwapApp.Services
{
    public class NavigationService
    {
        private readonly Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo(Type pageType)
        {
            _frame.Navigate(Activator.CreateInstance(pageType));
        }

        public void NavigateTo(Type pageType, params object[] parameters)
        {
            var page = Activator.CreateInstance(pageType, parameters) as Page;
            _frame.Navigate(page);
        }

        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }
    }
}
