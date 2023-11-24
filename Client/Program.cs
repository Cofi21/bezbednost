using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using Common;
using Common.Manager;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                GetOperationChoice();    // konstantno pozivanje meni - a
            }
        }

        static void GetOperationChoice()
        {
            Console.WriteLine("Izaberite zahtev: ");
            Console.WriteLine("\t1 - Kreiranje naloga");
            Console.WriteLine("\t2 - Izdavanje kartice");
            Console.WriteLine("\t3 - Povlacenje sertifikata ");
            Console.WriteLine("\t4 - Izvrsenje transakcije");
            Console.WriteLine("\t5 - Reset pin koda");
            Console.WriteLine("\t0 - Izlaz");

            Console.Write("Unesite Vas izbor: ");
            int  operationChoice = Int32.Parse(Console.ReadLine());

            // NA OSNOVU IZBORA BIRAMO KOJI CEMO PROTOKOL DA KORISTIMO
            if (operationChoice == 2)
            {
                CertConnection(operationChoice);
            }
            else
            {
                AuthConnection(operationChoice);
            }
        }

        static void CertConnection(int broj)
        {
            string srvCertCN = "server";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:4001/MainService"), new X509CertificateEndpointIdentity(srvCert));

            using (ClientCert proxy = new ClientCert(binding, address))
            {
                try
                {
                    Console.WriteLine("Certificate communication is active");
                    ClientOperations.Opcije(broj, null, proxy);     // u konstruktoru metode su dodati redom (int, IMain, ICert) radi ispisa na serveru ali 
                                                                    //  to ne radi pa moramo videti kako cemo
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static void AuthConnection(int broj)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:4000/MainService";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            using (ClientProxy proxy = new ClientProxy(binding, address))
            {

                try
                {
                    Console.WriteLine("Windows Authentication communication is active");    // isto kao i gore 
                    ClientOperations.Opcije(broj, proxy, null); 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
