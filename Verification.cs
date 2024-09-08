using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap
{
    internal class Verification
    {
        private string _verifDate;
        private string _verifStatus;
        public string VerificationDate
        {
            get { return _verifDate; }
        }
        public string VerificationStatus
        {
            get { return _verifStatus; }
        }
        public void updateVerificationStatus(string verifStatus)
        {
            if (verifStatus == "DONE")
            {
                _verifDate = "Today";
                _verifStatus = "Completed";
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
