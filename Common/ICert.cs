﻿using System;
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
        void TestCommunication();

        [OperationContract]
        bool ResetujPinKod(string pin, string brojNaloga, byte[] signature);


        [OperationContract]
        bool IzvrsiTransakciju(int opcija, byte[] brojRacuna, byte[] svota, byte[] signature);
    }
}
