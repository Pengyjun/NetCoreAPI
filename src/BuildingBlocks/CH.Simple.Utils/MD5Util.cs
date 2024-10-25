using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CH.Simple.Utils
{
    public static class MD5Util
    {
        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] res = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            string hash = BitConverter.ToString(res).Replace("-", "");
            return hash.ToLower();
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(Stream fileStream)
        {
            MD5 md5 = MD5.Create();
            byte[] cryptBytes = md5.ComputeHash(fileStream);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < cryptBytes.Length; i++)
            {
                sb.Append(cryptBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

    }
}
