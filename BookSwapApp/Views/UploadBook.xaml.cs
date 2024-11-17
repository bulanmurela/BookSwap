using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using BookSwapApp.Services;
using BookSwapApp.Helpers;
using BookSwapApp.Models;
using BookSwapApp.Repositories;
using System.Windows.Media;

namespace BookSwapApp.Views
{
    public partial class UploadBook : Window
    {
        private string coverFilePath;
        private NavigationService _navigationService;
        private readonly User currentUser;

        public UploadBook()
        {
            InitializeComponent();
        }

        public UploadBook(User user, NavigationService navigationService) : this()
        {
            currentUser = user;
            _navigationService = navigationService;
        }

        private void btnUploadCover_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == true)
            {
                coverFilePath = openFileDialog.FileName;
                btnUploadCover.Content = System.IO.Path.GetFileName(coverFilePath);
            }
        }
        private byte[] ConvertImageToByteArray(string imagePath)
        {
            byte[] imageBytes = null;
            try
            {
                imageBytes = File.ReadAllBytes(imagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading image file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return imageBytes;
        }

        private bool SaveBookData(string title, string author, string genre, string condition, User user, string coverFilePath)
        {
            if (user == null)
            {
                MessageBox.Show("User information is missing. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                byte[] coverImageBytes = ConvertImageToByteArray(coverFilePath);
                Book book = new Book(title, author, genre, condition, user);

                BookRepository bookRepository = new BookRepository();

                return bookRepository.UploadBook(currentUser, book, coverImageBytes);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private string SaveImageToServer(string sourceFilePath)
        {
            string destinationFolder = @"C:\Server\BookCovers"; // Ubah path sesuai kebutuhan
            string fileName = Path.GetFileName(sourceFilePath);
            string destinationPath = Path.Combine(destinationFolder, fileName);

            try
            {
                File.Copy(sourceFilePath, destinationPath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving image: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return destinationPath;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text) || string.IsNullOrEmpty(txtAuthor.Text) ||
                string.IsNullOrEmpty(txtGenre.Text) || string.IsNullOrEmpty(txtCondition.Text))
            {
                MessageBox.Show("Please fill in all fields and upload a cover image.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool isSaved = SaveBookData(txtTitle.Text, txtAuthor.Text, txtGenre.Text, txtCondition.Text, currentUser, coverFilePath);

            if (isSaved)
            {

                MessageBox.Show($"The book '{txtTitle.Text}' has been successfully uploaded and is awaiting verification by the admin.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.NavigateTo(typeof(HomePage));
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to save data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.Text == "Book Title" || textBox.Text == "Author" || textBox.Text == "Genre" || textBox.Text == "Condition")
                {
                    textBox.Text = "";
                    textBox.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox.Name == "txtTitle")
                        textBox.Text = "Title";
                    else if (textBox.Name == "txtAuthor")
                        textBox.Text = "Author";
                    else if (textBox.Name == "txtGenre")
                        textBox.Text = "Genre";
                    else if (textBox.Name == "txtCondition")
                        textBox.Text = "Condition";

                    textBox.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }
    }
}