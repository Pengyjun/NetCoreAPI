﻿using HNKC.CrewManagePlatform.Common;
using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.UserManager;
using HNKC.CrewManagePlatform.Models.Enums;
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

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userLoginRequest"></param>
        /// <returns></returns>
        public async Task<Result> UserLoginAsync(UserLoginRequest userLoginRequest)
        {
            //var currentUser = GlobalCurrentUser;
            var token = string.Empty;
            var secretKey = AppsettingsHelper.GetValue("MD5SecretKey");
            var pwd = $"{secretKey}{userLoginRequest.Password}".ToMd5();
            var userInfo = await dbContext.Queryable<User>().Where(x => x.IsDelete == 1 && x.WorkNumber == userLoginRequest.WorkNumber
            && x.Password == pwd).FirstAsync();
            if (userInfo != null)
            {
                var instutionInfo = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == userInfo.Oid
                 ).FirstAsync();
                if (instutionInfo != null)
                {
                    var userInstitution = await dbContext.Queryable<UserInstitution>().FirstAsync(x => x.IsDelete == 1 && x.UserBusinessId == userInfo.BusinessId && x.InstitutionBusinessId == instutionInfo.BusinessId);
                    if (userInstitution != null)
                    {
                        var institutionRoleInfo = await dbContext.Queryable<InstitutionRole>().Where(x => x.IsDelete == 1 && x.InstitutionBusinessId == userInstitution.InstitutionBusinessId).FirstAsync();
                        if (institutionRoleInfo == null)
                        {
                            return Result.Fail("登录失败");
                        }
                        var roltInfo = await dbContext.Queryable<HNKC.CrewManagePlatform.SqlSugars.Models.Role>().Where(x => x.IsDelete == 1 && x.BusinessId == institutionRoleInfo.RoleBusinessId).FirstAsync();

                        Claim[] claims = new Claim[]
                       {
                          new Claim("Id",userInfo.Id.ToString()),
                          new Claim("IsAdmin",roltInfo.IsAdmin.ToString()),
                          new Claim("BId",userInfo.BusinessId.ToString()),
                          new Claim("WorkNumber",userInfo.WorkNumber?.ToString()),
                          new Claim("Name",userInfo.Name?.ToString()),
                          new Claim("Oid",userInfo.Oid?.ToString()),
                          new Claim("BInstitutionId",instutionInfo.BusinessId.ToString()),
                          new Claim("Phone",userInfo.Phone?.ToString()),
                          new Claim("RoleBusinessId",institutionRoleInfo.RoleBusinessId.ToString()),
                       };
                        var expores = int.Parse(AppsettingsHelper.GetValue("AccessToken:Expires"));
                        token = jwtService.CreateAccessToken(claims, expores);
                        //存入Redis
                        RedisUtil.Instance.Set(userInfo.Id.ToString(), token, expores);
                        return Result.Success(data: token);
                    }

                }
            }
            return Result.Fail("登录失败");
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


        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public Task<PageResult<UserResponse>> SearchUserAsync(PageRequest pageRequest)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="addUserRequest"></param>
        /// <returns></returns>
        public async Task<Result> AddUserAsync(AddUserRequest addUserRequest)
        {
            var userInfo = dbContext.Queryable<User>().Where(x => x.Phone == addUserRequest.Phone).FirstAsync();
            if (userInfo != null)
            {
                return Result.Fail("手机号已存在请修改", (int)ResponseHttpCode.DataExist);
            }
            var secretKey = AppsettingsHelper.GetValue("MD5SecretKey");
            var pwd = $"{secretKey}{addUserRequest.Pwd}".ToMd5();
            User user = new User()
            {
                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                BusinessId = GuidUtil.Next(),
                Name = addUserRequest.Name,
                Phone = addUserRequest.Phone,
                Oid = addUserRequest.Oid,
                Password = pwd,
                IsInsert = 1
            };

            await dbContext.Insertable<User>(user).ExecuteCommandAsync();
            return Result.Success("添加成功");

        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="baseRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result> RemoveUserAsync(BaseRequest baseRequest)
        {
            var userInfo = await dbContext.Queryable<User>().Where(x => x.BusinessId == baseRequest.BId).FirstAsync();
            if (userInfo == null)
            {
                return Result.Fail("数据不存在", (int)ResponseHttpCode.DataNoExist);
            }
            userInfo.IsDelete = 0;
            await dbContext.Updateable<User>(userInfo).ExecuteCommandAsync();
            return Result.Success("删除成功");
        }

        public async Task<Result> ModifyUserAsync(ModifyUserResquest modifyUserResquest)
        {
            var userInfo = await dbContext.Queryable<User>().Where(x => x.BusinessId == modifyUserResquest.BId).FirstAsync();
            if (userInfo == null)
            {
                return Result.Fail("数据不存在", (int)ResponseHttpCode.DataNoExist);
            }
            userInfo.Name = modifyUserResquest.Name;
            userInfo.Phone = modifyUserResquest.Phone;
            userInfo.Password = modifyUserResquest.Pwd;
            userInfo.Oid = modifyUserResquest.Oid;
            await dbContext.Updateable<User>(userInfo).ExecuteCommandAsync();
            return Result.Success("修改成功");
        }
    }
}
