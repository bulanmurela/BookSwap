using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookSwapApp.Views;

namespace BookSwapApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // halaman default saat aplikasi dibuka
            // MainFrame.Navigate(new Views.HomePage());
        }

        // navigasi untuk berpindah halaman
        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }

        private void btnToHomePage_Click(object sender, RoutedEventArgs e)
        {
            // Navigasi ke halaman HomePage
            MainFrame.Navigate(new Views.HomePage());
        }
    }

}