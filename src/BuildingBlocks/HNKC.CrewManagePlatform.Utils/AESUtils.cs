using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Utils
{
    public static class AESUtils
    {
        /// <summary>
        /// 默认Key 256
        /// </summary>
        const string KEY = @"LIf6HaF90YVHWWwqcHiMN9IpjkGPPm7Q";

        /// <summary>
        /// 默认偏移量
        /// </summary>
        const string IV = "BE56E057F20F883E";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Encrypt(string text, string key = KEY, string iv = IV)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            var bytes = Encoding.UTF8.GetBytes(text);
            var result = Origin(bytes, false, key, iv);

            return Convert.ToBase64String(result, 0, result.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Decrypt(string ciphertext, string key = KEY, string iv = IV)
        {
            if (string.IsNullOrEmpty(ciphertext))
                return string.Empty;
            var bytes = Convert.FromBase64String(ciphertext);
            var result = Origin(bytes, true, key, iv);

            if (result.Length < 1)
                return string.Empty;

            return Encoding.UTF8.GetString(result);
        }

        /// <summary>
        /// 底层方法
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="isDecrypt"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static byte[] Origin(byte[] bytes, bool isDecrypt, string key = KEY, string iv = IV)
        {
            try
            {
                var keyBytes = Encoding.UTF8.GetBytes(key);
                var ivBytes = Encoding.UTF8.GetBytes(iv);

                var rijndaelManaged = new RijndaelManaged
                {
                    BlockSize = 128,
                    KeySize = 256,
                    FeedbackSize = 128,
                    Padding = PaddingMode.PKCS7,
                    Key = keyBytes,
                    IV = ivBytes,
                    Mode = CipherMode.CBC
                };

                var cryptoTransform = isDecrypt ? rijndaelManaged.CreateDecryptor() : rijndaelManaged.CreateEncryptor();
                var result = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);

                return result;
            }
            catch
            {
                return new byte[] { };
            }
        }
    }
}
