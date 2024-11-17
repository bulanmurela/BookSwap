using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using BookSwapApp.Models;

namespace BookSwapApp.Models
{
    public class Book
    {
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; private set; }
        public string Genre { get; private set; }
        public string Condition { get; private set; }
        public bool VerificationStatus { get; set; }

        [NotMapped] 
        public User Owner { get; internal set; }
        public string OwnerUsername { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerAddress { get; set; }


        private byte[] _coverImage;
        public byte[] CoverImage
        {
            get => _coverImage;
            set
            {
                _coverImage = value;
                SetCoverImageSource(_coverImage); 
            }
        }

        [NotMapped]
        public BitmapImage CoverImageSource { get; private set; }

        private BitmapImage ConvertToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;

            using (var ms = new MemoryStream(imageData))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze(); 
                return bitmap;
            }
        }

        public void SetCoverImageSource(byte[] coverImage)
        {
            CoverImageSource = ConvertToImage(coverImage);
        }

        public Book() { }
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

        public void AssignOwner(User user)
        {
            if (user != null && user.Username == OwnerUsername)
            {
                Owner = user;
            }
        }

        public void SetOwnerContactInfo(string email, string address)
        {
            OwnerEmail = email;
            OwnerAddress = address;
        }

    }
}