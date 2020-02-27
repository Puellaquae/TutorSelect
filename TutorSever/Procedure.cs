using System;
using System.Collections.Generic;
using Puelloc;

namespace TutorSever
{
    internal class Procedure
    {
        internal static ResponseMessage Auth(RequsetMessage request)
        {
            Dictionary<string, string> query = request.UrlQuerys;
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

        internal static ResponseMessage IsFirstLogin(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("token"))
            {
                string user = query["user"];
                string token = query["token"];
                if (Authority.CheckToken(user, token))
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

        internal static ResponseMessage DownloadPic(RequsetMessage requset)
        {
            throw new NotImplementedException();
        }

        internal static ResponseMessage UploadPic(RequsetMessage requset)
        {
            throw new NotImplementedException();
        }

        internal static ResponseMessage UpdatePassword(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("token") && query.ContainsKey("newpsw"))
            {
                string user = query["user"];
                string token = query["token"];
                string newpsw = query["newpsw"];
                if (Authority.CheckToken(user, token))
                {
                    return DataBase.UpdatePassword(user, newpsw)
                        ? new ResponseMessage("true")
                        : new ResponseMessage("false");
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

        internal static ResponseMessage Login(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("token"))
            {
                string token = query["token"];
                string username = query["user"];
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

        internal static ResponseMessage GetInformation(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("token"))
            {
                string token = query["token"];
                string username = query["user"];
                if (Authority.CheckToken(username, token))
                {
                    return new ResponseMessage(DataBase.GetInformation(username));
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

        internal static ResponseMessage UpdateInformation(RequsetMessage requset)
        {
            throw new NotImplementedException();
        }

        internal static ResponseMessage GetTutorInformation(RequsetMessage requset)
        {

            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("token") && query.ContainsKey("type"))
            {
                string token = query["token"];
                string username = query["user"];
                string type = query["type"];
                if (Authority.CheckToken(username, token))
                {
                    return new ResponseMessage(string.Join("\r\n", DataBase.GetTutorInformation(type)));
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
    }
}