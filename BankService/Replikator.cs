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
        public void Posalji(List<Account> accounts)
        {
            foreach(Account acc in accounts)
            {
                IMDatabase.AccountsDB[acc.BrojRacuna] = acc;
            }
        }

        public List<Account> Preuzmi()
        {
            Dictionary<string, Account> accounts = Json.LoadAccountsFromFile();
            List<Account> accountList = new List<Account>();

            foreach(Account acc in accounts.Values)
            {
                accountList.Add(acc);
            }

            return accountList;
        }

    }
}
