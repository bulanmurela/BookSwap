using System.Configuration;
using System.Data;
using System.Windows;
using BookSwapApp.Models;

namespace BookSwapApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public User CurrentUser { get; set; }
    }

}
