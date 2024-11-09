using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using BookSwapApp.Services;
using BookSwapApp.Helpers;
using BookSwapApp.Models;
using BookSwapApp.Repositories;

namespace BookSwapApp.Views
{
    public partial class UploadBook : Window
    {
        private string coverFilePath;
        private NavigationService _navigationService;

        public UploadBook()
        {
            InitializeComponent();
        }

        private void btnUploadCover_Click(object sender, RoutedEventArgs e)
        {
            //// Open a file dialog to select an image for the book cover
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            //if (openFileDialog.ShowDialog() == true)
            //{
            //    // Store the file path and display the file name
            //    coverFilePath = openFileDialog.FileName;
            //    btnUploadCover.Content = System.IO.Path.GetFileName(coverFilePath);
            //}
        }

        //private void btnSubmit_Click(object sender, RoutedEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtTitle.Text) || string.IsNullOrEmpty(txtAuthor.Text) ||
        //        string.IsNullOrEmpty(txtGenre.Text) || string.IsNullOrEmpty(txtCondition.Text) ||
        //        string.IsNullOrEmpty(coverFilePath))
        //    {
        //        MessageBox.Show("Please fill in all fields and upload a cover image.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }

        //    // Assuming a method SaveBookData exists to save data to the database
        //    bool isSaved = SaveBookData(txtTitle.Text, txtAuthor.Text, txtGenre.Text, txtCondition.Text, coverFilePath);

        //    if (isSaved)
        //    {
        //        MessageBox.Show("Data has saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        //        // Navigate back to the homepage
        //        _navigationService.NavigateTo(typeof(HomePage));
        //    }
        //    else
        //    {
        //        MessageBox.Show("Failed to save data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private bool SaveBookData(string title, string author, string genre, string condition, string availabilityStatus, User user)
        //{
        //    try
        //    {
        //        // Create a new Book object
        //        Book book = new Book(title, author, genre, condition, availabilityStatus, user);

        //        // Initialize the BookRepository
        //        BookRepository bookRepository = new BookRepository();

        //        // Call the UploadBook method in the repository
        //        bool isUploaded = bookRepository.UploadBook(user, book);

        //        if (isUploaded)
        //        {
        //            MessageBox.Show("Data has been saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //            return true;
        //        }
        //        else
        //        {
        //            MessageBox.Show("Failed to save data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error saving data: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }
        //}

    }
}
