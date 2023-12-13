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
    public interface IWin
    {
        [OperationContract]
        bool CreateAccount(byte[] recievedData, byte[] signature);

        [OperationContract]
        bool CreateMasterCardCertificate(string name, string pin);

        [OperationContract]
        bool PullAndCreateCertificate(string name);

    }
}
