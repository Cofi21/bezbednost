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
    public interface ICert
    {

        [OperationContract]
        bool ResetPinCode(byte[] encMess, byte[] signature);


        [OperationContract]
        bool IzvrsiTransakciju(byte[] transaction, byte[] signature, byte[] encPin);
    }
}
