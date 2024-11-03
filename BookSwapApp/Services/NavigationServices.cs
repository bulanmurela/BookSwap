using System;
using System.Windows.Controls;

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
