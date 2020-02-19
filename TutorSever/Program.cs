using Puelloc;
using System;
using System.Collections.Generic;

namespace TutorSever
{
    internal class Program
    {
        private static ResponseMessage Auth(RequsetMessage request)
        {
            Dictionary<string, string> query = request.Querys;
            if (query.ContainsKey("user") && query.ContainsKey("pwd") && query.ContainsKey("type"))
            {
                string token = Authority.GetToken(query["user"], query["pwd"], query["type"]);
                return new ResponseMessage(token);
            }
            else
            {
                return new ResponseMessage(400);
            }
        }

        private static ResponseMessage IsFirstLogin(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.Querys;
            if (query.ContainsKey("user")&& query.ContainsKey("token"))
            {
                string user = query["user"];
                string token = query["token"];
                if(Authority.CheckToken(user, token))
                {
                    return DataBase.FirstLoginCheck(user) ? new ResponseMessage("true") : new ResponseMessage("false");
                }
                else
                {
                    return new ResponseMessage(400);
                }
            }
            else
            {
                return new ResponseMessage(400);
            }
        }

        private static ResponseMessage UpdatePassword(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.Querys;
            if (query.ContainsKey("user") && query.ContainsKey("token") && query.ContainsKey("newpsw"))
            {
                string user = query["user"];
                string token = query["token"];
                string newpsw = query["newpsw"];
                if (Authority.CheckToken(user, token))
                {
                    return DataBase.UpdatePassword(user,newpsw) ? new ResponseMessage("true") : new ResponseMessage("false");
                }
                else
                {
                    return new ResponseMessage(400);
                }
            }
            else
            {
                return new ResponseMessage(400);
            }
        }

        private static ResponseMessage Login(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.Querys;
            if (query.ContainsKey("user") && query.ContainsKey("token"))
            {
                string token = query["token"];
                string username = query["username"];
                if (Authority.CheckToken(username, token))
                {
                    string type = DataBase.GetType(username);
                    return type switch
                    {
                        "student" => ResponseMessage.TryGetFileResponse("student/index.html"),
                        "teacher" => ResponseMessage.TryGetFileResponse("teacher/index.html"),
                        "admin" => ResponseMessage.TryGetFileResponse("admin/index/html"),
                        _ => new ResponseMessage("UNKOWN TYPE", 406)
                    };
                }
                else
                {
                    return new ResponseMessage(403);
                }
            }
            else
            {
                return new ResponseMessage(400);
            }
        }

        private static void Main()
        {
            
            Authority.Init();
            Pipe auth = new Pipe((method, url) => method == "GET" && url.StartsWith("/Auth"), Auth);
            Pipe firstlogin = new Pipe((method, url) => method == "GET" && url.StartsWith("/FirstLogin"), IsFirstLogin);
            Pipe updatePassword = new Pipe((method, url) => method == "GET" && url.StartsWith("/UpdatePassword"),
                UpdatePassword);
            Pipe login = new Pipe((method, url) => method == "GET" && url.StartsWith("/Login"), Login);
            Setting setting = new Setting();
            OperatingSystem osInfo = Environment.OSVersion;
            PlatformID platformID = osInfo.Platform;
            Console.WriteLine(osInfo.ToString());
            if (platformID == PlatformID.Unix)
            {
                setting.SetBindIP("127.0.0.1", 1516);
            }
            else if (platformID == PlatformID.Win32NT)
            {
                setting.BasePath = @"F:\Administrator\Documents\code\TutorSelect\TutorSever";
            }

            HttpClient httpClient = new HttpClient(setting, auth, firstlogin, updatePassword, login);
            httpClient.Listen();
            Console.WriteLine(setting.BindIP.ToString());
            while (Console.ReadLine()!="shutdown")
            {
            }

            httpClient.Stop();
        }
    }
}