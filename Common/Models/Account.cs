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
        private double stanje;
        private string pin;
        // dodato zbog replikacije
        DateTime vremePoslednjeIzmene;

        [DataMember]
        public List<MasterCard> MasterCards { get; set; }

        [DataMember]
        public string BrojRacuna { get => brojRacuna; set => brojRacuna = value; }

        [DataMember]
        public double Stanje { get => stanje; set => stanje = value; }

        [DataMember]
        public string Pin { get => pin; set => pin = value; }

        

        public Account() 
        {
            Stanje = 0;
            MasterCards = new List<MasterCard>();
        }

        public Account(string brojRacuna, string pin)
        {
            BrojRacuna = brojRacuna;
            Stanje = 0;
            Pin = pin;
            MasterCards = new List<MasterCard>();
        }

        public override bool Equals(object obj)
        {
            return obj is Account account &&
                   brojRacuna == account.brojRacuna &&
                   stanje == account.stanje &&
                   BrojRacuna == account.BrojRacuna &&
                   Stanje == account.Stanje;
        }

        public override int GetHashCode()
        {
            int hashCode = -1499122616;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(brojRacuna);
            hashCode = hashCode * -1521134295 + stanje.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BrojRacuna);
            hashCode = hashCode * -1521134295 + Stanje.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return "Broj racuna: " + BrojRacuna + "\n" + "Stanje: " + Stanje;
        }
    }
}
