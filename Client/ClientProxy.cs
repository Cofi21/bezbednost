using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientProxy : ChannelFactory<IMain>, IMain, IDisposable
    {
        IMain factory;
        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void Ispis()
        {
            Console.WriteLine("Izaberite zahtev: ");
            Console.WriteLine("\t1 - Kreiranje naloga");
            Console.WriteLine("\t2 - Povlacenje sertifikata ");
            Console.WriteLine("\t3 - Izvrsenje transakcije");
            Console.WriteLine("\t4 - Reset pin koda");
            Console.WriteLine("\t0 - Izlaz");
            Console.Write("Unesite Vas izbor: ");
            int broj = Int32.Parse(Console.ReadLine());
            Opcije(broj);
            
        }
        public void Opcije(int broj)
        {
            switch (broj)
            {
                case 1:
                    KreirajNalog();
                    break;
                case 2:
                    PovuciSertifikat();
                    break;
                case 3:
                    IzvrsiTransakciju();
                    break;
                case 4:
                    ResetujPinKod();
                    break;
                case 0:
                    return;
            }
        }
        public void KreirajNalog()
        {
            Console.Write("Unesite ime: ");
            string ime = Console.ReadLine();
            Console.Write("Unesite prezime: ");
            string prezime = Console.ReadLine();
            Console.Write("Unesite username: ");
            string username = Console.ReadLine();
            Console.Write("Unesite password: ");
            string password = Console.ReadLine();
            AddUser(username, password, ime, prezime);
            Ispis();
        }
        public void AddUser(string username, string password, string ime, string prezime)
        {
            try
            {
                factory.AddUser(username, password, ime, prezime);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
        
        public void PovuciSertifikat() { }
        public void IzvrsiTransakciju() { }
        public void ResetujPinKod() { }

    }
}
