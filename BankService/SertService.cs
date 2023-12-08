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
using System.Threading.Tasks;

namespace BankService
{
    public class SertService : ICert
    {
        private string secretKey = "123456";

        public void TestCommunication()
        {
        }
        public bool IzdajKarticu()
        {
            throw new NotImplementedException();
        }
        public bool PovuciSertifikat()
        {
            throw new NotImplementedException();
        }
        public bool ResetujPinKod(byte[] encMess, byte[] signature)
        {
            IMDatabase.AccountsDB = Json.LoadAccountsFromFile();
            string decMess = DecryptString(encMess, secretKey);
            string[] parts = decMess.Split('|');
            string pin = parts[0];
            string brojNaloga = parts[1];

            if (ValidSignature(brojNaloga, signature))
            {
                if (IMDatabase.AccountsDB.ContainsKey(brojNaloga.Trim()))
                {
                    IMDatabase.AccountsDB[brojNaloga].Pin = pin;
                    foreach (MasterCard mc in IMDatabase.AccountsDB[brojNaloga].MasterCards)
                    {
                        if (mc.SubjectName.Equals(WindowsIdentity.GetCurrent().Name))
                        {
                            mc.Pin = pin;
                        }
                    }
                    Json.SaveAccountsToFile(IMDatabase.AccountsDB);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool IzvrsiTransakciju(byte[] transaction, byte[] signature, byte[] encPin)
        {
            try
            {
                IMDatabase.AccountsDB = Json.LoadAccountsFromFile();
                IMDatabase.MasterCardsDB = Json.LoadMasterCardsFromFile();

                Transaction decTrans = DecryptAndDeserializeAccount(transaction, secretKey);
                string pin = DecryptString(encPin, secretKey);

                if (ValidSignature(transaction.ToString(), signature))
                {
                    Console.WriteLine("Sign is valid");
                    if (decTrans.Akcija == 1)
                    {
                        if (IMDatabase.AccountsDB.ContainsKey(decTrans.BrojRacuna))
                        {
                            byte[] key = Convert.FromBase64String(IMDatabase.AccountsDB[decTrans.BrojRacuna].Pin);
                            string keyPin = DecryptString(key, secretKey);

                            if (keyPin.Equals(pin))
                            {
                                IMDatabase.AccountsDB[decTrans.BrojRacuna].Stanje += decTrans.Svota;
                                Console.WriteLine($"Uspesna uplata!");
                                Json.SaveAccountsToFile(IMDatabase.AccountsDB);
                                Json.SaveMasterCardsToFile(IMDatabase.MasterCardsDB);
                                return true;
                            }
                            else
                            {
                                Console.WriteLine($"Uneli ste los pin code");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Neuspesna uplata! Ne postoji racun sa brojem {decTrans.BrojRacuna}");
                            return false;
                        }
                    }
                    else if (decTrans.Akcija == 2)
                    {
                        if (IMDatabase.AccountsDB.ContainsKey(decTrans.BrojRacuna))
                        {
                            byte[] key = Convert.FromBase64String(IMDatabase.AccountsDB[decTrans.BrojRacuna].Pin);
                            string keyPin = DecryptString(key, secretKey);

                            if (keyPin.Equals(pin))
                            {

                                double trenutnoStanje = IMDatabase.AccountsDB[decTrans.BrojRacuna].Stanje;

                                if (trenutnoStanje < decTrans.Svota)
                                {
                                    Console.WriteLine($"Na racunu nemate dovoljno sretstava za isplatu!");
                                    return false;
                                }
                                IMDatabase.AccountsDB[decTrans.BrojRacuna].Stanje -= decTrans.Svota;
                                Console.WriteLine($"Uspesna isplata!");
                                Json.SaveAccountsToFile(IMDatabase.AccountsDB);
                                Json.SaveMasterCardsToFile(IMDatabase.MasterCardsDB);
                                return true;
                            }
                            else
                            {
                                Console.WriteLine($"Uneli ste los pin code");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Neuspesna uplata!Ne postoji racun sa brojem {decTrans.BrojRacuna} ili ste uneli los pin kod.");
                            return false;
                        }

                    }
                    return false;
                }
                else
                {
                    Console.WriteLine("Sign is invalid");
                    return false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("greska "+ e.Message);
                return false;
            }
        }
        public static Transaction DeserializeTransaction(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Transaction));
                return (Transaction)serializer.ReadObject(memoryStream);
            }
        }
        public static Transaction DecryptAndDeserializeAccount(byte[] encryptedData, string secretKey)
        {
            byte[] decryptedData = TripleDES_Symm_Algorithm.Decrypt(encryptedData, secretKey);
            return DeserializeTransaction(decryptedData);
        }
        public bool ValidSignature(string message, byte[] signature) 
        {
            string clientName = Common.Manager.Formatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);
            string clientNameSign = clientName + "_ds";

            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, clientNameSign);

            if (DigitalSignature.Verify(message, HashAlgorithm.SHA1, signature, certificate)) return true;
            else return false;
        }
        public static string DecryptString(byte[] encryptedData, string secretKey)
        {
            byte[] decryptedBytes = TripleDES_Symm_Algorithm.Decrypt(encryptedData, secretKey);

            // Konvertovanje dekriptovanih bajtova u string
            string decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedString;
        }
        /*

        public static double DecryptDouble(byte[] encryptedData, string secretKey)
        {
            byte[] decryptedBytes = TripleDES_Symm_Algorithm.Decrypt(encryptedData, secretKey);

            double decryptedDouble = BitConverter.ToDouble(decryptedBytes, 0);

            return decryptedDouble;
        }
        */
    }
}