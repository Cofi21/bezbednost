using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankService
{
    class Program
    {
        static void Main(string[] args)
        {
            MainService ms = new MainService();
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:4000/MainService";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;


            using (ServiceHost host = new ServiceHost(typeof(MainService)))
            {
                host.AddServiceEndpoint(typeof(IMain), binding, address);
                host.Open();
                Console.WriteLine("Server je pokrenut...");
                Console.WriteLine("Pritisnite [Enter] za zaustavljanje servera!");
                Console.ReadKey();
                host.Close();
            }
        }
    }
}
