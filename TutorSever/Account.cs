
using Microsoft.VisualBasic;

namespace TutorSever
{
    public interface IAccount
    {
        string UserName { get; set; }
        string RealName { get; set; }
        string HMAC { get; set; }
        string Type { get; }
        string GetInforamtion();
    }

    public class Student : IAccount
    {
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string HMAC { get; set; }
        public string ID { get; set; }
        public string Major { get; set; }
        public string College { get; set; }
        public string SelfIntroduction { get; set; }
        public string Type => "student";
        public string TutorType;
        public string FirstTutor { get; set; }
        public string SecondTutor { get; set; }
        public bool IsFirstLogin { get; set; }

        public Student(string userName, string hmac)
        {
            UserName = userName;
            HMAC = hmac;
            IsFirstLogin = true;
        }

        public string GetInforamtion()
        {
            return $"{{\"name\":\"{RealName}\"," +
                   $"\"id\":\"{ID}\"," +
                   $"\"major\":\"{Major}\"," +
                   $"\"college\":\"{College}\"," +
                   $"\"selfintro\":\"{SelfIntroduction}\"," +
                   $"\"choose\":\"{TutorType}\"," +
                   $"\"firsttutor\":\"{FirstTutor}\"," +
                   $"\"secondtutor\":\"{SecondTutor}\"}}";
        }
    }

    public class Teacher : IAccount
    {
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string ID { get; set; }
        public string Posititon { get; set; }
        public string College { get; set; }

        /// <summary>
        /// Could be "under", "project" or "all"
        /// </summary>
        public string Group { get; set; }

        public string Field { get; set; }
        public string SelfIntroduction { get; set; }
        public string Achievement { get; set; }
        public string HMAC { get; set; }
        public string Type => "teacher";

        public Teacher(string userName, string hmac)
        {
            UserName = userName;
            HMAC = hmac;
        }

        public string GetInforamtion()
        {
            return $"{{\"name\":\"{RealName}\"," +
                   $"\"id\":\"{ID}\"," +
                   $"\"posititon\":\"{Posititon}\"," +
                   $"\"college\":\"{College}\"," +
                   $"\"field\":\"{Field}\"," +
                   $"\"achievement\":\"{Achievement}\"," +
                   $"\"selfintro\":\"{SelfIntroduction}\"}}";
        }
    }

    public class Admin : IAccount
    {
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string HMAC { get; set; }
        public string Type => "admin";

        public Admin(string userName, string hmac)
        {
            UserName = userName;
            HMAC = hmac;
        }

        public string GetInforamtion()
        {
            throw new System.NotImplementedException();
        }
    }
}