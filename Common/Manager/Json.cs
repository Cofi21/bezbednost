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

        public static void SaveDictionaryToFile(Dictionary<string, Account> accountDictionary, string filePath)
        {
            string json = JsonConvert.SerializeObject(accountDictionary);

            // Čuvanje JSON podataka u fajlu
            File.WriteAllText(filePath, json);
        }

        public static Dictionary<string, Account> LoadDictionaryFromFile(string filePath)
        {
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

        public static void SaveListToFile(List<MasterCard> masterCardList, string filePath)
        {
            string json = JsonConvert.SerializeObject(masterCardList);

            // Čuvanje JSON podataka u fajlu
            File.WriteAllText(filePath, json);
        }

        // Metoda za učitavanje liste MasterCard objekata iz JSON fajla
        public static List<MasterCard> LoadListFromFile(string filePath)
        {
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

        public static void SaveUserToFile(User user, string filePath)
        {
            string json = JsonConvert.SerializeObject(user);

            // Čuvanje JSON podataka u fajlu
            File.WriteAllText(filePath, json);
        }

        public static User LoadUserFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                User user = JsonConvert.DeserializeObject<User>(json);
                return user;
            }
            else
            {
                throw new FileNotFoundException("Fajl ne postoji.");
            }
        }
    }
}
