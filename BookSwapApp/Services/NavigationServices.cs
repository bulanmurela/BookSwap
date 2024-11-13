using System;
using System.Windows;
using System.Windows.Controls;
using BookSwapApp.Views;

namespace BookSwapApp.Services
{
    public class NavigationService
    {
        // Private: hanya bisa diakses di dalam kelas ini
        private readonly Frame _frame;

        // Konstruktor publik yang menerima Frame sebagai parameter
        public NavigationService(Frame frame)
        {
            // menginisialisasi _frame
            _frame = frame;
        }

        // Protected Navigate yang menyediakan navigasi dasar
        // Bisa dipanggil di kelas turunan
        protected void Navigate(Page page)
        {
            _frame.Navigate(page);
        }

        // Public: untuk navigasi ke halaman tertentu tanpa atau dengan parameter
        public void NavigateTo(Type pageType, params object[] parameters)
        {
            if (typeof(Page).IsAssignableFrom(pageType))
            {
                var page = Activator.CreateInstance(pageType, parameters) as Page;
                if (page != null)
                {
                    Navigate(page);
                }
                else
                {
                    throw new InvalidOperationException("Page type must inherit from Page.");
                }
            }
            else if (typeof(Window).IsAssignableFrom(pageType))
            {
                // If pageType is a Window, create a new instance and show it as a window
                var window = Activator.CreateInstance(pageType, parameters) as Window;
                if (window != null)
                {
                    window.Show();
                }
                else
                {
                    throw new InvalidOperationException("Window type must inherit from Window.");
                }
            }
            else
            {
                throw new InvalidOperationException("Type must inherit from either Page or Window.");
            }
        }


        // Public: untuk ke halaman sebelumnya
        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }
    }
}
