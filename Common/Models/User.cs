using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class User
    {
        private int jmbg;
        private string ime;
        private string prezime;

        public User(int jmbg, string ime, string prezime)
        {
            this.jmbg = jmbg;
            this.ime = ime;
            this.prezime = prezime;
        }
        public User() { }

        public int JMBG { get => jmbg; set => jmbg = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }

        public override string ToString()
        {
            return $"{JMBG} {Ime} {Prezime}";
        }

    }
}
