using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using Common;
using Common.Manager;
using Common.Models;
using Manager;
using SymmetricAlgorithms;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Logovani korisnik { WindowsIdentity.GetCurrent().Name}");
            while (true)
            {
                int operacija = Meni();
                if(operacija == 3 || operacija == 4)
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
            Console.WriteLine("\t5 - Ispis naloga");
            Console.WriteLine("\t0 - Izlaz");

            Console.Write("Unesite Vas izbor: ");
            int izbor = Int32.Parse(Console.ReadLine());
            return izbor;
        }
        static void CertConnection(int operacija)
        {
            string srvCertCN = "server";
            string secretKey = "123456";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:4001/SertService"), new X509CertificateEndpointIdentity(srvCert));

            // digitalni potpisi
            string signCertCN = Common.Manager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name) + "_ds";

            X509Certificate2 certificateSign = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCN);
            using (ClientCert proxy = new ClientCert(binding, address))
            {
                try
                {
                    if(operacija == 3)
                    { 
                        Console.WriteLine("Izaberite akciju: ");
                        Console.WriteLine("\t1 - Uplata novca");
                        Console.WriteLine("\t2 - Podizanje novca ");
                        Console.WriteLine("\t0 - Izlaz");

                        Console.Write("Unesite Vas izbor: ");
                        int izbor = Int32.Parse(Console.ReadLine());
                        double svotaNovca = 0;

                        if (izbor == 0) return;

                        Console.Write("Unesite broj racuna na kom ce se izvrsiti transakcija: ");
                        string brojRacuna = Console.ReadLine();

                        if (izbor == 1)
                        {
                            Console.Write("Unesite koliko novca zelite da uplatite: ");
                            svotaNovca = Double.Parse(Console.ReadLine());

                            Transaction transaction = new Transaction(izbor, brojRacuna, svotaNovca);

                            byte[] transactionCrypted = CreateEncryptedTransaction(transaction, secretKey);
                            byte[] signature = DigitalSignature.Create(transactionCrypted.ToString(), Manager.HashAlgorithm.SHA1, certificateSign);
                            
                            Console.Write("Unesite PIN: ");
                            string pinCode = ReadPassword();
                            byte[] encPin = CreateEncryptedPin(pinCode, secretKey);

                            if (proxy.IzvrsiTransakciju(transactionCrypted, signature, encPin)) Console.WriteLine($"Uspesno ste uplatili {transaction.Svota} dinara.");
                            else Console.WriteLine("Transakcija nije moguca! Uneli ste nepostojeci broj racuna!");
                        }
                        else if (izbor == 2)
                        {
                            Console.Write("Unesite koliko novca zelite da podignete: ");
                            svotaNovca = Double.Parse(Console.ReadLine());

                            Transaction transaction = new Transaction(izbor, brojRacuna, svotaNovca);

                            byte[] transactionCrypted = CreateEncryptedTransaction(transaction, secretKey);
                            byte[] signature = DigitalSignature.Create(transactionCrypted.ToString(), Manager.HashAlgorithm.SHA1, certificateSign);

                            Console.Write("Unesite PIN: ");
                            string pinCode = ReadPassword();
                            byte[] encPin = CreateEncryptedPin(pinCode, secretKey);

                            if (proxy.IzvrsiTransakciju(transactionCrypted, signature, encPin)) Console.WriteLine($"Uspesno ste podigli {svotaNovca} dinara.");
                            else Console.WriteLine("Transakcija nije moguca! Uneli ste nepostojeci broj racuna ili nemate dovoljno sredstava na racunu!");
                        }

                    }else if(operacija == 4)
                    {
                        Console.Write("Unesite broj naloga: ");
                        string brojNaloga = Console.ReadLine();
                        string pin = ResetPin(brojNaloga, secretKey);
                        string message = pin + "|" + brojNaloga;

                        byte[] encMess = EncryptString(message, secretKey);
                        byte[] signature = DigitalSignature.Create(brojNaloga, Manager.HashAlgorithm.SHA1, certificateSign);
                        if (proxy.ResetujPinKod(encMess, signature))
                        {
                            Console.WriteLine("Uspesna promena pin koda!");
                        }
                        else
                        {
                            Console.WriteLine("Greska! Pin kod nije promenjen!");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message  + "\n" +e.StackTrace);
                }
            }
        }
        static void AuthConnection(int broj)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:4000/WinService";

            string secretKey = "123456";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            // digitalni potpisi
            string signCertCN = Common.Manager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name) + "_ds";
            X509Certificate2 certificateSign = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCN);

            using (ClientWin proxy = new ClientWin(binding, address))
            {
                ReadAccounts(proxy);
                try
                {
                    switch (broj)
                    {
                        case 1:
                            Account acc = KreirajNalog(secretKey);
                            if (acc == null) Console.WriteLine("Nalog je null");
                            byte[] account = CreateEncryptedAccount(acc, secretKey);
                            //byte[] signature = DigitalSignature.Create(account.ToString(), Manager.HashAlgorithm.SHA1, certificateSign);
                            byte[] signature = null;
                            if (proxy.KreirajNalog(account, signature))
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
                        case 5:
                            Ispis();
                            break;
                        case 0:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                    Console.ReadKey();
                }
            }
        }
        public static void Ispis()
        {
            IMDatabase.AccountsDB = Json.LoadAccountsFromFile();

            foreach(Account acc in IMDatabase.AccountsDB.Values)
            {
                Console.WriteLine($"\tBroj racuna: {acc.BrojRacuna}\tStanje: {acc.Stanje}");
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
        public static Account KreirajNalog(string secretKey)
        {
            string logged = WindowsIdentity.GetCurrent().Name;
            string[] parts = logged.Split('\\');
            string username = parts[1];
            Console.WriteLine("Username: " + username);
            Console.Write("Unesite broj naloga: ");
            string broj = Console.ReadLine();
            Console.Write("Unesite PIN: ");
            string pin = ReadPassword();
            Console.Write("Potvrdite PIN: ");
            string pinPotvrda = ReadPassword();

            if (pin.Equals(pinPotvrda))
            {
                byte[] key = EncryptString(pin, secretKey);
                string pinCode = Convert.ToBase64String(key);
                return new Account(broj, pinCode, username);
            }
            else
            {
                Console.WriteLine("Greska! Uneti PIN kodovi se ne poklapaju.");
                return null;
            }
        }      
        public static string ResetPin(string brojNaloga, string secretKey)
        {
            IMDatabase.AccountsDB = Json.LoadAccountsFromFile();
            if (IMDatabase.AccountsDB.ContainsKey(brojNaloga.Trim()))
            {
                Console.Write("Unesite stari Pin: ");
                string stariPin = ReadPassword();
                string pinCode;

                byte[] key = Convert.FromBase64String(IMDatabase.AccountsDB[brojNaloga].Pin);
                string keyPin = DecryptString(key, secretKey);

                if (keyPin.Equals(stariPin))
                {
                    Console.Write("Unesite novi Pin: ");
                    string noviPin = ReadPassword();
                    Console.Write("Potvrdite novi Pin: ");
                    string noviPinPotvrda = ReadPassword();
                    if (noviPin.Equals(noviPinPotvrda))
                    {
                        byte[] newPin = EncryptString(noviPin, secretKey);
                        pinCode = Convert.ToBase64String(newPin);
                    }
                    else
                    {
                        Console.WriteLine("Greska! Pin kodovi se ne poklapaju!");
                        return null;
                    }

                    return pinCode;
                }
                else
                {
                    Console.WriteLine("Pogresan pin kod ili broj naloga. Pokusajte ponovo!");
                    return String.Empty;
                }
            }
            else
            {
                Console.WriteLine("Greska! Uneti broj naloga ne postoji!");
                return null;
            }

        }
        public static void ReadAccounts(ClientWin proxy)
        {                               //      return IMDatabase.AccountsDB;
            Dictionary<string, Account> AllUsersDict = proxy.ReadDict();      
            foreach (Account acc in AllUsersDict.Values)
            {
                if (!IMDatabase.AccountsDB.ContainsKey(acc.BrojRacuna))
                {
                    IMDatabase.AccountsDB.Add(acc.BrojRacuna, acc);
                }
            }
        }
        public static byte[] SerializeAccount(Account account)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Account));
                serializer.WriteObject(memoryStream, account);
                return memoryStream.ToArray();
            }
        }
        public static byte[] SerializeTransaction(Transaction transaction)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Transaction));
                serializer.WriteObject(memoryStream, transaction);
                return memoryStream.ToArray();
            }
        }
        public static byte[] CreateEncryptedAccount(Account account, string secretKey)
        {
            byte[] serializedAccount = SerializeAccount(account);
            return TripleDES_Symm_Algorithm.Encrypt(serializedAccount, secretKey);
        }    
        public static byte[] CreateEncryptedPin(string pin, string secretKey)
        {
            return EncryptString(pin, secretKey);
        }
        public static byte[] CreateEncryptedTransaction(Transaction transaction, string secretKey)
        {
            byte[] serializedTransaction = SerializeTransaction(transaction);
            return TripleDES_Symm_Algorithm.Encrypt(serializedTransaction, secretKey);
        }   
        public static byte[] EncryptString(string message, string secretKey)
        {
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(message);

            byte[] encryptedBytes = TripleDES_Symm_Algorithm.Encrypt(bytesToEncrypt, secretKey);

            return encryptedBytes;
        }
        public static string DecryptString(byte[] encryptedData, string secretKey)
        {
            byte[] decryptedBytes = TripleDES_Symm_Algorithm.Decrypt(encryptedData, secretKey);

            // Konvertovanje dekriptovanih bajtova u string
            string decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedString;
        }
    }
}
