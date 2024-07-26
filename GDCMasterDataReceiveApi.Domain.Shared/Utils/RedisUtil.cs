using CSRedis;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Domain.Shared.Utils
{

    /// <summary>
    /// redis 简单工具类
    /// </summary>
    public static class RedisUtil
    {
        public readonly static object obj = new object();
        private readonly static string ip = AppsettingsHelper.GetValue("Redis:Ip");
        private readonly static string password = AppsettingsHelper.GetValue("Redis:Password");
        private readonly static string defaultDatabase = AppsettingsHelper.GetValue("Redis:DefaultDatabase");
        private readonly static string poolsize = AppsettingsHelper.GetValue("Redis:Poolsize");
        private readonly static string ssl = AppsettingsHelper.GetValue("Redis:Ssl");
        private readonly static string writeBuffer = AppsettingsHelper.GetValue("Redis:WriteBuffer");
        private readonly static string prefix = AppsettingsHelper.GetValue("Redis:Prefix");
        private static CSRedisClient redisClient { get; set; }
        private  static CSRedisClient RedisClientInit()
        {
			var connetStr = $"{ip},password={password},defaultDatabase={defaultDatabase},poolsize={poolsize},ssl={ssl},writeBuffer={writeBuffer},prefix={prefix}";
			var client = new CSRedis.CSRedisClient(connetStr);
            RedisHelper.Initialization(client);
            return RedisHelper.Instance;
        }
        /// <summary>
        /// RedisInstance 实例
        /// </summary>
        public static CSRedisClient  Instance
        {
            get
            {
                if (redisClient == null)
                {
                    lock (obj)
                    {
                        if (redisClient == null)
                        {
                            redisClient = RedisClientInit();
                        }
                    }
                }
                return redisClient;
            }
        }
    }
}
