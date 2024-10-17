using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap
{
    internal class Transaction
    {
        private string _transDate;
        private string _transID;
        private string _transStatus;

        public string TransactionDate
        {
            get { return _transDate; }
            set { _transDate = value; }
        }

        public string TransactionID
        {
            get { return _transID; }
            set { _transID = value; }
        }

        public string TransactionStatus
        {
            get { return _transStatus; }
            set { _transStatus = value; }
        }

        public bool completeTransaction(string transID)
        {
            if (!string.IsNullOrEmpty(transID))
            {
                _transID = transID;
                _transDate = DateTime.Now.ToString("dd/MM/yyyy"); // set tanggal sekarang (dinamis)
                _transStatus = "Complete";
                return true;
            }
            else
            { return  false; }
        }

        public string updateTransactionStatus(string newStatus)
        {
            _transStatus = newStatus;
            return _transStatus;
        }
    }
}
