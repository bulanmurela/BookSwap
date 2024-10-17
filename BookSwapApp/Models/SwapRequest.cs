using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace BookSwapApp
{
    internal class SwapRequest
    {
        public string RequestDate { get; set; }
        public string RequestID { get; set; }
        public string Request { get; set; }
        public string Status { get; set; }

        public bool CreateRequest(string req)
        {
            if (req == "Bertukar")
            {
                Request = req;
                RequestDate = DateTime.Now.ToString("dd/MM/yyyy"); // tanggal dinamis
                RequestID = Guid.NewGuid().ToString(); // unique req ID dinamis
                Status = "Requested";
                return true;
            }
            else
            { return false; }
        }

        public string UpdateStatus(string status)
        {
            Status = status;
            return Status;
        }
    }
}