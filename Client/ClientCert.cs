using Common;
using Common.Manager;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientCert : ChannelFactory<ICert>, ICert
    {
        ICert factory;

        public ClientCert(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
            try
            {

                string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

                this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
                this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

                this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

                factory = this.CreateChannel();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska u konstruktoru " + ex);
            }
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public bool ResetPinCode(byte[] encMess, byte[] signature)
        {
            return factory.ResetPinCode(encMess, signature);
        }

        public bool IzvrsiTransakciju(byte[] transaction, byte[] signature, byte[] encPin)
        {
            try
            {
                return factory.IzvrsiTransakciju(transaction, signature, encPin);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

    }
}
