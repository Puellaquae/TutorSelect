using System.Collections.Generic;

namespace TutorSever
{
    internal class DataBase
    {
        private class Account
        {
            internal string UserName { get; set; }
            internal string HMAC { get; set; }
            internal string Type { get; set; }
            internal bool FirstLogin { get; set; }
            internal Account(string username, string hmac, string type)
            {
                UserName = username;
                HMAC = hmac;
                Type = type;
                FirstLogin = Type == "student";
            }
        }

        private static readonly Dictionary<string, Account> Accounts = new Dictionary<string, Account>();
        public static bool PassWordCheck(string userName,string hmac,string type)
        {
            return Accounts[userName].HMAC == hmac && Accounts[userName].Type == type;
        }
        public static void Init()
        {
            Accounts.Add("123456", new Account("123456", "30ce71a73bdd908c3955a90e8f7429ef", "teacher"));//123456
            Accounts.Add("201906", new Account("201906", "9551179bec1582a6e1617aac41ab7b0f", "student"));//987654
            Accounts.Add("admin1", new Account("admin1", "8be5be290b6c848e30e72367d38aa47a", "admin"));//abc123
        }
        public static bool FirstLoginCheck(string userName)
        {
            return Accounts[userName].FirstLogin;
        }
        public static bool HasAccount(string userName)
        {
            return Accounts.ContainsKey(userName);
        }
        public static bool UpdatePassword(string username, string newpsw)
        {
            if (Accounts[username].HMAC != newpsw)
            {
                Accounts[username].HMAC = newpsw;
                if (Accounts[username].FirstLogin) Accounts[username].FirstLogin = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetType(string username)
        {
            return Accounts[username].Type;
        }
    }
}
