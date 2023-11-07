using Common;
using Common.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace BankService
{
    class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
           
         //   NetTcpBinding bindingWin = new NetTcpBinding();
         //   bindingWin.Security.Mode = SecurityMode.Transport;
         //   bindingWin.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
         //   bindingWin.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;


            string address = "net.tcp://localhost:8000/MainService";
          //  string address2 = "net.tcp://localhost:4000/MainService";
            ServiceHost host = new ServiceHost(typeof(MainService));
            host.AddServiceEndpoint(typeof(IMain), binding, address);
          //  host.AddServiceEndpoint(typeof(IMain), bindingWin, address2);

            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            try
            {
                host.Open();

                Console.WriteLine("Server je pokrenut od strane " + WindowsIdentity.GetCurrent().Name);
                Console.WriteLine("Pritisnite [Enter] za zaustavljanje servera!");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                host.Close();
            }
 
        }
    }
}
