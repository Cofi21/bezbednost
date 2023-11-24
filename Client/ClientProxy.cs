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
        public IMain factory;

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

        public void AddAccount(string username, string password, string broj, IMain factory)
        {
            try
            {
                if (Database.UserAccountsDB.ContainsKey(broj))
                {
                    bool message = false;
                    Console.WriteLine($"Nalog broj {broj} vec postoji!");
                    factory.Message(message, new User(username, password, broj));
                }
                else
                {
                    bool message = true;
                    Database.UserAccountsDB.Add(broj, new User(username, password, broj));
                    Console.WriteLine($"Korisnik je uspesno kreirao nalog broj: {broj}.");
                    factory.Message(message, new User(username, password, broj));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska! " + ex.Message + "\n" + ex.StackTrace);
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
                Console.WriteLine($"Korisnik je uspesno kreirao nalog broj: {u.Broj}.");
            }
            else
            {
                Console.WriteLine($"Nalog broj {u.Broj} vec postoji!");
            }
        }
    }
}
