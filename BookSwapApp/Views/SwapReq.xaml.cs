using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookSwapApp.Views
{
    /// <summary>
    /// Interaction logic for SwapReq.xaml
    /// </summary>
    public partial class SwapReq : Page
    {
        public SwapReq()
        {
            // Error, dikomen dulu
            // InitializeComponent();
        }

        private void btnReqSwap_Click(object sender, RoutedEventArgs e)
        {
            // Ambil referensi MainWindow
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            // Navigasi ke halaman SwapRequest
            mainWindow.NavigateToPage(new StatusRequest());
        }
    }
}
