using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankService;
using Common;
using Common.Models;

namespace BackupService
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding bindingWin = new NetTcpBinding();
            bindingWin.Security.Mode = SecurityMode.Transport;
            bindingWin.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingWin.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            string addressWin = "net.tcp://localhost:8000/Replikator";
            ServiceHost hostWin = new ServiceHost(typeof(Replikator));
            hostWin.AddServiceEndpoint(typeof(IReplikator), bindingWin, addressWin);


            try
            {
                hostWin.Open();
                Console.WriteLine("Backup server...");

                ChannelFactory<IReplikator> channelFactory = new ChannelFactory<IReplikator>(bindingWin, new EndpointAddress(addressWin));
                IReplikator replicator = channelFactory.CreateChannel();

                while (true)
                {
                    Thread.Sleep(4000);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                hostWin.Close();
            }
        }
    }
}
