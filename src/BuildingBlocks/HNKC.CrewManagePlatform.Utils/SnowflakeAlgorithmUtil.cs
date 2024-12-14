using Snowflake.Core;

namespace HNKC.CrewManagePlatform.Utils
{
    /// <summary>
    /// 雪花算法工具类
    /// </summary>
    public class SnowFlakeAlgorithmUtil
    {
        private static object obj =new object();

        /// <summary>
        /// 单例 雪花实例
        /// </summary>
        private static IdWorker idWorker { get; set; }
        /// <summary>
        ///  单例 雪花实例
        /// </summary>
        private static IdWorker Instance
        {
            get
            {
                if (idWorker == null)
                {
                    lock (obj)
                    {
                        if (idWorker == null)
                        {
                            idWorker =new IdWorker(1, 1);
                        }
                    }
                }
                return idWorker;
            }
        }

        /// <summary>
        /// 生成雪花ID
        /// </summary>
        /// <returns></returns>
        public static long GenerateSnowflakeId() {
            return  Instance.NextId();
        }

    }
}
