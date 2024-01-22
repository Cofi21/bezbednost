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
using System.Xml.Linq;

namespace BankingAudit
{
    public class BankingAuditService : IBankingAudit
    {
        // AccessingLog treba da primi parametar objekta
        public void AccessingLog(TransactionPayments tp)
        {
            Console.WriteLine("Izvrsenje...");
            int index = tp.TransactionsList.Count;
            if (index > 0)
            {
                index -= 1;
            }
            try
            {
                if (tp.TransactionsList == null)
                {
                    Audit.BankingAuditSuccess(tp.BankName, tp.AccountName, tp.TimeOfDetection, 0);
                }
                else
                {
                    Audit.BankingAuditSuccess(tp.BankName, tp.AccountName, tp.TimeOfDetection, tp.TransactionsList[index].Svota);
                }
            }
            catch (Exception e)
            {
                Audit.BankingAuditFailed(tp.BankName, tp.AccountName, tp.TimeOfDetection, 0);
                Console.WriteLine("BankingAudit failed with an error: " + e.Message);
            }
        }
    }
}
