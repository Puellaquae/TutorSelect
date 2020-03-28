using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace TutorSever
{
    internal class DataBaseSQLite:IDataBase
    {
        private readonly string _dbPath;
        public DataBaseSQLite(string dbPath)
        {
            _dbPath = dbPath;
        }

        public bool PassWordCheck(string username, string hmac, string type)
        {
            using SQLiteConnection connection = new SQLiteConnection($"Data Source={_dbPath}");
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText =
                @"
                        SELECT UserName FROM Student WHERE UserName = $user AND HMAC = $hmac
                        UNION 
                        SELECT UserName FROM Teacher WHERE UserName = $user AND HMAC = $hmac
                        UNION 
                        SELECT UserName FROM Administrator WHERE UserName = $user AND HMAC = $hmac
                ";
            command.Parameters.AddWithValue("$user", username);
            command.Parameters.AddWithValue("$hmac", hmac);
            using SQLiteDataReader reader = command.ExecuteReader();
            return reader.FieldCount != 0;
        }

        public bool IsFirstLogin(string username)
        {
            using SQLiteConnection connection = new SQLiteConnection($"Data Source={_dbPath}");
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText =
                @"
                        SELECT IsFirstLogin FROM Student WHERE UserName = $user
                ";
            command.Parameters.AddWithValue("$user", username);
            using SQLiteDataReader reader = command.ExecuteReader();
            return reader.GetBoolean(0);
        }

        public bool HasAccount(string username)
        {
            using SQLiteConnection connection = new SQLiteConnection($"Data Source={_dbPath}");
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText =
                @"
                        SELECT UserName FROM Student WHERE UserName = $user
                        UNION 
                        SELECT UserName FROM Teacher WHERE UserName = $user
                        UNION 
                        SELECT UserName FROM Administrator WHERE UserName = $user
                ";
            command.Parameters.AddWithValue("$user", username);
            using SQLiteDataReader reader = command.ExecuteReader();
            return reader.FieldCount != 0;
        }

        public bool UpdatePassword(string username, string newpsw)
        {
            using SQLiteConnection connection = new SQLiteConnection($"Data Source={_dbPath}");
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText =
                @"
                        UPDATE Student SET HMAC = $newhmac, IsFirstLogin = 0 WHERE UserName = $user;
                        UPDATE Teacher SET HMAC = $newhmac WHERE UserName = $user;
                        UPDATE Administrator SET HMAC = $newhmac WHERE UserName = $user;
                ";
            command.Parameters.AddWithValue("$user", username);
            command.Parameters.AddWithValue("$newhmac", newpsw);
            int ans = command.ExecuteNonQuery();
            return ans != 0;
        }

        public string GetType(string username)
        {
            using SQLiteConnection connection = new SQLiteConnection($"Data Source={_dbPath}");
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText =
                @"
                        SELECT $map FROM Student WHERE UserName = $user
                ";
            command.Parameters.AddWithValue("$user", username);
            foreach(string x in new string[]{"Teacher","Student","Administrator" }){
                command.Parameters.AddWithValue("$map",x);
                using SQLiteDataReader reader = command.ExecuteReader();
                if(reader.FieldCount != 0){
                    return x;
                } 
            }
            throw new Exception($"NOT FOUND ACCOUNT {username} IN GetType()");
        }

        public string GetInformation(string username)
        {
            throw new NotImplementedException();
        }

        public string[] GetTutorInformations(string tutortype)
        {
            throw new NotImplementedException();
        }

        public bool UpdateInformation(string username, IAccount newinf)
        {
            throw new NotImplementedException();
        }

        public bool UploadPhoto(string username, byte[] blob)
        {
            throw new NotImplementedException();
        }
    }
}