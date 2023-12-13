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
        private readonly string secretKey = "123456";

        public bool CreateAccount(byte[] recievedData, byte[] signature)
        {
            Account acc = DecryptAndDeserializeAccount(recievedData, secretKey);
            string name = Common.Manager.Formatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
            Registration();
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
            try
            {
                string workingDirectory = "..//..//..//Certificates";

                string cmd = "/c makecert -sv " + name + ".pvk -iv TestCA.pvk -n \"CN=" + name + "\" -pe -ic TestCA.cer " + name + ".cer -sr localmachine -ss My -sky exchange";
                var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = cmd,
                    Verb = "runas",
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    
                });

                process.WaitForExit();

                
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine("Makecert izlaz: " + output);

                string cmd2 = "/c pvk2pfx.exe /pvk " + name + ".pvk /pi " + pin + " /spc " + name + ".cer /pfx " + name + ".pfx";
                var process2 = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = cmd2,
                    WorkingDirectory = workingDirectory
                });

                process2.WaitForExit();

                string output2 = process.StandardOutput.ReadToEnd();
                Console.WriteLine("pfx izlaz:" + output2);


                string cmdSign1 = "/c makecert -sv " + name + "_sign.pvk -iv TestCA.pvk -n \"CN=" + name + "_sign" + "\" -pe -ic TestCA.cer " + name + "_sign.cer -sr localmachine -ss My -sky signature";
                var process3 = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = cmdSign1,
                    WorkingDirectory = workingDirectory
                });

                process3.WaitForExit();

                string output3 = process.StandardOutput.ReadToEnd();
                Console.WriteLine("Sign1 izlaz: " + output3);

                string cmdSign2 = "/c pvk2pfx.exe /pvk " + name + "_sign.pvk /pi " + pin + " /spc " + name + "_sign.cer /pfx " + name + "_sign.pfx";
                var process4 = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = cmdSign2,
                    WorkingDirectory = workingDirectory
                });

                process4.WaitForExit();

                string output4 = process.StandardOutput.ReadToEnd();
                Console.WriteLine("Sign1 izlaz: " + output4);
                return true;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return false;
            }
            
        }

        public bool PullAndCreateCertificate(string username)
        {
            Registration();
            try
            {
                string path = "..//..//..//Certificates";
                File.Delete(Path.Combine(path, username + ".pvk"));
                File.Delete(Path.Combine(path, username + "_sign.pvk"));
                File.Delete(Path.Combine(path, username + ".pfx"));
                File.Delete(Path.Combine(path, username + "_sign.pfx"));
                File.Delete(Path.Combine(path, username + ".cer"));
                File.Delete(Path.Combine(path, username + "_sign.cer"));


                IMDatabase.UsersDB[username].HaveCertificate = false;

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
    }
}
