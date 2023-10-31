using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankService
{
    
    public class MainService : IMain
    {
        public static Dictionary<string, User> UserAccountsDB = new Dictionary<string, User>();

        public void AddUser(string username, string password, string ime, string prezime)
        {
            if (UserAccountsDB.ContainsKey(username))
            {
                Console.WriteLine($"Korisnik {username} vec postoji");
            }
            else
            {
                UserAccountsDB.Add(username, new User(username, password, ime, prezime));
                Console.WriteLine($"Korisnik {username} je uspesno dodat!");
            }
        }
    }
}
