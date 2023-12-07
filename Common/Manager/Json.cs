using Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Manager
{
    public class Json
    {

        public static void SaveAccountsToFile(Dictionary<string, Account> accountDictionary)
        {
            string filePath = "JsonDB/Accounts.json";
            string json = JsonConvert.SerializeObject(accountDictionary);

            File.WriteAllText(filePath, json);
        }

        public static Dictionary<string, Account> LoadAccountsFromFile()
        {
            string filePath = "JsonDB/Accounts.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Dictionary<string, Account> accountDictionary = JsonConvert.DeserializeObject<Dictionary<string, Account>>(json);
                return accountDictionary;
            }
            else
            {
                throw new FileNotFoundException("Fajl ne postoji.");
            }
        }

        public static void SaveMasterCardsToFile(List<MasterCard> masterCardList)
        {
            string filePath = "JsonDB/MasterCards.json";
            string json = JsonConvert.SerializeObject(masterCardList);

            File.WriteAllText(filePath, json);
        }

        public static List<MasterCard> LoadMasterCardsFromFile()
        {
            string filePath = "JsonDB/MasterCards.json";
            
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<MasterCard> masterCardList = JsonConvert.DeserializeObject<List<MasterCard>>(json);
                return masterCardList;
            }
            else
            {
                throw new FileNotFoundException("Fajl ne postoji.");
            }
        }

        public static void SaveUsersToFile(Dictionary<string, User> usersDictionary)
        {
            string filePath = "JsonDB/Users.json";
            string json = JsonConvert.SerializeObject(usersDictionary);

            File.WriteAllText(filePath, json);
        }

        public static Dictionary<string, User> LoadUsersFromFile()
        {
            string filePath = "JsonDB/Users.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Dictionary<string, User> usersDictionary = JsonConvert.DeserializeObject<Dictionary<string, User>>(json);
                return usersDictionary;
            }
            else
            {
                throw new FileNotFoundException("Fajl ne postoji.");
            }
        }
    }
}
