using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class User
    {
        private string username;
        private string password;
     //   private string ime;
       // private string prezime;
        private string broj;


        public User() { }

        public User(string username, string password, string broj)
        {
            this.username = username;
            this.password = password;
            this.broj = broj;
        }

      //  public string Ime { get => ime; set => ime = value; }
      //  public string Prezime { get => prezime; set => prezime = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Broj { get => broj; set => broj = value; }

        public override string ToString()
        {
            return $"Usename: {Username}, Naziv naloga: {Broj}";
        }

    }
}
