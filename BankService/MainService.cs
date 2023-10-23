using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankService
{
    
    public class MainService : IMain
    {
        public void Ispis()
        {
            Console.WriteLine("Radi Klijent");
        }
    }
}
