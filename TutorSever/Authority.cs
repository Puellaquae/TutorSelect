using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TutorSever
{
    internal class AES
    {
        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>
        public static string AESEncrypt(string sourceStr, string cryptoKey)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
            byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
            aes.Key = key;
            aes.IV = iv;
            byte[] dataByteArray = Encoding.UTF8.GetBytes(sourceStr);
            using MemoryStream ms = new MemoryStream();
            using CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(dataByteArray, 0, dataByteArray.Length);
            cs.FlushFinalBlock();
            string encrypt = Convert.ToBase64String(ms.ToArray());
            return encrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <returns>返回解密后的明文字符串</returns>
        public static string AESDecrypt(string sourceStr, string cryptoKey)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] key = sha256.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
            byte[] iv = md5.ComputeHash(Encoding.UTF8.GetBytes(cryptoKey));
            aes.Key = key;
            aes.IV = iv;

            byte[] dataByteArray = Convert.FromBase64String(sourceStr);
            using MemoryStream ms = new MemoryStream();
            using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(dataByteArray, 0, dataByteArray.Length);
            cs.FlushFinalBlock();
            string decrypt = Encoding.UTF8.GetString(ms.ToArray());
            return decrypt;
        }
    }

    internal static class Authority
    {
        private static int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0); 
        }
        private static string GetRandomString(int len)
        {
            string s = "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";
            string reValue = string.Empty;
            Random rnd = new Random(GetNewSeed());
            while (reValue.Length < len)
            {
                string s1 = s[rnd.Next(0, s.Length)].ToString();
                if (reValue.IndexOf(s1, StringComparison.Ordinal) == -1) reValue += s1;
            }
            return reValue;
        }
        private static bool _inited;
        private static string _tokenGenKey;
        public static void Init()
        {
            _tokenGenKey = GetRandomString(16);
            DataBase.Init();
            _inited = true;
        }
        //private static Dictionary<string, string> Tokens = new Dictionary<string, string>();
        public static string GetToken(string userName,string hmac,string type)
        {
            if (DataBase.PassWordCheck(userName, hmac, type))
            {
                string token = TokenGen(userName);
                //Tokens.Add(token, UserName);
                return token;
            }
            else
            {
                return "false";
            }
        }

        private static string GetMD5(string text)
        {
            using MD5 md5 = MD5.Create();
            byte[] source = Encoding.Default.GetBytes(text);
            byte[] crypto = md5.ComputeHash(source);
            string result = Convert.ToBase64String(crypto);
            return result;
        }

        private static string TokenGen(string userName)
        {
            if (_inited == false|| _tokenGenKey == null)
            {
                Init();
            }
            string res = userName + DateTime.Today.ToString("d");
            string cry = AES.AESEncrypt(res, _tokenGenKey);
            return System.Web.HttpUtility.UrlEncode(cry);
        }
        public static bool CheckToken(string userName,string token)
        {
            if (DataBase.HasAccount(userName))
            {
                switch (token.Length % 4)
                {
                    case 2:
                        token += "==";
                        break;
                    case 3:
                        token += "=";
                        break;
                }
                string retoken = AES.AESDecrypt(token,_tokenGenKey);
                return retoken == userName + DateTime.Today.ToString("d");
            }
            else
            {
                return false;
            }
        }
    }
}
