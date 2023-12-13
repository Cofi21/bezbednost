using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class IMDatabase
    {
        public static Dictionary<string, User> UsersDB = new Dictionary<string, User>();
        public static Dictionary<string, Account> AccountsDB = new Dictionary<string, Account>();
    }
}
