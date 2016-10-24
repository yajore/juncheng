using System;


namespace Lawyers.Utilities
{
    public class MD5Helper
    {
        public static string Encode(string md5Str)
        {
            var MD5CSP = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] MD5Source = System.Text.Encoding.UTF8.GetBytes(md5Str);
            byte[] MD5Out = MD5CSP.ComputeHash(MD5Source);
            return Convert.ToBase64String(MD5Out);
        }
    }
}
