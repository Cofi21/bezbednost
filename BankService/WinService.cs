using Common;
using Common.Manager;
using Common.Models;
using Manager;
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
            Console.WriteLine("Communication established.");
        }

        public bool KreirajNalog(byte[] recievedData, byte[] signature)
        {
            string name = Common.Manager.Formatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
            Account acc = DecryptAndDeserializeAccount(recievedData, secretKey);
            if (ValidSignature(acc.BrojRacuna, signature))
            {

                if (!IMDatabase.UsersDB.ContainsKey(name))
                {
                    IMDatabase.UsersDB.Add(name, new User(name));
                }
                Console.WriteLine("Dodat user u bazu");
                try
                {
                    if (IMDatabase.AllUserAccountsDB.ContainsKey(acc.BrojRacuna))
                    {
                        Console.WriteLine("Vec postoji racun sa unetim brojem!");
                        return false;
                    }
                    else
                    { 
                        MasterCard mc = new MasterCard(name, acc.Pin);
                        acc.MasterCards.Add(mc);

                        IMDatabase.UsersDB[name].UserAccounts.Add(acc.BrojRacuna, acc);
                        IMDatabase.AllUserAccountsDB.Add(acc.BrojRacuna, acc);

                        //Ispis radi provere
                        foreach (Account ac in IMDatabase.AllUserAccountsDB.Values)
                        {
                            Console.WriteLine(ac.ToString());
                        }
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
            return IMDatabase.AllUserAccountsDB;
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

    }
}
