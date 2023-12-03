using Common;
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
                IMDatabase.AllUserAccountsDB[acc.BrojRacuna] = acc;
            }
        }

        public List<Account> Preuzmi(DateTime vremeReplikacije)
        {
            List<Account> accounts = new List<Account>();

            foreach(Account acc in IMDatabase.AllUserAccountsDB.Values)
            {
                if(acc.VremePoslednjeIzmene >= vremeReplikacije)
                {
                    accounts.Add(acc);
                }
            }

            return accounts;
        }

        public void Ispis()
        {
            foreach(Account acc in IMDatabase.AllUserAccountsDB.Values)
            {
                Console.WriteLine(acc + "\n");
            }
        }
    }
}
