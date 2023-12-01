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
            if(!IMDatabase.UsersDB.ContainsKey(name))
            {
                IMDatabase.UsersDB.Add(name, new User(name));
            }
            Console.WriteLine("Nalog kreiran");
            try
            {
                if (IMDatabase.AllUserAccountsDB.ContainsKey(acc.BrojRacuna))
                {
                    Console.WriteLine("Vec postoji racun sa unetim brojem!");
                    return false;
                }
                else
                {
                    MasterCard mc = new MasterCard(name, acc.Pin);
                    acc.MasterCards.Add(mc);

                    IMDatabase.UsersDB[name].UserAccounts.Add(acc.BrojRacuna, acc);
                    IMDatabase.AllUserAccountsDB.Add(acc.BrojRacuna, acc);

                    //Ispis radi provere
                    foreach(Account ac in IMDatabase.AllUserAccountsDB.Values)
                    {
                        Console.WriteLine(ac.ToString());
                    }
                    Console.WriteLine("Uspesno");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        public bool PovuciSertifikat()
        {
            throw new NotImplementedException();
        }

        public bool ResetujPinKod(string pin, string brojNaloga)
        {
            if (IMDatabase.AllUserAccountsDB.ContainsKey(brojNaloga.Trim()))
            {
                IMDatabase.AllUserAccountsDB[brojNaloga].Pin = pin;
                foreach(MasterCard mc in IMDatabase.AllUserAccountsDB[brojNaloga].MasterCards)
                {
                    if(mc.SubjectName.Equals(WindowsIdentity.GetCurrent().Name))
                    {
                        mc.Pin = pin;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }

        }

        public Dictionary<string, Account> ReadDict()
        {
            return IMDatabase.AllUserAccountsDB;
        }

        public Dictionary<string, User> ReadDictUsers()
        {
            return IMDatabase.UsersDB;
        }
    }
}
