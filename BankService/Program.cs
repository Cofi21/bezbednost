using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankService
{
    class Program
    {
        static void Main(string[] args)
        {
            MainService ms = new MainService();

            using (ServiceHost host = new ServiceHost(typeof(MainService)))
            {
                host.Open();
                Console.WriteLine("Server je pokrenut...");
                Console.WriteLine("Pritisnite [Enter] za zaustavljanje servera!");
                Console.ReadKey();
                host.Close();
            }
        }
    }
}
