using BankingAudit;
using Common;
using Common.Manager;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace BankService
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Server je pokrenut od strane " + WindowsIdentity.GetCurrent().Name);

            #region Windows
            NetTcpBinding bindingWin = new NetTcpBinding();
            bindingWin.Security.Mode = SecurityMode.Transport;
            bindingWin.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingWin.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            string addressWin = "net.tcp://localhost:4000/WinService";
            ServiceHost hostWin = new ServiceHost(typeof(WinService));
            hostWin.AddServiceEndpoint(typeof(IWin), bindingWin, addressWin);
            #endregion

            #region Certificates
            string srvCertCN = "server";
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:4001/SertService";
            ServiceHost hostCert = new ServiceHost(typeof(SertService));
            hostCert.AddServiceEndpoint(typeof(ICert), binding, address);
            hostCert.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            hostCert.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            hostCert.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            #endregion

            #region Replikator
            NetTcpBinding bindingWinRep = new NetTcpBinding();
            bindingWinRep.Security.Mode = SecurityMode.Transport;
            bindingWinRep.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingWinRep.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            string addressWinRep = "net.tcp://localhost:4002/Replikator";
            ServiceHost hostWinRep = new ServiceHost(typeof(Replikator));
            hostWinRep.AddServiceEndpoint(typeof(IReplikator), bindingWinRep, addressWinRep);
            #endregion

            #region BankingAudit
            NetTcpBinding bindingAudit = new NetTcpBinding();
            string bankAuditAddress = "net.tcp://localhost:4003/BankingAuditService";
            bindingAudit.Security.Mode = SecurityMode.Transport;
            bindingAudit.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingAudit.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            ServiceHost hostWinBankingAudit = new ServiceHost(typeof(BankingAuditService));
            hostWinBankingAudit.AddServiceEndpoint(typeof(IBankingAudit), bindingAudit, bankAuditAddress);
            #endregion

            try
            {
                hostWin.Open();
                Console.WriteLine("Windows server");

                hostCert.Open();
                Console.WriteLine("Certificate server");

                hostWinRep.Open();
                Console.WriteLine("Replikator server");

                hostWinBankingAudit.Open();
                Console.WriteLine("Banking Audit server");

                Console.ReadKey(); 
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                hostCert.Close();
                hostWin.Close();
                hostWinRep.Close();
                hostWinBankingAudit.Close();
            }
        }
    }
}
