using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using Common;
using Common.Manager;
using Common.Models;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {     
            if(!Database.UsersDB.ContainsKey(WindowsIdentity.GetCurrent().Name))
            {
                Database.UsersDB.Add(WindowsIdentity.GetCurrent().Name, new User(WindowsIdentity.GetCurrent().Name));
            }

            while (true)
            {
                int operacija = Meni();
                if(operacija == 3)
                {
                    CertConnection(operacija);
                }
                else
                {
                    AuthConnection(operacija);
                }
            }
        }


        public static int Meni()
        {
            Console.WriteLine("Izaberite zahtev: ");
            Console.WriteLine("\t1 - Kreiranje naloga");
            Console.WriteLine("\t2 - Povlacenje sertifikata ");
            Console.WriteLine("\t3 - Izvrsenje transakcije");
            Console.WriteLine("\t4 - Reset pin koda");
            Console.WriteLine("\t0 - Izlaz");

            Console.Write("Unesite Vas izbor: ");
            int izbor = Int32.Parse(Console.ReadLine());
            return izbor;
        }

        static void CertConnection(int operacija)
        {
            string srvCertCN = "server";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:4001/SertService"), new X509CertificateEndpointIdentity(srvCert));

            using (ClientCert proxy = new ClientCert(binding, address))
            {
                try
                {
                    Console.WriteLine("Certificate communication is active");
                    Console.WriteLine("Izaberite akciju: ");
                    Console.WriteLine("\t1 - Uplata novca");
                    Console.WriteLine("\t2 - Podizanje novca ");
                    Console.WriteLine("\t0 - Izlaz");
                    Console.Write("Unesite Vas izbor: ");
                    int izbor = Int32.Parse(Console.ReadLine());

                    if (izbor == 0) return;
                    Console.Write("Unesite broj racuna na kom ce se izvrsiti transakcija: ");
                    string brojRacuna = Console.ReadLine();
                    double svotaNovca = 0;

                    if (izbor == 1)
                    {
                        Console.Write("Unesite koliko novca zelite da uplatite: ");
                        svotaNovca = Double.Parse(Console.ReadLine());
                        if (proxy.IzvrsiTransakciju(izbor, brojRacuna, svotaNovca)) Console.WriteLine($"Uspesno ste uplatili {svotaNovca} dinara.");
                        else Console.WriteLine("Transakcija nije moguca! Uneli ste nepostojeci broj racuna!");
                    }
                    else if (izbor == 2)
                    {
                        Console.Write("Unesite koliko novca zelite da podignete: ");
                        svotaNovca = Double.Parse(Console.ReadLine());
                        if (proxy.IzvrsiTransakciju(izbor, brojRacuna, svotaNovca)) Console.WriteLine($"Uspesno ste podigli {svotaNovca} dinara.");
                        else Console.WriteLine("Transakcija nije moguca! Uneli ste nepostojeci broj racuna ili nemate dovoljno sredstava na racunu!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void AuthConnection(int broj)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:4000/MainService";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            using (ClientWin proxy = new ClientWin(binding, address))
            {
                Console.WriteLine("Windows Authentication communication is active");    
                try
                {
                    switch (broj)
                    {
                        case 1:
                            Account acc = KreirajNalog();
                            if (proxy.KreirajNalog(acc))
                            {
                                Console.WriteLine("Cestitamo! Uspesno ste kreirali nalog!");
                            }
                            else
                            {
                                Console.WriteLine("Greska! Nalog sa unetim brojem vec postoji!");
                            }
                            break;
                        case 2:
                            break;

                        case 4:
                            break;

                        case 5:
                            break;

                        case 0:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                }
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
        public static Account KreirajNalog()
        {
            //string username = WindowsIdentity.GetCurrent().Name;
            Console.Write("Unesite broj naloga: ");
            string broj = Console.ReadLine();
            Console.Write("Unesite PIN: ");
            string pin = ReadPassword();
            Console.Write("Potvrdite PIN: ");
            string pinPotvrda = ReadPassword();

            if (pin.Equals(pinPotvrda))
            {
                return new Account(broj, pin);
            }
            else
            {
                Console.WriteLine("Greska! Uneti PIN kodovi se ne poklapaju.");
                return null;
            }
        }



    }
}
