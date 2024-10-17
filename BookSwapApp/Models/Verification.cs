using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace BookSwapApp
{
    internal class Verification
    {
        public string VerificationDate { get; private set; }
        public string VerificationStatus { get; private set; }

        public bool UpdateVerificationStatus(string verifStatus)
        {
            if (verifStatus == "DONE")
            {
                VerificationDate = DateTime.Now.ToString("dd/MM/yyyy");
                VerificationStatus = "Completed";
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}