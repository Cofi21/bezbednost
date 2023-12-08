using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class Transaction
    {
        private int akcija;
        private string brojRacuna;
        private double svota;

        public Transaction(int akcija, string brojRacuna, double svota)
        {
            this.akcija = akcija;
            this.brojRacuna = brojRacuna;
            this.svota = svota;
        }
        [DataMember]
        public int Akcija { get => akcija; set => akcija = value; }
        [DataMember]
        public string BrojRacuna { get => brojRacuna; set => brojRacuna = value; }
        [DataMember]
        public double Svota { get => svota; set => svota = value; }

        public override string ToString()
        {
            return Akcija + "|" + BrojRacuna + "|" + Svota;
        }
    }
}
