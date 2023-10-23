using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var client = new ChannelFactory<IMain>("MainService"))
            {
                IMain proxy = client.CreateChannel();

                while (true)
                {
                    try
                    {
                        proxy.Ispis();
                        Console.ReadKey();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
                        
        }
    }
}
