using Common;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Replikator
{
    public class BackupWin : ChannelFactory<IReplikator>, IReplikator, IDisposable
    {

        IReplikator factory;

        public BackupWin(NetTcpBinding binding, string address) : base(binding, address)
        {
            try
            {
                factory = this.CreateChannel();
            }
            catch (Exception ex)
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

        public void Send(List<Account> studenti)
        {
            factory.Send(studenti);
        }

        public List<Account> Collect()
        {
            return factory.Collect();
        }
    }
}
