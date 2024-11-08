using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace BookSwapApp
{
    public class Review
    {
        private string ReviewDate { get; set; }
        private string Comment { get; set; }
        private int Rating { get; set; }

        public Review(string comment, int rating)
        {
            ReviewDate = DateTime.Now.ToString("yyyy-MM-dd");
            Comment = comment;
            Rating = rating;
        }
    }
}