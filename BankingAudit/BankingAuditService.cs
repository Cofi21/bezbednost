using Common;
using Common.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankingAudit
{
    public class BankingAuditService : IBankingAudit
    {
        public void AccessingLog(Audit audit)
        {
            string fileInput = string.Empty;
            fileInput += "\n---------------------------------------------------------------\n";
            fileInput += "Bank name: " + audit.BankName + "\n";
            fileInput += "Account name: " + audit.AccountName + "\n";
            fileInput += "Time of Detection: " + audit.TimeOfDetection.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            fileInput += "Transactions:";
            string numberOfTransactions;

            foreach (TransactionDetails td in audit.TransactionsList)
            {
                numberOfTransactions = Regex.Match(td.ToString(), @"\d+").Value;

                fileInput += "\n\n\tTransaction: " + td.TransactionOrderNumber;
                fileInput += "\n\tReceived Time: " + td.ReceivedDateTime.ToString("dd.MM.yyyy HH:mm:ss");
                fileInput += "\n\tAmmount: " + td.Svota;
            }

            fileInput += "\n---------------------------------------------------------------\n";

            using (StreamWriter sw = new StreamWriter("..\\..\\LogFile.txt", true))
            {
                sw.WriteLine(fileInput);
            }
        }
    }
}
