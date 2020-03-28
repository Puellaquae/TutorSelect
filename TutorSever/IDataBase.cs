namespace TutorSever
{
    public interface IDataBase
    {
        bool PassWordCheck(string username,string hmac,string type);
        bool IsFirstLogin(string username);
        bool HasAccount(string username);
        bool UpdatePassword(string username, string newpsw);
        string GetType(string username);
        string GetInformation(string username);
        string[] GetTutorInformations(string tutortype);
        bool UpdateInformation(string username, IAccount newinf);
        bool UploadPhoto(string username, byte[] blob);
    }
}