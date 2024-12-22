using System.Security.Cryptography;

namespace HNKC.CrewManagePlatform.Utils
{
    public class GuidUtil
    {
        /// <summary>
        /// RFC版本,值为 1~5
        /// </summary>
        private static readonly byte _version = 4;
        /// <summary>
        /// RFC变体,值为 [8,9,A,B]
        /// </summary>
        private static readonly byte _variant = 8;
        /// <summary>
        /// 高位过滤，0b00001111
        /// </summary>
        private static readonly byte _filterHighBit = 0b00001111;
        /// <summary>
        /// 低位过滤，0b11110000
        /// </summary>
        private static readonly byte _filterLowBit = 0b11110000;
        private static readonly RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();

        /// <summary>
        /// 生成连续递增的GUID(不符合 RFC 4122 标准)
        /// </summary>  
        /// <param name="sequentialGuidType"></param>
        /// <returns></returns>
        public static Guid Increment(SequentialGuidType guidType = SequentialGuidType.AsString)
        {
            var randomBytes = new byte[8];
            _randomNumberGenerator.GetBytes(randomBytes);
            // 一个Tick是100ns，10_000Tick=1ms
            var timestamp = DateTime.UtcNow.Ticks;
            var timestampBytes = BitConverter.GetBytes(timestamp);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }
            var rslt = new byte[16];
            switch (guidType)
            {
                case SequentialGuidType.AsString:
                case SequentialGuidType.AsBinary:
                    {
                        // 16位数组：前8位为时间戳，后8位为随机数
                        Buffer.BlockCopy(timestampBytes, 0, rslt, 0, 8);
                        Buffer.BlockCopy(randomBytes, 0, rslt, 8, 8);
                        // 小端系统需要翻转
                        if (guidType == SequentialGuidType.AsString && BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(rslt, 0, 4);
                            Array.Reverse(rslt, 4, 2);
                            Array.Reverse(rslt, 6, 2);
                        }
                    }
                    break;
                case SequentialGuidType.AsEnd:
                    {
                        // 16位数组：前8位为随机数，后8位为时间戳
                        Buffer.BlockCopy(randomBytes, 0, rslt, 0, 8);
                        Buffer.BlockCopy(timestampBytes, 6, rslt, 8, 2);
                        Buffer.BlockCopy(timestampBytes, 0, rslt, 10, 6);
                    }
                    break;
                default:
                    break;
            }
            return new Guid(rslt);
        }
        /// <summary>
        /// 生成连续GUID
        /// <para>符合 RFC 4122 标准</para>
        /// </summary>
        /// <param name="sequentialGuidType"></param>
        /// <returns></returns>
        public static Guid Next(SequentialGuidType guidType = SequentialGuidType.AsString)
        {
            /* RFC 4122
             * dddddddd-dddd-Mddd-Ndrr-rrrrrrrrrrrr
             * M，RFC版本（version），这里值为4
             * N，RFC变体（variant），这里固定值为8
             * d，从0001-1-1 0时至今的时钟周期数（DateTime.UtcNow.Ticks）
             * r，随机数（random bytes）
             */
            var randomBytes = new byte[8];
            _randomNumberGenerator.GetBytes(randomBytes);
            var timestamp = DateTime.UtcNow.Ticks;
            var timestampBytes = BitConverter.GetBytes(timestamp);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }
            var rslt = new byte[16];
            switch (guidType)
            {
                case SequentialGuidType.AsString:
                case SequentialGuidType.AsBinary:
                    {
                        /* 时间戳 + 随机数
                         * 0 1 2 3  4 5  6 7  8 9  10
                         * dddddddd-dddd-Mddd-Ndrr-rrrrrrrrrrrr
                         * Block1   2     3   4     5
                         * Data1    2     3   4
                         */
                        // 时间戳前6个字节
                        Buffer.BlockCopy(timestampBytes, 0, rslt, 0, 6);
                        // 高4位为版本 | 低4位是时间戳元素[6]的高4位
                        rslt[6] = (byte)(_version << 4 | (timestampBytes[6] & _filterLowBit) >> 4);
                        // 高4位：[6]低4位 | 低4位：[7]高4位
                        rslt[7] = (byte)((timestampBytes[6] & _filterHighBit) << 4 | (timestampBytes[7] & _filterLowBit) >> 4);
                        // 高4位：变体 | 低4位：[7]低4位
                        rslt[8] = (byte)(_variant << 4 | timestampBytes[7] & _filterHighBit);
                        // 剩余7个字节由随机数组填充
                        Buffer.BlockCopy(randomBytes, 0, rslt, 9, 7);
                        // 小端系统需要翻转
                        if (guidType == SequentialGuidType.AsString && BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(rslt, 0, 4);
                            Array.Reverse(rslt, 4, 2);
                            Array.Reverse(rslt, 6, 2);
                        }
                    }
                    break;
                case SequentialGuidType.AsEnd:
                    {
                        /* 随机数 + 时间戳
                         * rrrrrrrr-rrrr-Mxdr-Nddd-dddddddddddd
                         * Block1    2    3     4   5
                         * Data4 = Block4 + Block5
                         * 排序顺序：Block5 > Block4 > Block3 > Block2 > Block1
                         * Data3 = Block3，被认为是 uint16，排序并不是从左往右，为消除影响，x位取固定值
                         */
                        Buffer.BlockCopy(randomBytes, 0, rslt, 0, 6);
                        // Mx 高4位为版本 | 低4位：0
                        rslt[6] = (byte)(_version << 4);
                        // dr 高4位：[7]低4位 | 低4位：随机数
                        rslt[7] = (byte)((timestampBytes[7] & _filterHighBit) << 4 | randomBytes[7] & _filterHighBit);
                        // Nd 高4位：变体 | 低4位：[6]高4位
                        rslt[8] = (byte)(_variant << 4 | (timestampBytes[6] & _filterLowBit) >> 4);
                        // dd 高4位：[6]低4位 | 低4位：[7]高4位
                        rslt[9] = (byte)((timestampBytes[6] & _filterHighBit) << 4 | (timestampBytes[7] & _filterLowBit) >> 4);
                        // 剩余6个字节是时间戳的前6个字节
                        Buffer.BlockCopy(timestampBytes, 0, rslt, 10, 6);
                        if (BitConverter.IsLittleEndian)
                        {
                            // 包含版本数的 Data3 需要翻转
                            Array.Reverse(rslt, 6, 2);
                        }
                    }
                    break;
                default:
                    break;
            }
            return new Guid(rslt);
        }

        public enum SequentialGuidType
        {
            /// <summary>
            /// <para>dddddddd-dddd-Mddd-Ndrr-rrrrrrrrrrrr</para>
            /// <para>用于MySql和PostgreSql</para>
            /// <para>当使用<see cref="Guid.ToString()"/>方法进行格式化时连续</para>
            /// <para>顺序体现在第8个字节</para>
            /// </summary>
            AsString,
            /// <summary>
            /// <para>dddddddd-dddd-Mddd-Ndrr-rrrrrrrrrrrr</para>
            /// <para>用于Oracle</para>
            /// <para>当使用<see cref="Guid.ToByteArray()"/>方法进行格式化时连续</para>
            /// <para>顺序体现在第8个字节，连续递增</para>
            /// </summary>
            AsBinary,
            /// <summary>
            /// <para>rrrrrrrr-rrrr-Mxdr-Nddd-dddddddddddd</para>
            /// <para>用于SqlServer</para>
            /// <para>连续性体现于GUID的第4块（Data4）</para>
            /// <para>顺序比较Block5 > Block4 > Block3 > Block2 > Block1</para>
            /// </summary>
            AsEnd,
        }
        /// <summary>
        /// 32位guid
        /// </summary>
        /// <returns></returns>
        public static int Generate32BitGuid()
        {
            var guid = Guid.NewGuid();  // 生成一个新的 GUID
            byte[] bytes = guid.ToByteArray();  // 将 GUID 转换为字节数组
            return BitConverter.ToInt32(bytes, 0);  // 从字节数组中提取前 4 个字节并转换为 32 位整数
        }
    }
}
