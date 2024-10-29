using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.Utils
{
    public static class MD5Util
    {

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(Stream fileStream)
        {
            var cryptBytes = MD5.HashData(fileStream);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < cryptBytes.Length; i++)
            {
                sb.Append(cryptBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            byte[] hashBytes = MD5.HashData(inputBytes);

            // 将哈希值转换为十六进制字符串  
            StringBuilder sb = new();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            // 返回最终的哈希字符串  
            return sb.ToString().ToLower();
        }

    }
}
