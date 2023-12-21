using Common;
using Common.Manager;
using Common.Models;
using Manager;
using SymmetricAlgorithms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace BankService
{
    public class SertService : ICert
    {


        private const int _maxNumberOfTransactions = 3;
        private const int _secondsBetweenTransactions = 120;
        private static Dictionary<string, List<TransactionDetails>> receivedTransactionsDict = new Dictionary<string, List<TransactionDetails>>();

        public bool ResetPinCode(byte[] encMess, byte[] signature)
        {
            string clientName = Common.Manager.Formatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);
            string secretKey = SecretKey.LoadKey(clientName);

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
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

            ChannelFactory<IBankingAudit> factory = new ChannelFactory<IBankingAudit>(binding,
            new EndpointAddress("net.tcp://localhost:8001/BankingAuditService"));

            IBankingAudit serviceProxy = factory.CreateChannel();

            string clientName = Common.Manager.Formatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);
            string secretKey = SecretKey.LoadKey(clientName);

            try
            {
                DateTime currentTime = DateTime.UtcNow;

                IMDatabase.AccountsDB = Json.LoadAccountsFromFile();

                Transaction decTrans = DecryptAndDeserializeAccount(transaction, secretKey);
                string pin = DecryptString(encPin, secretKey);

                if (ValidSignature(transaction.ToString(), signature))
                {
                    
                    if (decTrans.Akcija == 1)
                    {
                        if (IMDatabase.AccountsDB.ContainsKey(decTrans.BrojRacuna))
                        {
                            // proveriti jel ovo okej mesto ili treba samo ako je pin tacan.
                            if (IsMaxNumberOfTransactionsExceeded(decTrans, currentTime, out List<TransactionDetails> listOfTransactionDetails))
                            {
                                Console.WriteLine("Prevelik broj transakcija na istom racunu!");
                                // Izmeniti da bude drugi objekat, treba nam Audit za EventLog
                                TransactionPayments tp = new TransactionPayments()
                                {
                                    BankName = "BankName",
                                    AccountName = decTrans.BrojRacuna,
                                    TimeOfDetection = currentTime,
                                    TransactionsList = listOfTransactionDetails
                                };

                                serviceProxy.AccessingLog(tp);
                                //try
                                //{
                                //    Audit.BankingAuditSuccess(tp.BankName);
                                //}
                                //catch (Exception e)
                                //{
                                //    Console.WriteLine("Banking Audit failed with an error: " + e.Message);
                                //    Audit.BankingAuditFailed(tp.BankName);
                                //}
                            }

                            byte[] key = Convert.FromBase64String(IMDatabase.AccountsDB[decTrans.BrojRacuna].Pin);
                            string keyPin = DecryptString(key, secretKey);

                            if (keyPin.Equals(pin))
                            {
                                IMDatabase.AccountsDB[decTrans.BrojRacuna].Stanje += decTrans.Svota;
                                Console.WriteLine($"Uspesna uplata!");
                                Json.SaveAccountsToFile(IMDatabase.AccountsDB);

                                // Audit log
                                //Audit.TransactionSuccess(clientName);

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
                Console.WriteLine("greska "+ e.Message + "\n" + e.StackTrace);
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

            string decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedString;
        }
        public bool IsMaxNumberOfTransactionsExceeded(Transaction transaction, DateTime currentTime, out List<TransactionDetails> listOfTransactionDetails)
        {
            string accountNumber = transaction.BrojRacuna;

            if (!receivedTransactionsDict.ContainsKey(accountNumber))
            {
                receivedTransactionsDict.Add(accountNumber,
                    new List<TransactionDetails>()
                    {
                        new TransactionDetails()
                        {
                            TransactionOrderNumber = 1,
                            ReceivedDateTime = currentTime,
                            Svota = transaction.Svota
                        }
                    });

                listOfTransactionDetails = receivedTransactionsDict[accountNumber].ToList();
                return 1 > _maxNumberOfTransactions;
            }

            listOfTransactionDetails = receivedTransactionsDict[accountNumber];
            if (listOfTransactionDetails.Count == 0)
            {
                receivedTransactionsDict[accountNumber].Add(
                    new TransactionDetails()
                    {
                        TransactionOrderNumber = 1,
                        ReceivedDateTime = currentTime,
                        Svota = transaction.Svota
                    });

                listOfTransactionDetails = receivedTransactionsDict[accountNumber].ToList();
                return 1 > _maxNumberOfTransactions;
            }

            DateTime timeOfFirstTransaction = listOfTransactionDetails.First().ReceivedDateTime;
            DateTime maximumAllowedTime = timeOfFirstTransaction.AddSeconds(_secondsBetweenTransactions);

            // ako je u okviru zadatog vremena
            if (currentTime < maximumAllowedTime)
            {
                // sada treba proveriti da li je broj transakcija prekoracen
                int currentNumberOfTransactionDetails = listOfTransactionDetails.Count;
                receivedTransactionsDict[accountNumber].Add(
                        new TransactionDetails()
                        {
                            TransactionOrderNumber = currentNumberOfTransactionDetails++,
                            ReceivedDateTime = currentTime,
                            Svota = transaction.Svota
                        });

                listOfTransactionDetails = receivedTransactionsDict[accountNumber].ToList();

                if (currentNumberOfTransactionDetails > _maxNumberOfTransactions)
                {
                    return true; // prekoraceno, cisti se lista i vraca true;
                }
                else
                {
                    return false; // nije prekoraceno, samo vrati false;
                }
            }
            else
            {
                // u ovaj deo koda ulazi jer je vreme isteklo, sada treba resetovati listu i ponovo dodavati elemente
                //receivedTransactionsDict[accountNumber].Clear();

                // obrisi sve elemente ciji je received time manji od trenutnog vremena za _secondsBetweenTransactions od toga
                receivedTransactionsDict[accountNumber] = receivedTransactionsDict[accountNumber]
                    .Where(x => x.ReceivedDateTime.AddSeconds(_secondsBetweenTransactions) >= currentTime)
                    .ToList();

                receivedTransactionsDict[accountNumber].Add(
                    new TransactionDetails()
                    {
                        TransactionOrderNumber = 1,
                        ReceivedDateTime = currentTime,
                        Svota = transaction.Svota
                    });

                listOfTransactionDetails = receivedTransactionsDict[accountNumber].ToList();
                return 1 > _maxNumberOfTransactions;
            }
        }

    }
}