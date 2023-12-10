using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Manager
{
    public class TransactionDetails
    {
        public int TransactionOrderNumber { get; set; }
        public DateTime ReceivedDateTime { get; set; }
        public double Svota { get; set; }
    }
}
