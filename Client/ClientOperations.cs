using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientOperations
    {
        // IZBACUJE GRESKU DA SVE MORA DA BUDE STATIC, MORAMO RESITI TO
        public static void Opcije(int broj, IMain factory, ICert certFactory)
        {
            var clientOperations = new ClientOperations();
            switch (broj)
            {
                case 1:
                    clientOperations.KreirajNalog(factory);
                    break;
                case 2:
                    clientOperations.IzdajKarticu(certFactory);
                    break;
                case 3:
                    PovuciSertifikat();
                    break;
                case 4:
                    IzvrsiTransakciju();
                    break;
                case 5:
                    ResetujPinKod();
                    break;
                case 0:
                    return;
            }
        }
        // metoda za koriscenje * umesto karaktera kod unosa PIN - a
        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Remove(password.Length - 1);
                        Console.Write("\b \b"); // Erase the character from the console
                    }
                }
                else
                {
                    password += key.KeyChar;
                    Console.Write("*"); // Display asterisks instead of the actual characters
                }
            }

            return password;
        }
        public  void KreirajNalog(IMain factory)
        {

            string username = WindowsIdentity.GetCurrent().Name;
            Console.Write("Unesite broj naloga: ");
            string broj = Console.ReadLine();
            Console.Write("Unesite PIN: ");
            string pin = ReadPassword();
            Console.Write("Potvrdite PIN: ");
            string pinPotvrda = ReadPassword();

            if (pin.Equals(pinPotvrda))
            {

                AddAccount(username, pin, broj, factory);
            }
            else
            {
                Console.WriteLine("Greska! Uneti PIN kodovi se ne poklapaju.");
            }
        }
        public static void AddAccount(string username, string password, string broj, IMain factory)
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
                    factory.Message(message, new User(username, password, broj));
                    Console.WriteLine($"Korisnik je uspesno kreirao nalog broj: {broj}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska! " + ex.Message);
            }
        }

        public static void PovuciSertifikat() { }
        public  void IzdajKarticu( ICert certFactory)
        {
            certFactory.Connection();
            Console.WriteLine("Kartica je uspesno izdata.");
        }
        public static void IzvrsiTransakciju()
        {

        }
        public static void ResetujPinKod() { }

    }
}
