using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;

namespace GHMonitoringCenterApi.Domain.Shared
{


    /// <summary>
    /// 字符串加解密扩展
    /// </summary>
    public class CryptoStringExtension
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes(AppsettingsHelper.GetValue("AESCeypto:Key")); 
        private static readonly byte[] IV = Encoding.UTF8.GetBytes(AppsettingsHelper.GetValue("AESCeypto:IV"));  
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string EncryptAsync(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);

                return Convert.ToBase64String(encryptedBytes);
            }
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public static string DecryptAsync(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public static bool TextAuth(string encryptedText)
        {
            var flag=false;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    byte[] bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"可能存在非法请求:{encryptedText}");
            }
            return flag;
        }
    }
}
