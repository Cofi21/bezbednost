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



        public bool KreirajNalog(Account acc)
        {
            return factory.KreirajNalog(acc);
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

        public bool PovuciSertifikat()
        {
            return factory.PovuciSertifikat();
        }

        public bool ResetujPinKod(string pin, string brojNaloga)
        {
            return factory.ResetujPinKod(pin, brojNaloga);
        }

        public Dictionary<string, Account> ReadDict()
        {
            return factory.ReadDict();
        }

        public Dictionary<string, User> ReadDictUsers()
        {
            return factory.ReadDictUsers();
        }
    }
}
