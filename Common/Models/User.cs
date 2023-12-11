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
        private bool haveCertificate;

        public User() { }

        public User(string username, bool haveCertificate)
        {
            Username = username;
            HaveCertificate = haveCertificate;
        }

        public string Username { get => username; set => username = value; }
        public bool HaveCertificate { get => haveCertificate; set => haveCertificate = value; }

        public override string ToString()
        {
            return "Username: " + Username + "\n"; 
        }
    }
}
