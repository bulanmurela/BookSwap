using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap
{
    internal class Book
    {
        private string _title;
        private string _author;
        private string _genre;
        private string _condition;
        private string _availabilityStatus;
        public string Title
        {
            get { return _title; }
        }
        public string Author
        {
            get { return _author; }
        }
        public string Genre
        {
            get { return _genre; }
        }
        public string Condition
        {
            get { return _condition; }
        }
        public string AvailabilityStatus
        {
            get { return _availabilityStatus; }
        }
        public Book(string title, string author, string genre, string condition, string availabilityStatus)
        {
            _title = title;
            _author = author;
            _genre = genre;
            _condition = condition;
            _availabilityStatus = availabilityStatus;
        }
        public void UpdateAvailabilityStatus(string status)
        {
            _availabilityStatus = status;
        }
        public string GetBookDetails()
        {
            return $"Title: {_title}, Author: {_author}, Genre: {_genre}, Condition: {_condition}, AvailabilityStatus: {_availabilityStatus}
        }
        public void SetCondition(string condition)
        {
            _condition = condition;
        }

    }
}
