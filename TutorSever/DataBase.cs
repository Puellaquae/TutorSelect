using System;
using System.Collections.Generic;
using System.Linq;

namespace TutorSever
{
    internal class DataBase
    {
        private static readonly Dictionary<string, IAccount> Accounts = new Dictionary<string, IAccount>();

        public static bool PassWordCheck(string userName, string hmac, string type)
        {
            return Accounts[userName].HMAC == hmac && Accounts[userName].Type == type;
        }

        public static void Init()
        {
            Teacher teacher1 = new Teacher("123456", "30ce71a73bdd908c3955a90e8f7429ef")
            {
                RealName = "张三",
                ID = "T000001",
                College = "计算机学院",
                Field = "人工智能",
                Posititon = "教授",
                Achievement = "2020 No Award\r\nApplication of CNNs in Mahjong",
                SelfIntroduction = "Hello World!",
                Group = "all"
            };
            Accounts.Add("123456", teacher1); //123456
            Student student1 = new Student("201906", "9551179bec1582a6e1617aac41ab7b0f")
            {
                RealName = "李四",
                ID = "20200202",
                College = "计算机学院",
                Major = "计算机科学与技术",
                SelfIntroduction = "Dominae et Viri, I'm LiSi, LiSi is me."
            };
            Accounts.Add("201906", student1); //987654
            Admin admin1 = new Admin("admin1", "8be5be290b6c848e30e72367d38aa47a")
            {
                RealName = "王二麻子"
            };
            Accounts.Add("admin1", admin1); //abc123
        }

        public static bool FirstLoginCheck(string userName)
        {
            if (Accounts[userName] is Student a)
            {
                return a.IsFirstLogin;
            }
            else
            {
                return false;
            }
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
                if (Accounts[username] is Student a && a.IsFirstLogin)
                {
                    a.IsFirstLogin = false;
                }

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

        public static string GetInformation(string username)
        {
            return Accounts[username].GetInforamtion();
        }

        public static string[] GetTutorInformation(string tutorType)
        {
            IEnumerable<string> query = from x in Accounts.Values
                where x is Teacher a && (a.Group == tutorType || a.Group == "all")
                select x.GetInforamtion();
            return query.ToArray();
        }

        public static void UpdateInformation(string username)
        {
            throw new NotImplementedException();
        }
    }
}