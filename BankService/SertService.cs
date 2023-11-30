using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankService
{
    public class SertService : ICert
    {
        public void TestCommunication()
        {
            Console.WriteLine("Communication established.");
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

        public bool IzvrsiTransakciju(int opcija, string brojRacuna, double svota)
        {
            string name = WindowsIdentity.GetCurrent().Name;
            if (opcija == 1)
            {
                if (Database.AllUserAccountsDB.ContainsKey(brojRacuna))
                {
                    Database.AllUserAccountsDB[brojRacuna].Stanje += svota;
                    Console.WriteLine($"Uspesna uplata!");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Neuspesna uplata! Ne postoji racun sa brojem {brojRacuna}");
                    return false;
                }
            }
            else if(opcija == 2)
            {
                if (Database.AllUserAccountsDB.ContainsKey(brojRacuna))
                {
                    double trenutnoStanje = Database.AllUserAccountsDB[brojRacuna].Stanje;

                    if(trenutnoStanje < svota)
                    {
                        Console.WriteLine($"Na racunu nemate dovoljno sretstava za isplatu!");
                        return false;
                    }
                    Database.AllUserAccountsDB[brojRacuna].Stanje -= svota;
                    Console.WriteLine($"Uspesna isplata!");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Neuspesna uplata!Ne postoji racun sa brojem {brojRacuna}");
                    return false;
                }

            }
            return false;
        }
    }
}
