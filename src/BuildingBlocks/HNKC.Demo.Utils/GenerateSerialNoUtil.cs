using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Utils/GenerateSerialNoUtil.cs
namespace HNKC.CrewManagePlatform.Utils
========
namespace HNKC.Demo.Utils
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Utils/GenerateSerialNoUtil.cs
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
