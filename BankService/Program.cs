﻿using Common;
using Common.Manager;
using Common.Models;
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

            #region Windows
            NetTcpBinding bindingWin = new NetTcpBinding();
            bindingWin.Security.Mode = SecurityMode.Transport;
            bindingWin.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingWin.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            string addressWin = "net.tcp://localhost:4000/MainService";
            ServiceHost hostWin = new ServiceHost(typeof(WinService));
            hostWin.AddServiceEndpoint(typeof(IWin), bindingWin, addressWin);
            #endregion

            #region Certificates
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            string address = "net.tcp://localhost:4001/SertService";
            ServiceHost hostCert = new ServiceHost(typeof(SertService));
            hostCert.AddServiceEndpoint(typeof(ICert), binding, address);
            hostCert.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            hostCert.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            hostCert.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            #endregion

            try
            {
                hostWin.Open();
                Console.WriteLine("Windows server");

                hostCert.Open();
                Console.WriteLine("Certificate server");

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
            }
        }



    }
}
