using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CH.Simple.Utils
{
    public static class RSAUtils
    {

        #region 基本信息
        //var rsa = new RSACryptoServiceProvider();
        //var publicKey = rsa.ToXmlString(false);
        //var privateKey = rsa.ToXmlString(true);

        /// <summary>
        /// PUBLIC KEY
        /// </summary>
        const string PUBLIC_KEY = @"<RSAKeyValue><Modulus>nCOWeDH/oUU6BBQA413O5XldIjTHWgoNCxQg/bTVukoJ5OG9ca3YZl03tPV9Vf5FNrN5NuDV7kHt5LPPkUBpQRqzEiGlly5K/MFhRzPHvNGcmwaDMuADG1E38i2Fiuo0SdyyGYAgbRPn2mC1PXD4eDwpybHGyXHF60wW3/8wjLs=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        /// <summary>
        /// PRIVATE KEY
        /// </summary>
        const string PRIVATE_KEY = @"<RSAKeyValue><Modulus>nCOWeDH/oUU6BBQA413O5XldIjTHWgoNCxQg/bTVukoJ5OG9ca3YZl03tPV9Vf5FNrN5NuDV7kHt5LPPkUBpQRqzEiGlly5K/MFhRzPHvNGcmwaDMuADG1E38i2Fiuo0SdyyGYAgbRPn2mC1PXD4eDwpybHGyXHF60wW3/8wjLs=</Modulus><Exponent>AQAB</Exponent><P>y3fa4Pn7j8GGuCf/khhmtnsSeUVVua5vFaoomD9MGvjmSyEd6OvADs+u2huDieKeESPYatoHDvbnMhJ4CCnebQ==</P><Q>xHOH8hju/JWn+xzvixpqea2il58GykA0en9XqiQt6Hvm15QUdaxzqDJJYuCzRtVpEJZySndkIaLFew9fMPd+xw==</Q><DP>wKB20rk35RBFl8EeXtTFIQuBINh4YTL4Ld2LUx/R0FNFy2jN5T6T9DHAivKzZG3sUbPK5tYFCrDLjocXpjrlPQ==</DP><DQ>s5cKSIASukX18tJZCklz3Rim8wUmJ+7aCsIvWhMJBOd/+MQekBS/BpwyCnpwaejey5M9mGXc3AL5la5Pz2vyMQ==</DQ><InverseQ>f4u74wyAnKihpdyHA124EIS0wycgKRuTiGKcagZvmPDmNMEKeYMKIOswPZde3Rdv2oZrAfU52H3enjgnpef9HQ==</InverseQ><D>LFBNXD+UdYjYNmAXAHCXT7lqHVMtYPiJSyOjWV4BaqWouTT4N1NHn4XFUa56q/VGX4gAVvrovH4prkb6GhdVx5rzoWWDa0trjoeis7109osUAaf8NIpdpL8+AkbhnDphDl0a0d5Kcz1SYSQSVzcUEwWVr0ynYuDMVdl3Yc4UcUE=</D></RSAKeyValue>";
        #endregion

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text">明文</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text, string publicKey = PUBLIC_KEY)
        {
            var blockSize = 256; // 加密块大小，越大越慢
            var bytes = Encoding.UTF8.GetBytes(text);
            var length = bytes.Length;

            // rsa实例
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);

            var offSet = 0; // 游标
            var i = 0;
            byte[] cache;
            var ms = new MemoryStream();
            while (length - offSet > 0)
            {
                var len = length - offSet > blockSize ? blockSize : length - offSet;
                var temp = new byte[len];
                Array.Copy(bytes, offSet, temp, 0, len);
                cache = rsa.Encrypt(bytes, false);
                ms.Write(cache, 0, cache.Length);

                i++;
                offSet = i * blockSize;
            }

            var cipherBytes = ms.ToArray();
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string ciphertext, string privateKey = PRIVATE_KEY)
        {
            var blockSize = 256;
            var bytes = Convert.FromBase64String(ciphertext);
            var length = bytes.Length;

            // rsa实例
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);

            var offSet = 0; // 游标
            var i = 0;
            byte[] cache;
            var ms = new MemoryStream();
            while (length - offSet > 0)
            {
                var len = length - offSet > blockSize ? blockSize : length - offSet;
                var temp = new byte[len];
                Array.Copy(bytes, offSet, temp, 0, len);
                cache = rsa.Decrypt(temp, false);
                ms.Write(cache, 0, cache.Length);

                i++;
                offSet = i * blockSize;
            }

            var cipherBytes = ms.ToArray();
            return Encoding.UTF8.GetString(cipherBytes);
        }
    }
}
