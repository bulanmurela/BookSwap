using System.Configuration;
using System.Data;
using System.Windows;
using BookSwapApp.Models;

namespace BookSwapApp
{
    public partial class App : Application
    {
        public User CurrentUser { get; set; }
    }

}
