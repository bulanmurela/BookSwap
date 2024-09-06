using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap
{
    internal class SwapRequest
    {
        private string _requestDate;
        private string _requestID;
        private string _req;
        private string _status;

        public string RequestDate
        {
            get { return _requestDate; }
            set { _requestDate = value; }
        }

        public string RequestID
        {
            get { return _requestID; }
            set { _requestID = value; }
        }

        public string Request
        {
            get { return _req; }
            set { _req = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public bool createRequest(string req)
        {
            if (req == "Bertukar")
            {
                _req = req;
                _requestDate = DateTime.Now.ToString("dd/MM/yyyy"); // tanggal dinamis
                _requestID = Guid.NewGuid().ToString(); // unique req ID dinamis
                _status = "Requested";
                return true;
            }
            else
            { return false; }
        }

        public string updateStatus(string status)
        {
            _status = status;
            return _status;
        }
    }
}
