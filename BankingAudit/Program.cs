using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankingAudit
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("BankingAudit system is started.\nPress <enter> to stop ...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
