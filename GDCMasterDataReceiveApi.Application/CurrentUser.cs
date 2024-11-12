using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Domain.Shared;
using Newtonsoft.Json.Linq;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application
{
    /// <summary>
    /// 用户信息接口层
    /// </summary>
    public abstract class CurrentUser : ICurrentUser
    {
        #region 全局对象暴露属性
        /// <summary>
        /// 全局对象暴露属性
        /// </summary>
        public GlobalCurrentUser GlobalCurrentUser {
            get { return   UserAuthenticatedAsync().GetAwaiter().GetResult(); }
        }
        #endregion

        #region 获取用户信息并返回全局用户对象
        /// <summary>
        /// 获取用户信息并返回全局用户对象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GlobalCurrentUser> UserAuthenticatedAsync()
        {
            #region 旧代码


            //GlobalCurrentUser globalCurrentUser = new GlobalCurrentUser();
            //var userToken = HttpContentAccessFactory.GetUserToken;
            //if (string.IsNullOrWhiteSpace(userToken))
            //{
            //    return globalCurrentUser;
            //}
            ////token解析地址
            //var tokenParseUrl = $"{AppsettingsHelper.GetValue("ParseTokenUrl")}{userToken}";
            //WebHelper webHelper = new WebHelper();
            //var tokenResult = await webHelper.DoGetAsync(tokenParseUrl);
            //if (tokenResult.Code == 200)
            //{
            //    //var redis = RedisUtil.Instance;
            //    var code = JObject.Parse(tokenResult.Result);
            //    //用户账号
            //    var account = ((Newtonsoft.Json.Linq.JValue)code["account"]).Value.ToString();
            //    #region 先从Redis里面取出  如果没有再去查询
            //    //if (await redis.ExistsAsync(account))
            //    //{
            //    //    var userAccount = await redis.GetAsync(account);
            //    //    if (!string.IsNullOrWhiteSpace(userAccount))
            //    //    {
            //    //        var userBaeInfo = JsonConvert.DeserializeObject<GlobalCurrentUser>(userAccount);
            //    //        if (userBaeInfo != null)
            //    //        {
            //    //            return globalCurrentUser;
            //    //        }
            //    //    }
            //    //}
            //    #endregion

            //    #region 过期时间 兼容data.expTimeStamp
            //    //过期时间 兼容data.expTimeStamp
            //    DateTime expTimeStamp;
            //    if (code["data"] != null && code["expTimeStamp"] != null)
            //    {
            //        expTimeStamp = TimeHelper.TimeStampToDateTime(((Newtonsoft.Json.Linq.JValue)code["data"]["expTimeStamp"]).Value.ToString());
            //    }
            //    else
            //    {
            //        expTimeStamp = TimeHelper.TimeStampToDateTime(((Newtonsoft.Json.Linq.JValue)code["expires"]).Value.ToString());
            //    }
            //    #endregion

            //    #region 模拟数据库查询
            //    List<GlobalCurrentUser> globalCurrentUsers = new List<GlobalCurrentUser>();
            //    globalCurrentUsers.Add(new GlobalCurrentUser()
            //    {
            //        Account = "L10020065",
            //        Id = 11111111111111111
            //    });
            //    globalCurrentUsers.Add(new GlobalCurrentUser()
            //    {
            //        Account = "2022002687",
            //        Id = 2222222222222
            //    });
            //    var res= globalCurrentUsers.Where(x => x.Account == account).First();
            //    return res;
            //    #endregion
            //}
            //return globalCurrentUser;
            #endregion

            GlobalCurrentUser globalCurrentUser = new GlobalCurrentUser();
            var appKey = "appKey";
            var appinterfaceCode = "appinterfaceCode";
            var headers = HttpContentAccessFactory.Current.Request
                .HttpContext.Request.Headers;
            if (headers.ContainsKey(appKey) && headers.ContainsKey(appinterfaceCode))
            {
                var sKey = HttpContentAccessFactory.Current.Request.HttpContext.Request.Headers[appKey].ToString();
                var iKey = HttpContentAccessFactory.Current.Request.HttpContext.Request.Headers[appinterfaceCode].ToString();
                globalCurrentUser.AppKey = sKey;
                globalCurrentUser.AppinterfaceCode = iKey;
            }
                return globalCurrentUser;
        }
        #endregion
    }
}
