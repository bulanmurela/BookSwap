using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace BookSwapApp
{
    internal class Book
    {
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Genre { get; private set; }
        public string Condition { get; private set; }
        public string AvailabilityStatus { get; private set; }

        public Book(string title, string author, string genre, string condition, string availabilityStatus)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Condition = condition;
            AvailabilityStatus = availabilityStatus;
        }

        public void UpdateAvailabilityStatus(string status)
        {
            AvailabilityStatus = status;
        }

        public string GetBookDetails()
        {
            return $"Title: {Title}, Author: {Author}, Genre: {Genre}, Condition: {Condition}, AvailabilityStatus: {AvailabilityStatus}";
        }

        public void SetCondition(string condition)
        {
            Condition = condition;
        }
    }
}