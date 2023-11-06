using Common;
using Common.Manager;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientProxy : ChannelFactory<IMain>, IMain, IDisposable
    {
        IMain factory;
        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            factory = this.CreateChannel();
        }

        public void Ispis()
        {
            Console.WriteLine("Izaberite zahtev: ");
            Console.WriteLine("\t1 - Kreiranje naloga");
            Console.WriteLine("\t2 - Povlacenje sertifikata ");
            Console.WriteLine("\t3 - Izvrsenje transakcije");
            Console.WriteLine("\t4 - Reset pin koda");
            Console.WriteLine("\t0 - Izlaz");
            Console.Write("Unesite Vas izbor: ");
            int broj = Int32.Parse(Console.ReadLine());
            Opcije(broj);
            
        }
        public void Opcije(int broj)
        {
            switch (broj)
            {
                case 1:
                    KreirajNalog();
                    break;
                case 2:
                    PovuciSertifikat();
                    break;
                case 3:
                    IzvrsiTransakciju();
                    break;
                case 4:
                    ResetujPinKod();
                    break;
                case 0:
                    return;
            }
        }
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
        public void KreirajNalog()
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

                AddAccount(username, pin, broj);
                Ispis();
            }
            else
            {
                Console.WriteLine("Greska! Uneti PIN kodovi se ne poklapaju.");
                Ispis();
            }
        }
        public void AddAccount(string username, string password, string broj)
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
            }catch(Exception ex)
            {
                Console.WriteLine("Greska! " + ex.Message);
            }
        }
        
        public void PovuciSertifikat() { }
        public void IzvrsiTransakciju() { }
        public void ResetujPinKod() { }
        public void Message(bool message, User u)
        {
            if (message)
            {
                Database.UserAccountsDB.Add(u.Broj, u);
                Console.WriteLine($"Korisnik je uspesno kreirao nalog broj: {u.Broj}.");
            }
            else
            {
                Console.WriteLine($"Nalog broj {u.Broj} vec postoji!");
            }
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }
    }
}
