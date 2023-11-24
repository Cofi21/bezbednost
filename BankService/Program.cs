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
            Console.WriteLine("Server je pokrenut od strane " + WindowsIdentity.GetCurrent().Name);
            
            /// Windows komunikacija
            NetTcpBinding bindingWin = new NetTcpBinding();
            bindingWin.Security.Mode = SecurityMode.Transport;
            bindingWin.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingWin.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            string addressWin = "net.tcp://localhost:4000/MainService";
            ServiceHost hostWin = new ServiceHost(typeof(MainService));
            hostWin.AddServiceEndpoint(typeof(IMain), bindingWin, addressWin);

            /// Komunikacija koriscenjem sertifikata
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            string address = "net.tcp://localhost:4001/MainService";
            ServiceHost hostCert = new ServiceHost(typeof(MainService));
            hostCert.AddServiceEndpoint(typeof(IMain), binding, address);
            hostCert.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            hostCert.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            hostCert.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            while (true)
            {

                try
                {
                    hostWin.Open();
                    Console.WriteLine("Windows server");

                    /// OVDE ON NIKADA NE UDJE, TREBAMO MU SAMO NEKAKO PROSLEDITI DA PROVERI KADA KOJU VRSTU KOMUNIKACIJE KORISTIMO
                    if (MainService.Odgovor == 1)
                    {
                        // Close the Windows Authentication host
                        hostWin.Close();

                        // Open the Certificate Authentication host
                        hostCert.Open();
                        Console.WriteLine("Certificate server");

                        // Perform operations with Certificate Authentication
                        // ...

                        // Switch back to Windows Authentication
                        hostCert.Close();
                        hostWin = new ServiceHost(typeof(MainService));
                        hostWin.AddServiceEndpoint(typeof(IMain), bindingWin, addressWin);
                        hostWin.Open();
                        Console.WriteLine("Windows server");
                    }

                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] {0}", e.Message);
                    Console.WriteLine("[StackTrace] {0}", e.StackTrace);
                }
                finally
                {
                    hostWin.Close();
                }
                
            }
        }


    }
}
