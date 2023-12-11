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
            string fileInput = "Bank name: " + audit.BankName + "\n";
            fileInput += "Account name: " + audit.AccountName + "\n";
            fileInput += "Time of Detection: " + audit.TimeOfDetection + "\n";
            string numberOfTransactions;

            foreach (TransactionDetails td in audit.TransactionsList)
            {
                numberOfTransactions = Regex.Match(td.ToString(), @"\d+").Value;

                fileInput += "Broj transakcija: " + numberOfTransactions + "\n";
                fileInput += "---------------------------------------\n";
            }

            Console.WriteLine(fileInput);

            string path = "..\\..\\LogFile.txt";

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(fileInput);
            }

        }
    }
}
