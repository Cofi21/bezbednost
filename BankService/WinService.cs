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

        public bool KreirajNalog(User u)
        {
            if (Database.UserAccountsDB.ContainsKey(u.Broj))
            {
                Console.WriteLine("Vec postoji racun sa unetim brojem!");
                return false;
            }
            else{
                Database.UserAccountsDB.Add(u.Broj, u);
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
