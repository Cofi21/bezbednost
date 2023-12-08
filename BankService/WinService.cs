using Common;
using Common.Manager;
using Common.Models;
using Manager;
using Newtonsoft.Json;
using SymmetricAlgorithms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace BankService
{
    public class WinService : IWin
    {
        private string secretKey = "123456";
        public void TestCommunication()
        {
          //  IMDatabase.AccountsDB = Json.LoadAccountsFromFile();
          //  IMDatabase.MasterCardsDB = Json.LoadMasterCardsFromFile();
          //  IMDatabase.UsersDB = Json.LoadUsersFromFile();
        }

        public bool KreirajNalog(byte[] recievedData, byte[] signature)
        {
            Account acc = DecryptAndDeserializeAccount(recievedData, secretKey);
            string name = Common.Manager.Formatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
            if (ValidSignature(recievedData.ToString(), signature))
            {
                try
                {
                    Registracija();

                    // Ucitavanje korisnika je odradjeno u funkciji Registracija()
                    LoadAccounts();
                    LoadMasterCards();
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                }
                try
                {
                    if (IMDatabase.AccountsDB.ContainsKey(acc.BrojRacuna))
                    {
                        Console.WriteLine("Vec postoji racun sa unetim brojem!");
                        return false;
                    }
                    else
                    { 
                        MasterCard mc = new MasterCard(name, acc.Pin);
                        acc.MasterCards.Add(mc);
                        IMDatabase.MasterCardsDB.Add(mc);
                        
                        IMDatabase.UsersDB[name].UserAccounts.Add(acc.BrojRacuna, acc);
                        IMDatabase.AccountsDB.Add(acc.BrojRacuna, acc);

                        // Cuvanje korisnika je takodje odradjeno u funkciji Registracija()
                        Json.SaveAccountsToFile(IMDatabase.AccountsDB);
                        Json.SaveMasterCardsToFile(IMDatabase.MasterCardsDB);
                        Json.SaveUsersToFile(IMDatabase.UsersDB);

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
            else
            {
                return false;
            }
        }

        public bool PovuciSertifikat()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, Account> ReadDict()
        {
            return IMDatabase.AccountsDB;
        }

        public Dictionary<string, User> ReadDictUsers()
        {
            return IMDatabase.UsersDB;
        }
        public bool ValidSignature(string message, byte[] signature)
        {
            string clientName = Common.Manager.Formatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
            string clientNameSign = clientName + "_ds";

            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clientNameSign);

            if (DigitalSignature.Verify(message, HashAlgorithm.SHA1, signature, certificate)) return true;
            else return false;
        }

        public static Account DeserializeAccount(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Account));
                return (Account)serializer.ReadObject(memoryStream);
            }
        }

        // Funkcija za dekripciju i deserijsijalizaciju primljenih podataka u objekat Account
        public static Account DecryptAndDeserializeAccount(byte[] encryptedData, string secretKey)
        {
            byte[] decryptedData = TripleDES_Symm_Algorithm.Decrypt(encryptedData, secretKey);
            return DeserializeAccount(decryptedData);
        }

        public static void IzdajSertifikat(string name, string pin)
        {
            //Prepraviti na TestCA 
            string cmd = "/c makecert -sv " + name + ".pvk -iv RootCA.pvk -n \"CN=" + name + "\" -pe -ic RootCA.cer " + name + ".cer -sr localmachine -ss My -sky exchange";
            System.Diagnostics.Process.Start("cmd.exe", cmd).WaitForExit();

            string cmd2 = "/c pvk2pfx.exe /pvk " + name + ".pvk /pi " + pin + " /spc " + name + ".cer /pfx " + name + ".pfx";
            System.Diagnostics.Process.Start("cmd.exe", cmd2).WaitForExit();

            string cmdSign1 = "/c makecert -sv " + name + "_sign.pvk -iv RootCA.pvk -n \"CN=" + name + "_sign" + "\" -pe -ic RootCA.cer " + name + "_sign.cer -sr localmachine -ss My -sky signature";
            System.Diagnostics.Process.Start("cmd.exe", cmdSign1).WaitForExit();

            string cmdSign2 = "/c pvk2pfx.exe /pvk " + name + "_sign.pvk /pi " + pin + " /spc " + name + "_sign.cer /pfx " + name + "_sign.pfx";
            System.Diagnostics.Process.Start("cmd.exe", cmdSign2).WaitForExit();
        }

        public static void LoadAccounts()
        {
            string filePath = "JsonDB/Accounts.json";
            FileInfo info = new FileInfo(filePath);
            if (info.Length != 0)
            {
                IMDatabase.AccountsDB = Json.LoadAccountsFromFile();
            }
        }

        public static void LoadMasterCards()
        {
            string filePath = "JsonDB/MasterCards.json";
            FileInfo info = new FileInfo(filePath);
            if (info.Length != 0)
            {
                IMDatabase.MasterCardsDB = Json.LoadMasterCardsFromFile();
            }
        }

        public static bool Registracija()
        {
            string filePath = "JsonDB/MasterCards.json";
            FileInfo info = new FileInfo(filePath);
            if (info.Length != 0)
            {
                IMDatabase.UsersDB = Json.LoadUsersFromFile();
            }
            string name = Common.Manager.Formatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
            if(!IMDatabase.UsersDB.ContainsKey(name))
            {
                IMDatabase.UsersDB.Add(name, new User(name));
                Json.SaveUsersToFile(IMDatabase.UsersDB);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static byte[] EncryptString(string message, string secretKey)
        {
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(message);

            byte[] encryptedBytes = TripleDES_Symm_Algorithm.Encrypt(bytesToEncrypt, secretKey);

            return encryptedBytes;
        }
    }
}
