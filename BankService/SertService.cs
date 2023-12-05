using Common;
using Common.Manager;
using Common.Models;
using Manager;
using SymmetricAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankService
{
    public class SertService : ICert
    {
        public void TestCommunication()
        {
            Console.WriteLine("Communication established.");
        }

        private string secretKey = "123456";
        public bool IzdajKarticu()
        {
            throw new NotImplementedException();
        }

        public bool PovuciSertifikat()
        {
            throw new NotImplementedException();
        }

        public bool ResetujPinKod(string pin, string brojNaloga, byte[] signature)
        {
            if(ValidSignature(brojNaloga, signature))
            {
                if (IMDatabase.AllUserAccountsDB.ContainsKey(brojNaloga.Trim()))
                {
                    IMDatabase.AllUserAccountsDB[brojNaloga].Pin = pin;
                    foreach (MasterCard mc in IMDatabase.AllUserAccountsDB[brojNaloga].MasterCards)
                    {
                        if (mc.SubjectName.Equals(WindowsIdentity.GetCurrent().Name))
                        {
                            mc.Pin = pin;
                        }
                    }

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

        public bool IzvrsiTransakciju(int opcija, byte[] brojRacunaEnc, byte[] svotaNovca, byte[] signature)
        {
            try
            {
                double svota = DecryptDouble(svotaNovca, secretKey);
                string brojRacuna = DecryptString(brojRacunaEnc, secretKey);

                if (ValidSignature(opcija.ToString(), signature))
                {
                    Console.WriteLine("Sign is valid");
                    if (opcija == 1)
                    {
                        if (IMDatabase.AllUserAccountsDB.ContainsKey(brojRacuna))
                        {
                            IMDatabase.AllUserAccountsDB[brojRacuna].Stanje += svota;
                            Console.WriteLine($"Uspesna uplata!");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Neuspesna uplata! Ne postoji racun sa brojem {brojRacuna}");
                            return false;
                        }
                    }
                    else if (opcija == 2)
                    {
                        if (IMDatabase.AllUserAccountsDB.ContainsKey(brojRacuna))
                        {
                            double trenutnoStanje = IMDatabase.AllUserAccountsDB[brojRacuna].Stanje;

                            if (trenutnoStanje < svota)
                            {
                                Console.WriteLine($"Na racunu nemate dovoljno sretstava za isplatu!");
                                return false;
                            }
                            IMDatabase.AllUserAccountsDB[brojRacuna].Stanje -= svota;
                            Console.WriteLine($"Uspesna isplata!");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Neuspesna uplata!Ne postoji racun sa brojem {brojRacuna}");
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
   
        public bool ValidSignature(string message, byte[] signature) 
        {
            string clientName = Formatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);
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

        public static double DecryptDouble(byte[] encryptedData, string secretKey)
        {
            byte[] decryptedBytes = TripleDES_Symm_Algorithm.Decrypt(encryptedData, secretKey);

            double decryptedDouble = BitConverter.ToDouble(decryptedBytes, 0);

            return decryptedDouble;
        }

    }
}