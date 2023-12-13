using Common;
using Common.Manager;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Replikator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isFirstTime = true;
            while (true)
            {
                try
                {
                    NetTcpBinding binding = new NetTcpBinding();
                    string address1 = "net.tcp://localhost:4002/Replikator";
                    string address2 = "net.tcp://localhost:8000/Replikator";

                    binding.Security.Mode = SecurityMode.Transport;
                    binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                    binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

                    BackupWin izvor = new BackupWin(binding, address1);
                    BackupWin odrediste = new BackupWin(binding, address2);

                    if (isFirstTime)
                    {
                        odrediste.Send(IMDatabase.AccountsDB.Values.ToList());
                        isFirstTime = false; 
                    }
                    else
                    {
                        List<Account> accounts = izvor.Collect();
                        odrediste.Send(accounts);

                        Console.WriteLine("Repliciranje...");
                    }

                    Thread.Sleep(4000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                }
            }
        }
    }
}
