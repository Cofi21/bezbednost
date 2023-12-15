using Common.Manager;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IBankingAudit
    {
        [OperationContract]
        // AccessingLog treba da primi objekat
        void AccessingLog(TransactionPayments tp);
    }
}
