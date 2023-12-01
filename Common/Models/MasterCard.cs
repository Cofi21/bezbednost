using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class MasterCard
    {
        private string subjectName;
        private string pin;

        public MasterCard(string subjectName, string pin)
        {
            this.subjectName = subjectName;
            this.pin = pin;
        }

        [DataMember]
        public string SubjectName { get => subjectName; set => subjectName = value; }
        [DataMember]
        public string Pin { get => pin; set => pin = value; }

        public override string ToString()
        {
            return "Subject name: " + SubjectName;
        }
    }
}
