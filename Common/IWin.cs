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
        void TestCommunication();


        [OperationContract]
        bool KreirajNalog(byte[] recievedData, byte[] signature);


        [OperationContract]
        bool PovuciSertifikat();


        [OperationContract]
        Dictionary<string, Account> ReadDict();

    }
}
