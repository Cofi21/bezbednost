using Common;
using Common.Manager;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankService
{
    public class Replikator : IReplikator
    {
        public void Send(List<Account> accounts)
        {
            foreach(Account acc in accounts)
            {
                IMDatabase.AccountsDB[acc.BrojRacuna] = acc;
            }

            Console.WriteLine("Primljeni podaci o nalozima:");
            foreach (var acc in accounts)
            {
                Console.WriteLine($"Broj računa: {acc.BrojRacuna}, Stanje na računu: {acc.Stanje}");
            }
        }

        public List<Account> Collect()
        {
            List<Account> accountList = new List<Account>();

            foreach(Account acc in IMDatabase.AccountsDB.Values)
            {
                accountList.Add(acc);
            }

            return accountList;
        }

    }
}
