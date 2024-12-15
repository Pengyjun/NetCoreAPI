using HNKC.CrewManagePlatform.Common;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos.UserManager;
using HNKC.CrewManagePlatform.Services.Interface.CurrentUserService;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using HNKC.CrewManagePlatform.Web.Jwt;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;

namespace HNKC.CrewManagePlatform.Services.Interface.CurrentUser
{
    public class UserManagerService : CurrentUserService, IUserManagerService
    {
        #region 依赖注入
        private readonly ISqlSugarClient dbContext;
        private readonly IJwtService jwtService;
        public UserManagerService(ISqlSugarClient dbContext, IJwtService jwtService)
        {
            this.dbContext = dbContext;
            this.jwtService = jwtService;
        }


        #endregion

        public async Task<Result> UserLoginAsync(UserLoginRequest userLoginRequest)
        {
            var token = string.Empty;
            var secretKey = AppsettingsHelper.GetValue("MD5SecretKey");
            var pwd = $"{secretKey}{userLoginRequest.Password}".ToMd5();
            var userInfo = await dbContext.Queryable<User>().Where(x => x.IsDelete == 1 && x.WorkNumber == userLoginRequest.WorkNumber
            && x.Password == pwd).FirstAsync();
            if (userInfo != null)
            {
                Claim[] claims = new Claim[]
                {
                  new Claim("Id",userInfo.Id.ToString()),
                  new Claim("WorkNumber",userInfo.WorkNumber?.ToString()),
                  new Claim("Name",userInfo.Name?.ToString()),
                  new Claim("Oid",userInfo.Oid?.ToString()),
                  new Claim("Phone",userInfo.Phone?.ToString()),
                };
                var expores = int.Parse(AppsettingsHelper.GetValue("AccessToken:Expires"));
                token = jwtService.CreateAccessToken(claims, expores);
                //存入Redis
                RedisUtil.Instance.Set(userInfo.Id.ToString(), token, expores);
                return Result.Success(data: token);
            }
            else {
                return Result.Fail("登录失败");
            }
            //HttpContentAccessFactory.Current.Response.Headers["Authorization"] = token;
          
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="resetPwdResquest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result> ResetPwdAsync(ResetPwdResquest resetPwdResquest)
        {
            var userInfo = GlobalCurrentUser;
            var secretKey = AppsettingsHelper.GetValue("MD5SecretKey");
            var pwd = $"{secretKey}{resetPwdResquest.OldOnePwd}".ToMd5();
            var currentUser = await dbContext.Queryable<User>().Where(x => x.IsDelete == 1 && userInfo.WorkNumber == userInfo.WorkNumber && x.Password == pwd).FirstAsync();
            if (currentUser == null)
            {
                return Result.Fail("源密码输入不正确");
            }
            if (!resetPwdResquest.NewOnePwd.Equals(resetPwdResquest.NewTwoPwd))
            {
                return Result.Fail("密码不一致请重新输入");
            }
            var newPwd = $"{secretKey}{resetPwdResquest.NewOnePwd}".ToMd5();
            currentUser.Password = newPwd;
            await dbContext.Updateable(currentUser).ExecuteCommandAsync();
            return Result.Success("密码重置成功");
        }
    }
}
