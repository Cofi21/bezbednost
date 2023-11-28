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
        bool KreirajNalog(User u);


        [OperationContract]
        bool IzdajKarticu();


        [OperationContract]
        bool PovuciSertifikat();

    }
}
