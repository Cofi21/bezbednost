﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class Account
    {
        private string brojRacuna;
        private string username;
        private double stanje;
        private string pin;

        [DataMember]
        public string BrojRacuna { get => brojRacuna; set => brojRacuna = value; }
        [DataMember]
        public string Username { get => username; set => username = value; }

        [DataMember]
        public double Stanje { get => stanje; set => stanje = value; }

        [DataMember]
        public string Pin { get => pin; set => pin = value; }

        

        public Account() 
        {
            Stanje = 0;
        }

        public Account(string brojRacuna, string pin, string username)
        {
            BrojRacuna = brojRacuna;
            Username = username;
            Stanje = 0;
            Pin = pin;
        }



        public override string ToString()
        {
            return $"Username: {Username}\nBroj racuna: {BrojRacuna}\nStanje: {Stanje}";
        }

        public override bool Equals(object obj)
        {
            return obj is Account account &&
                   brojRacuna == account.brojRacuna &&
                   username == account.username &&
                   stanje == account.stanje &&
                   pin == account.pin;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
