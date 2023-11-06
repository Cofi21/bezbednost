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
        
         string username;
         string pin;
         string broj;


        public User() { }

        public User(string username, string pin, string broj)
        {
            this.username = username;
            this.pin = pin;
            this.broj = broj;
        }

         [DataMember]
        public string Username { get => username; set => username = value; }
          [DataMember]
        public string Pin { get => pin; set => pin = value; }
         [DataMember]
        public string Broj { get => broj; set => broj = value; }

        public override string ToString()
        {
            return $"Usename: {Username}, Naziv naloga: {Broj}";
        }

    }
}
