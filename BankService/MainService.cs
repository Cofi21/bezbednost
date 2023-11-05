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
        public void Message(bool message, User u)
        {
            if (message)
            {
                Console.WriteLine($"Korisnik je uspesno kreirao nalog broj: {u.Broj}.");
            }
            else
            {
                Console.WriteLine($"Nalog broj {u.Broj} vec postoji!");
            }
        }

    }
}
