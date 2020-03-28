using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace TutorSever
{
    public interface IAccount
    {
        string UserName { get; set; }
        string RealName { get; set; }
        string HMAC { get; set; }
        string Type { get; }
        string GetInforamtion();

        public class Inf { }
    }

    internal class JsonSetting
    {
        private class LowerCaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name) => name.ToLower();
        }

        internal static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNamingPolicy = new LowerCaseNamingPolicy(),
            WriteIndented = true
        };
    }

    public class Student : IAccount
    {

        public string UserName { get; set; }

        public string RealName
        {
            get => _information.Name;
            set => _information.Name = value;
        }

        public string HMAC { get; set; }

        public string ID
        {
            get => _information.ID;
            set => _information.ID = value;
        }

        public string Major
        {
            get => _information.Major;
            set => _information.Major = value;
        }

        public string College
        {
            get => _information.College;
            set => _information.College = value;
        }

        public string SelfIntroduction
        {
            get => _information.SelfIntro;
            set => _information.SelfIntro = value;
        }

        public int Grade
        {
            get=>_information.Grade;
            set => _information.Grade = value;
        }

        public string Type => "Student";

        public string TutorType
        {
            get => _information.Choose;
            set
            {
                _information.Choose = value switch
                {
                    "under" => value,
                    "project" => value,
                    "all" => value,
                    _ => throw new Exception()
                };
            }
        }

        public string FirstTutor
        {
            get => _information.FirstTutor;
            set => _information.FirstTutor = value;
        }

        public string SecondTutor
        {
            get => _information.SecondTutor;
            set => _information.SecondTutor = value;
        }

        public bool IsFirstLogin { get; set; }
        private Inf _information = new Inf();

        public class Inf
        {
            public string Name { get; set; }
            public string ID { get; set; }
            public string Major { get; set; }
            public string College { get; set; }
            public string SelfIntro { get; set; }
            public string Choose { get; set; }
            public string FirstTutor { get; set; }
            public string SecondTutor { get; set; }
            public int Grade { get; set; }
        }

        public Student(string userName, string hmac)
        {
            UserName = userName;
            HMAC = hmac;
            IsFirstLogin = true;
        }

        public string GetInforamtion()
        {
            return JsonSerializer.Serialize(_information, JsonSetting.Options);
        }
    }

    public class Teacher : IAccount
    {
        public string UserName { get; set; }

        public string RealName
        {
            get => _information.Name;
            set => _information.Name = value;
        }

        public string ID
        {
            get => _information.ID;
            set => _information.ID = value;
        }

        public string Position
        {
            get => _information.Position;
            set => _information.Position = value;
        }

        public string College
        {
            get => _information.College;
            set => _information.College = value;
        }


        public string Group
        {
            get => _information.Group;
            set
            {
                _information.Group = value switch
                {
                    "under" => value,
                    "project" => value,
                    "all" => value,
                    _ => throw new Exception()
                };
            }
        }

        public string Field
        {
            get => _information.Field;
            set => _information.Field = value;
        }

        public string SelfIntroduction
        {
            get => _information.SelfIntro;
            set => _information.SelfIntro = value;
        }

        public string Achievement
        {
            get => _information.Achievement;
            set => _information.Achievement = value;
        }

        public string HMAC { get; set; }
        public string Type => "Teacher";
        private Inf _information = new Inf();

        public class Inf
        {
            public string Name { get; set; }
            public string ID { get; set; }
            public string Position { get; set; }
            public string College { get; set; }
            public string SelfIntro { get; set; }
            public string Field { get; set; }
            [JsonIgnore] public string Group { get; set; }
            public string Achievement { get; set; }
        }

        public Teacher(string userName, string hmac)
        {
            UserName = userName;
            HMAC = hmac;
        }

        public string GetInforamtion()
        {
            return JsonSerializer.Serialize(_information, JsonSetting.Options);
        }
    }

    public class Admin : IAccount
    {
        public string UserName { get; set; }

        public string RealName
        {
            get=>_information.Name;
            set => _information.Name = value;
        }
        public string HMAC { get; set; }
        public string Permission { get; set; }
        public string Type => "Administrator";
        private Inf _information=new Inf();
        public Admin(string userName, string hmac)
        {
            UserName = userName;
            HMAC = hmac;
        }

        public class Inf
        {
            public string Name { get; set; }
        }
        public string GetInforamtion()
        {
            return JsonSerializer.Serialize(_information, JsonSetting.Options);
        }
    }
}