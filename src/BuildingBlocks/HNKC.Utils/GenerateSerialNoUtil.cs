using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.Utils
{
    public static class GenerateSerialNoUtil
    {
        public static string GenerateSerialNo(int lenth)
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random randrom = new Random((int)DateTime.Now.Ticks);
            string str = "";
            for (int i = 0; i < lenth; i++)
            {
                str += chars[randrom.Next(chars.Length)];
            }
            return str;
        }

        public static long GetUnixTime()
        {
            return DateTime.Now.ToUniversalTime().Ticks / 10000000 - 62135596800;
        }

        public static string GenerateAppNo(int lenth)
        {
            return GenerateSerialNo(lenth) + GetUnixTime();
        }
    }
}
