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
    /// TREBA RESITI OVAJ ISPIS NA SERVERU NE RADI, --> na serveru nastavak komentara
    public class MainService : IMain, ICert
    {
        public static int Odgovor;
        public int Connection()
        {
            Console.WriteLine("Uspesan sertifikat");
            Odgovor = 1;
            return 1;
        }

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
