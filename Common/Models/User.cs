using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class User
    {
        private string broj;
        private double stanje;
        private MasterCard masterCard;


        public User() { }

        public User(string username, string broj, MasterCard master)
        {
            this.stanje = 0;
            this.broj = broj;
            MasterCardProp = master;
        }

        [DataMember]
        public MasterCard MasterCardProp { get => masterCard; set => masterCard = value; }

        [DataMember]
        public string Broj { get => broj; set => broj = value; }
        
        [DataMember]
        public double Stanje { get => stanje; set => stanje = value; }

        public override string ToString()
        {
            return $"Broj naloga: {Broj}, naziv naloga: {MasterCardProp.SubjectName}";
        }

    }
}
