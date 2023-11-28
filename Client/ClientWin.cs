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
    public class ClientWin : ChannelFactory<IWin>, IWin, IDisposable
    {
        IWin factory;

        public ClientWin(NetTcpBinding binding, string address) : base(binding, address)
        {
            try
            {
                factory = this.CreateChannel();
                
            }catch(Exception ex)
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



        public bool KreirajNalog(User u)
        {
            return factory.KreirajNalog(u);
        }

        public void TestCommunication()
        {
            try
            {
                factory.TestCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        public bool IzdajKarticu()
        {
            return factory.IzdajKarticu();
        }

        public bool PovuciSertifikat()
        {
            return factory.PovuciSertifikat();
        }



    }
}
