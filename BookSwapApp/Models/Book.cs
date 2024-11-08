using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public string AvailabilityStatus { get; private set; }
        public bool VerificationStatus { get; private set; }
        public User Owner { get; private set; }

        public Book(string title, string author, string genre, string condition, string availabilityStatus, User owner)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Condition = condition;
            AvailabilityStatus = availabilityStatus;
            VerificationStatus = false;
            Owner = owner;
        }

        public string GetBookDetails()
        {
            return $"Title: {Title}, Author: {Author}, Genre: {Genre}, Condition: {Condition}, AvailabilityStatus: {AvailabilityStatus}";
        }

        public void SetCondition(string condition)
        {
            Condition = condition;
        }

        public void UpdateAvailabilityStatus(string status)
        {
            AvailabilityStatus = status;
        }

        public void Verify(Admin admin)
        {
            if (!VerificationStatus)
            {
                VerificationStatus = true;
                Console.WriteLine($"Buku '{Title}'sudah terverifikasi oleh admin: {admin.Username}");
            }
            else
            {
                Console.WriteLine("Buku sudah terverifikasi.");
            }
        }
    }
}