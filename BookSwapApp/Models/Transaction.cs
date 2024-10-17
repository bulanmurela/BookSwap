using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace BookSwapApp
{
    internal class Transaction
    {
        public string TransactionDate { get; set; }
        public string TransactionID { get; set; }
        public string TransactionStatus { get; set; }

        public bool CompleteTransaction(string transID)
        {
            if (!string.IsNullOrEmpty(transID))
            {
                TransactionID = transID;
                TransactionDate = DateTime.Now.ToString("dd/MM/yyyy"); // set tanggal sekarang (dinamis)
                TransactionStatus = "Complete";
                return true;
            }
            else
            {
                return false;
            }
        }

        public string UpdateTransactionStatus(string newStatus)
        {
            TransactionStatus = newStatus;
            return TransactionStatus;
        }
    }
}