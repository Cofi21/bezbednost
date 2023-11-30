using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace BankService
{
    public class WinService : IWin
    {
        public void TestCommunication()
        {
            Console.WriteLine("Communication established.");
        }

        public bool KreirajNalog(Account acc)
        {
            string name = WindowsIdentity.GetCurrent().Name;
            if (Database.UsersDB[name].UserAccounts.ContainsKey(acc.BrojRacuna))
            {
                Console.WriteLine("Vec postoji racun sa unetim brojem!");
                return false;
            }
            else{
                Database.UsersDB[name].UserAccounts.Add(acc.BrojRacuna, acc);
                Database.AllUserAccountsDB.Add(acc.BrojRacuna, acc);
                IzdajKarticu(acc, name);
                Console.WriteLine("Uspesno");
                return true;
            }
        }

        public void IzdajKarticu(Account acc, string username)
        {
            // MasterCard parametri
            acc.MasterCardProp.SubjectName = username;
            acc.MasterCardProp.Pin = acc.Pin;
        }

        public bool PovuciSertifikat()
        {
            throw new NotImplementedException();
        }

        public bool ResetujPinKod()
        {
            throw new NotImplementedException();
        }


    }
}
