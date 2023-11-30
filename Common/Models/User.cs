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
        private string username;
        public Dictionary<string, Account> userAccounts { get; set; }


        public User() {}

        public User(string username, MasterCard master)
        {
            this.username = username;
            MasterCardProp = master;
            this.userAccounts = new Dictionary<string, Account>();
        }


        [DataMember]
        public string Username { get => username; set => username = value; }

        public override string ToString()
        {
            return $"Username: {username}, naziv naloga: {MasterCardProp.SubjectName}";
        }

    }
}
