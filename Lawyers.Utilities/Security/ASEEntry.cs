using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lawyers.Utilities
{

    public class ASEDecrypt
    {
        public static string key = ConfigurationManager.AppSettings["queryString_key"];

        /// <summary>
        /// 2进制转Hex
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// Hex 转 2进制
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptStr">待加密字符串</param>
        /// <returns>hex</returns>
        public static string Encrypt(string encryptStr)
        {
            var toEncryptArray = Encoding.UTF8.GetBytes(encryptStr);
            var rDel = new RijndaelManaged();
            rDel.Key = Encoding.UTF8.GetBytes(key);
            rDel.IV = new byte[16];
            rDel.Mode = CipherMode.CBC;

            using (var cTransform = rDel.CreateEncryptor())
            {
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return byteToHexStr(resultArray).ToLower();
            }

        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptStr">加密hex字符串</param>
        /// <returns></returns>
        public static string Decrypt(string encryptStr)
        {
            string plaintext = String.Empty;
            try
            {

                var toEncryptArray = strToToHexByte(encryptStr);
                var rijalg = new RijndaelManaged();
                rijalg.Key = Encoding.UTF8.GetBytes(key);
                rijalg.IV = new byte[16];
                rijalg.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(toEncryptArray))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteException("【解密失败】", ex, encryptStr);
            }

            return plaintext;
        }

    }

}
