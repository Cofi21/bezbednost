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
        private static string secretKey;
        

        public bool CreateAccount(byte[] recievedData, byte[] signature)
        {
            Registration();
            Account acc = DecryptAndDeserializeAccount(recievedData, secretKey);
            string name = Common.Manager.Formatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
            if (ValidSignature(recievedData.ToString(), signature))
            {
                try
                {
                    IMDatabase.AccountsDB = Json.LoadAccountsFromFile();
                }
                catch(Exception e)
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
                        IMDatabase.AccountsDB.Add(acc.BrojRacuna, acc);


                        if (!IMDatabase.UsersDB[name].HaveCertificate && CreateMasterCardCertificate(name, acc.Pin))
                        {
                            IMDatabase.UsersDB[name].HaveCertificate = true;
                            Console.WriteLine("Korisniku je uspesno dodeljen sertifikat");
                        }
                        else
                        {
                            Console.WriteLine("Korisnik vec ima sertifikat");
                        }

                        Json.SaveUsersToFile(IMDatabase.UsersDB);
                        Json.SaveAccountsToFile(IMDatabase.AccountsDB);

                        Console.WriteLine("Uspesno kreiranje naloga!");
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

        public static Account DecryptAndDeserializeAccount(byte[] encryptedData, string secretKey)
        {
            byte[] decryptedData = TripleDES_Symm_Algorithm.Decrypt(encryptedData, secretKey);
            return DeserializeAccount(decryptedData);
        }

        public bool CreateMasterCardCertificate(string name, string pin)
        {
            // Dodati logovanje
            string logged = Common.Manager.Formatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
            secretKey = SecretKey.LoadKey(logged);
            try
            {
                string workingDirectory = "..//..//..//Certificates";
                string cmd = "/c makecert -sv " + name + ".pvk -iv TestCA.pvk -n \"CN=" + name + "\" -pe -ic TestCA.cer " + name + ".cer -sr localmachine -ss My -sky exchange";
                var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = cmd,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    
                });

                process.WaitForExit();

                
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine("Makecert izlaz: " + output);
                string pin1;
                if (!int.TryParse(pin, out int num))
                {
                    byte[] key = Convert.FromBase64String(pin);
                    pin1 = DecryptString(key, secretKey);
                }
                else
                  
                pin1 = pin;

                string cmd2 = "/c pvk2pfx.exe /pvk " + name + ".pvk /pi " + pin1 + " /spc " + name + ".cer /pfx " + name + ".pfx";
                Console.WriteLine(cmd2);
                var process2 = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = cmd2,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                });

                process2.WaitForExit();

                string output2 = process2.StandardOutput.ReadToEnd();
                Console.WriteLine("pfx izlaz:" + output2);

                // Audit log
                try
                {
                    Audit.SertificateCreationSuccess(name);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Sertificate Creation failed with an error: " + e.Message);
                }

                return true;
            }catch(Exception e)
            {
                // Audit log
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                try
                {
                    Audit.SertificateCreationFailed(name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in sertificate creation " + ex.Message);
                }
                return false;
            }
            
        }

        public bool PullAndCreateCertificate(string username)
        {
            // Dodati logovanje
            Registration();
            try
            {
                string path = "..//..//..//Certificates";
                File.Delete(Path.Combine(path, username + ".pvk"));
                File.Delete(Path.Combine(path, username + ".pfx"));
                File.Delete(Path.Combine(path, username + ".cer"));

                string pin = Math.Abs(Guid.NewGuid().GetHashCode()).ToString();
                pin = pin.Substring(0, 4);

                Console.WriteLine("Certificate renewed.New pin: " + pin);

                CreateMasterCardCertificate(username, pin);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        public static byte[] EncryptString(string message, string secretKey)
        {
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(message);

            byte[] encryptedBytes = TripleDES_Symm_Algorithm.Encrypt(bytesToEncrypt, secretKey);

            return encryptedBytes;
        }

        public static bool Registration()
        {
            IMDatabase.UsersDB = Json.LoadUsersFromFile();
            string name = Common.Manager.Formatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
            try
            {
                secretKey = SecretKey.LoadKey(name);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            if (!IMDatabase.UsersDB.ContainsKey(name))
            {
                IMDatabase.UsersDB.Add(name, new User(name, false));
                Json.SaveUsersToFile(IMDatabase.UsersDB);
                
                return true;
            }
            else
            {
                return false;
            }

        }

        public static string DecryptString(byte[] encryptedData, string secretKey)
        {
            byte[] decryptedBytes = TripleDES_Symm_Algorithm.Decrypt(encryptedData, secretKey);

            string decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedString;
        }
    }
}
