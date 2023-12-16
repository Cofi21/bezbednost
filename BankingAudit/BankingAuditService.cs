using Common;
using Common.Manager;
using Common.Models;
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
        // AccessingLog treba da primi parametar objekta
        public void AccessingLog(TransactionPayments tp)
        {
            Console.WriteLine("Izvrsenje...");
            string fileInput = string.Empty;
            fileInput += "\n---------------------------------------------------------------\n";
            fileInput += "Bank name: " + tp.BankName + "\n";
            fileInput += "Account name: " + tp.AccountName + "\n";
            fileInput += "Time of Detection: " + tp.TimeOfDetection.ToString("dd.MM.yyyy HH:mm:ss") + "\n";
            fileInput += "Transactions:";
            string numberOfTransactions;

            foreach (TransactionDetails td in tp.TransactionsList)
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
                Console.WriteLine(fileInput);
            }
        }
    }
}
