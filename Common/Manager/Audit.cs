using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Manager
{
    public class Audit
    {
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public DateTime TimeOfDetection { get; set; }
        public List<TransactionDetails> TransactionsList { get; set; }

    }
}
