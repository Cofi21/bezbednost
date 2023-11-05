using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:4000/MainService";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;


            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Klijent je pokrenut od strane " + WindowsIdentity.GetCurrent().Name);
                        proxy.Ispis();
                        Console.ReadKey();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
                        
        }
    }
}
