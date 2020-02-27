using Puelloc;
using System;

namespace TutorSever
{
    internal class Program
    {
        private static void Main()
        {
            Authority.Init();
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

            Pipe auth = new Pipe("GET", "/Auth", Procedure.Auth);
            Pipe firstlogin = new Pipe("GET", "/FirstLogin", Procedure.IsFirstLogin);
            Pipe updatePassword = new Pipe("GET", "/UpdatePassword", Procedure.UpdatePassword);
            Pipe login = new Pipe("GET", "/Login", Procedure.Login);
            Pipe getInf = new Pipe("GET", "/Inf", Procedure.GetInformation);
            Pipe updateInf = new Pipe("POST", "/UpdateInf", Procedure.UpdateInformation);
            Pipe getTutorInf = new Pipe("GET", "/TutorInf", Procedure.GetTutorInformation);
            Pipe uploadPic =new Pipe("PUT","/Pic",Procedure.UploadPic);
            Pipe downloadPic =new Pipe("GET","/Pic",Procedure.DownloadPic);
            HttpClient httpClient = new HttpClient(setting,
                auth, firstlogin, updatePassword, login,
                getInf, updateInf, getTutorInf,uploadPic,downloadPic);
            httpClient.Listen();
            Console.WriteLine(setting.BindIP.ToString());
            while (Console.ReadLine() != "shutdown")
            {
            }

            httpClient.Stop();
        }
    }
}