using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Manager;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = "server";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;


          //  NetTcpBinding bindingWin = new NetTcpBinding();
           // bindingWin.Security.Mode = SecurityMode.Transport;
           // bindingWin.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
           // bindingWin.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
           
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:8000/MainService"), new X509CertificateEndpointIdentity(srvCert));
          //  EndpointAddress address2 = new EndpointAddress(new Uri("net.tcp://localhost:4000/MainService"), new X509CertificateEndpointIdentity(srvCert));

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
