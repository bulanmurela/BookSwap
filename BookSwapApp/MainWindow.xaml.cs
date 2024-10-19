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
            // MainFrame.Navigate(new HomePage());
        }

        // contoh untuk navigasi dari satu halaman ke halaman lain dengan button (tombol)
        //private void BtnProfile_Click(object sender, RoutedEventArgs e)
        //{
        //    // Navigasi ke halaman profil
        //    MainFrame.Navigate(new ProfilePage());
        //}

    }

}