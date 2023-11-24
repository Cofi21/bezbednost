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
    public class ClientProxy : ChannelFactory<IMain>, IMain, IDisposable
    {
        IMain factory;

        public ClientProxy()
        {
        }

        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
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

        public void Message(bool message, User u)
        {
            if (message)
            {
                Database.UserAccountsDB.Add(u.Broj, u);
                Console.WriteLine($"Korisnik je uspesno kreirao nalog broj: {u.Broj}.");
            }
            else
            {
                Console.WriteLine($"Nalog broj {u.Broj} vec postoji!");
            }
        }
    }
}
