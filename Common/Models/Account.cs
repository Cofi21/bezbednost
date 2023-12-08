using System;
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
        // dodato zbog replikacije
        DateTime vremePoslednjeIzmene;

        [DataMember]
        public List<MasterCard> MasterCards { get; set; }

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
            MasterCards = new List<MasterCard>();
        }

        public Account(string brojRacuna, string pin, string username)
        {
            BrojRacuna = brojRacuna;
            Username = username;
            Stanje = 0;
            Pin = pin;
            MasterCards = new List<MasterCard>();
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
    }
}
