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
        public static int Odgovor { get; set; }
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

        // Premesteno ovde jer server treba da implementira ovakve funkcije, a i dodatno treba da izgenerise mastercard sertifikat pri izvrsenju dodavanja naloga.
        public void AddAccount(string username, string password, string broj, IMain factory)
        {
            try
            {
                if (Database.UserAccountsDB.ContainsKey(broj))
                {
                    bool message = false;
                    Console.WriteLine($"Nalog broj {broj} vec postoji!");
                    factory.Message(message, new User(username, password, broj));
                }
                else
                {
                    bool message = true;
                    Database.UserAccountsDB.Add(broj, new User(username, password, broj));
                    Console.WriteLine($"Korisnik je uspesno kreirao nalog broj: {broj}.");
                    factory.Message(message, new User(username, password, broj));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska! " + ex.Message +  "\n" +  ex.StackTrace);
            }
        }
    }
}
