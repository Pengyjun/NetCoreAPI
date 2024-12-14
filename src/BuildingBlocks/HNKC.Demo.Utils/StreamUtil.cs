using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Utils/StreamUtil.cs
namespace HNKC.CrewManagePlatform.Utils
========
namespace HNKC.Demo.Utils
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Utils/StreamUtil.cs
{
    public static class StreamUtil
    {
        public static byte[] ToBytes(this Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// 将 byte[] 转成 Stream

        public static Stream ToStream(this byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        public static byte[] ToBytes(this string base64)
        {
            return Convert.FromBase64String(base64);
        }

    }
}
