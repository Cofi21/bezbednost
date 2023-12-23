using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace BankingAudit
{
    public class Program
    {
        static void Main(string[] args)
        {
            //NetTcpBinding bindingWin = new NetTcpBinding();
            //bindingWin.Security.Mode = SecurityMode.Transport;
            //bindingWin.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //bindingWin.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            //string addressWin = "net.tcp://localhost:8001/BankingAuditService";
            //ServiceHost hostWin = new ServiceHost(typeof(BankingAuditService));
            //hostWin.AddServiceEndpoint(typeof(IBankingAudit), bindingWin, addressWin);

            Console.WriteLine("BankingAudit system is started.\nPress <enter> to stop ...");
            Console.ReadLine();
            //try
            //{
            //    hostWin.Open();   
            //} 
            //catch (Exception e)
            //{
            //    Console.WriteLine("[ERROR] {0}", e.Message);
            //    Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            //    Console.ReadLine();
            //}
            //finally
            //{
            //    hostWin.Close();
            //}
        }
    }
}
