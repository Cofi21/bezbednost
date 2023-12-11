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
            string filePath = "..//..//..//BankService/bin/debug/JsonDB/Accounts.json";
            string json = JsonConvert.SerializeObject(accountDictionary);

            File.WriteAllText(filePath, json);
        }

        public static Dictionary<string, Account> LoadAccountsFromFile()
        {
            string filePath = "..//..//..//BankService/bin/debug/JsonDB/Accounts.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    Dictionary<string, Account> accountDictionary = JsonConvert.DeserializeObject<Dictionary<string, Account>>(json);
                    return accountDictionary;
                }
                else
                {
                    return new Dictionary<string, Account>();
                }
            }
            else
            {
                throw new FileNotFoundException("File does not exist.");
            }
        }

        public static void SaveMasterCardsToFile(List<MasterCard> masterCardList)
        {
            string filePath = "..//..//..//BankService/bin/debug/JsonDB/MasterCards.json";
            string json = JsonConvert.SerializeObject(masterCardList);

            File.WriteAllText(filePath, json);
        }

        public static List<MasterCard> LoadMasterCardsFromFile()
        {
            string filePath = "..//..//..//BankService/bin/debug/JsonDB/MasterCards.json";
            
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    List<MasterCard> masterCardList = JsonConvert.DeserializeObject<List<MasterCard>>(json);
                    return masterCardList;
                }
                else
                {
                    return new List<MasterCard>();
                }

            }
            else
            {
                throw new FileNotFoundException("Fajl ne postoji.");
            }
        }

        public static void SaveUsersToFile(Dictionary<string, User> userDictionary)
        {
            string filePath = "..//..//..//BankService/bin/debug/JsonDB/Users.json";
            string json = JsonConvert.SerializeObject(userDictionary);

            File.WriteAllText(filePath, json);
        }

        public static Dictionary<string, User> LoadUsersFromFile()
        {
            string filePath = "..//..//..//BankService/bin/debug/JsonDB/Users.json";
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    Dictionary<string, User> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, User>>(json);
                    return userDictionary;
                }
                else
                {
                    return new Dictionary<string, User>();
                }
            }
            else
            {
                throw new FileNotFoundException("File does not exist.");
            }
        }

    }
}
