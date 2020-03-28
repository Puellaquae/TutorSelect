using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TutorSever
{
    internal class DataBaseMemory:IDataBase
    {
        private readonly Dictionary<string, IAccount> _accounts = new Dictionary<string, IAccount>();

        public bool PassWordCheck(string userName, string hmac, string type)
        {
            return _accounts[userName].HMAC == hmac && _accounts[userName].Type == type;
        }

        public DataBaseMemory()
        {
            Teacher teacher1 = new Teacher("123456", "30ce71a73bdd908c3955a90e8f7429ef")
            {
                RealName = "张三",
                ID = "T000001",
                College = "计算机学院",
                Field = "人工智能",
                Position = "教授",
                Achievement = "2020 No Award\r\nApplication of CNNs in Mahjong",
                SelfIntroduction = "Hello World!",
                Group = "all"
            };
            _accounts.Add("123456", teacher1); //123456
            Student student1 = new Student("201906", "9551179bec1582a6e1617aac41ab7b0f")
            {
                RealName = "李四",
                ID = "20200202",
                College = "计算机学院",
                Major = "计算机科学与技术",
                SelfIntroduction = "Dominae et Viri, I'm LiSi, LiSi is me."
            };
            _accounts.Add("201906", student1); //987654
            Admin admin1 = new Admin("admin1", "8be5be290b6c848e30e72367d38aa47a")
            {
                RealName = "王二麻子"
            };
            _accounts.Add("admin1", admin1); //abc123
        }

        public bool IsFirstLogin(string username)
        {
            if (_accounts[username] is Student a)
            {
                return a.IsFirstLogin;
            }
            else
            {
                return false;
            }
        }

        public bool HasAccount(string username)
        {
            return _accounts.ContainsKey(username);
        }

        public bool UpdatePassword(string username, string newpsw)
        {
            if (_accounts[username].HMAC != newpsw)
            {
                _accounts[username].HMAC = newpsw;
                if (_accounts[username] is Student a && a.IsFirstLogin)
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

        public string GetType(string username)
        {
            return _accounts[username].Type;
        }

        public string GetInformation(string username)
        {
            return _accounts[username].GetInforamtion();
        }

        public string[] GetTutorInformations(string tutortype)
        {
            IEnumerable<string> query = from x in _accounts.Values
                where x is Teacher a && (a.Group == tutortype || a.Group == "all")
                select x.GetInforamtion();
            return query.ToArray();
        }

        public bool UpdateInformation(string username,IAccount newinf)
        {
            throw new NotImplementedException();
        }

        public bool UploadPhoto(string username, byte[] blob)
        {
            using var ms = new System.IO.MemoryStream(blob);
            Bitmap img = new Bitmap(ms);
            img.Save("D:\\test.bmp");
            return true;
        }
    }
}