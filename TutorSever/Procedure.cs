using System;
using System.Collections.Generic;
using Puelloc;

namespace TutorSever
{
    internal class Procedure
    {
        private readonly IDataBase _db;
        private readonly Authority _auth;
        public Procedure(IDataBase db)
        {
            _db = db;
            _auth = new Authority(db);
        }

        internal ResponseMessage Auth(RequsetMessage request)
        {
            Dictionary<string, string> query = request.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("pwd") && query.ContainsKey("type"))
            {
                string token = _auth.GetToken(query["user"], query["pwd"], query["type"]);
                return new ResponseMessage(token);
            }
            else
            {
                return new ResponseMessage(400);
            }
        }

        internal ResponseMessage IsFirstLogin(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("token"))
            {
                string user = query["user"];
                string token = query["token"];
                if (_auth.CheckToken(user, token))
                {
                    return _db.IsFirstLogin(user) ? new ResponseMessage("true") : new ResponseMessage("false");
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

        internal ResponseMessage DownloadPic(RequsetMessage requset)
        {
            throw new NotImplementedException();
        }

        internal ResponseMessage UploadPic(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("token"))
            {
                string token = query["token"];
                string username = query["user"];
                if (_auth.CheckToken(username, token))
                {

                    if (_db.UploadPhoto(username,
                        Convert.FromBase64String(
                            requset.Text
                            .Replace("data:image/png;base64,", "")
                            .Replace("data:image/jgp;base64,", "")
                            .Replace("data:image/jpg;base64,", "")
                            .Replace("data:image/jpeg;base64,", ""))))
                    {
                        return new ResponseMessage(200);
                    }
                    else
                    {
                        return new ResponseMessage(500);
                    }
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

        internal ResponseMessage TESTUploadPic(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user"))
            {
                string username = query["user"];
                if (_db.UploadPhoto(username,
                    Convert.FromBase64String(
                            requset.Text
                            .Replace("data:image/png;base64,", "")
                            .Replace("data:image/jgp;base64,", "")
                            .Replace("data:image/jpg;base64,", "")
                            .Replace("data:image/jpeg;base64,", ""))))
                {
                    return new ResponseMessage(200);
                }
                else
                {
                    return new ResponseMessage(500);
                }
            }
            else
            {
                return new ResponseMessage(400);
            }
        }

        internal ResponseMessage UpdatePassword(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("token") && query.ContainsKey("newpsw"))
            {
                string user = query["user"];
                string token = query["token"];
                string newpsw = query["newpsw"];
                if (_auth.CheckToken(user, token))
                {
                    return _db.UpdatePassword(user, newpsw)
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

        internal ResponseMessage Login(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("token"))
            {
                string token = query["token"];
                string username = query["user"];
                if (_auth.CheckToken(username, token))
                {
                    string type = _db.GetType(username);
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

        internal ResponseMessage GetInformation(RequsetMessage requset)
        {
            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("token"))
            {
                string token = query["token"];
                string username = query["user"];
                if (_auth.CheckToken(username, token))
                {
                    return new ResponseMessage(_db.GetInformation(username));
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

        internal ResponseMessage UpdateInformation(RequsetMessage requset)
        {
            throw new NotImplementedException();
        }

        internal ResponseMessage GetTutorInformation(RequsetMessage requset)
        {

            Dictionary<string, string> query = requset.UrlQuerys;
            if (query.ContainsKey("user") && query.ContainsKey("token") && query.ContainsKey("type"))
            {
                string token = query["token"];
                string username = query["user"];
                string type = query["type"];
                if (_auth.CheckToken(username, token))
                {
                    return new ResponseMessage("{[" + string.Join(",", _db.GetTutorInformations(type)) + "]}");
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