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
            if (Database.UsersDB[name].userAccounts.ContainsKey(acc.BrojRacuna))
            {
                Console.WriteLine("Vec postoji racun sa unetim brojem!");
                return false;
            }
            else{
                acc.MasterCardProp.SubjectName = name;
                acc.MasterCardProp.Pin = acc.Pin;
                Database.UsersDB[name].userAccounts.Add(acc.BrojRacuna, acc);
                Console.WriteLine("Uspesno");
                return true;
            }
        }

        public bool IzdajKarticu()
        {
            throw new NotImplementedException();
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
