using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using BookSwapApp.Models;

namespace BookSwapApp
{
    public class Book
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Genre { get; private set; }
        public string Condition { get; private set; }
        public bool VerificationStatus { get; set; }
        public User Owner { get; private set; }
        public byte[] CoverImage { get;  set; }

        [NotMapped]
        public BitmapImage CoverImageSource { get; set; }
        public Book(string title, string author, string genre, string condition, User owner)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Condition = condition;
            VerificationStatus = false;
            Owner = owner;
        }

        public string GetBookDetails()
        {
            return $"Title: {Title}, Author: {Author}, Genre: {Genre}, Condition: {Condition}";
        }

        public void SetCondition(string condition)
        {
            Condition = condition;
        }

        public void MarkAsVerified()
        {
            VerificationStatus = true;
        }

        public void Verify(Admin admin)
        {
            if (!VerificationStatus)
            {
                MarkAsVerified();
            }
        }
    }
}