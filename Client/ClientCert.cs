using Common;
using Common.Manager;
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
        ICert certFactory;
        public int Connection()
        {
            try
            {
                Console.WriteLine("Uspesna konekcija, klijent");
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
                return 0;
            }
        }
        public ClientCert(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
            try
            {

            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            certFactory = this.CreateChannel();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska u konstruktoru " + ex);
            }
        }

        public ClientCert()
        {
        }

        public void Dispose()
        {
            if (certFactory != null)
            {
                certFactory = null;
            }

            this.Close();
        }
    }
}
