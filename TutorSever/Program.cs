using Puelloc;
using System;

namespace TutorSever
{
    internal class Program
    {
        private static void Main()
        {
            IDataBase dbBase=new DataBaseMemory();
            Procedure proc = new Procedure(dbBase);
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

            Pipe auth = new Pipe("GET", "/Auth", proc.Auth);
            Pipe firstlogin = new Pipe("GET", "/FirstLogin", proc.IsFirstLogin);
            Pipe updatePassword = new Pipe("GET", "/UpdatePassword", proc.UpdatePassword);
            Pipe login = new Pipe("GET", "/Login", proc.Login);
            Pipe getInf = new Pipe("GET", "/Inf", proc.GetInformation);
            Pipe updateInf = new Pipe("POST", "/UpdateInf", proc.UpdateInformation);
            Pipe getTutorInf = new Pipe("GET", "/TutorInf", proc.GetTutorInformation);
            Pipe uploadPic =new Pipe("PUT","/Pic",proc.UploadPic);
            Pipe _TESTuploadPic =new Pipe("PUT","/testPic",proc.TESTUploadPic);
            Pipe downloadPic =new Pipe("GET","/Pic",proc.DownloadPic);

            HttpClient httpClient = new HttpClient(setting,
                auth, firstlogin, updatePassword, login,
                getInf, updateInf, getTutorInf, uploadPic, downloadPic, _TESTuploadPic);
            httpClient.Listen();
            Console.WriteLine(setting.BindIP.ToString());

            while (Console.ReadLine() != "shutdown")
            {
            }

            httpClient.Stop();
        }
    }
}