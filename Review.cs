using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap
{
    internal class Review
    {
        private string ReviewDate { get; set; }
        private string Comment { get; set; }
        private int Rating { get; set; }

        public Review(string reviewDate, string comment, int rating)
        {
            ReviewDate = reviewDate;
            Comment = comment;
            Rating = rating;
        }
    }
}
