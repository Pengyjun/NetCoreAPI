using HNKC.CrewManagePlatform.Common;
using HNKC.CrewManagePlatform.Models.CommonRequest;
using HNKC.CrewManagePlatform.Models.CommonResult;
using HNKC.CrewManagePlatform.Models.Dtos;
using HNKC.CrewManagePlatform.Models.Dtos.Role;
using HNKC.CrewManagePlatform.Models.Dtos.UserManager;
using HNKC.CrewManagePlatform.Models.Enums;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using HNKC.CrewManagePlatform.Utils;
using HNKC.CrewManagePlatform.Web.Jwt;
using SqlSugar;
using System.Data;
using System.Security.Claims;
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
            var token = string.Empty;
            var secretKey = AppsettingsHelper.GetValue("MD5SecretKey");
            var pwd = $"{secretKey}{userLoginRequest.Password}".ToMd5();
            //用户信息
            var userInfo = await dbContext.Queryable<User>().Where(x => x.IsDelete == 1 && x.Password == pwd &&
            ((x.Phone == userLoginRequest.Phone && x.IsLoginUser == 0) ||
            (x.LoginAccount == userLoginRequest.Phone && x.IsLoginUser == 2))
            )
            .FirstAsync();
            if (userInfo != null)
            {
                var instutionBusinessId = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.Oid == userInfo.Oid
              ).Select(x => x.BusinessId).FirstAsync();
                if (instutionBusinessId == Guid.Empty)
                {
                    return Result.Fail("登录失败", (int)ResponseHttpCode.LoginFail);
                }
                //存在多个角色时候  选择第一个创建的角色为默认角色
                //var userInstitutionList = await dbContext.Queryable<UserInstitution>().Where(x => x.IsDelete == 1 && x.UserBusinessId == userInfo.BusinessId && x.InstitutionBusinessId == instutionBusinessId).OrderBy(x => x.Created).ToListAsync();
                //if (userInstitutionList.Count == 0)
                //{
                //    return Result.Fail("登录失败", (int)ResponseHttpCode.LoginFail);
                //}
                //var roleBuinessId = userInstitutionList.Select(x => x.BusinessId).ToList();
                //用户机构角色信息
                var userInstitution = await dbContext.Queryable<InstitutionRole>().Where(x => x.IsDelete == 1 && x.UserBusinessId == userInfo.BusinessId).ToListAsync();
                //机构信息
                var institutionBusinessId = userInstitution.Select(x => x.InstitutionBusinessId).Distinct().ToList();
                var institution = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && institutionBusinessId.Contains(x.BusinessId)).ToListAsync();
                if (userInstitution.Count == 0)
                {
                    return Result.Fail("无权限请先分配权限", (int)ResponseHttpCode.LoginFail);
                }
                var roleBusinessId = userInstitution.Select(x => x.BusinessId).ToList();
                if (roleBusinessId.Count == 0)
                {
                    return Result.Fail("无权限请先分配权限", (int)ResponseHttpCode.LoginFail);
                }
                //默认选择第一个角色登录
                var firstRole = userInstitution.OrderBy(x => x.Created).FirstOrDefault();
                //角色信息
                var roleList = await dbContext.Queryable<HNKC.CrewManagePlatform.SqlSugars.Models.Role>().Where(x => x.IsDelete == 1 && roleBusinessId.Contains(x.BusinessId)).ToListAsync();
                var ownerShip = await dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).ToListAsync();
                if (roleList.Count > 0)
                {
                    UserLoginResponse userLoginResponse = new UserLoginResponse()
                    {
                        BId = userInfo.BusinessId,
                        Name = userInfo.Name,
                        WorkNumber = userInfo.WorkNumber,
                        RoleList = new List<Models.Dtos.Role.RoleResponse>()
                    };
                    foreach (var item in userInstitution)
                    {
                        var roleInfo = roleList.Where(x => x.BusinessId == item.RoleBusinessId).FirstOrDefault();
                        var institutionInfo = institution.Where(x => x.BusinessId == item.InstitutionBusinessId).FirstOrDefault();
                        if (userLoginResponse.RoleList.Count(x => x.RoleBusinessId == item.RoleBusinessId) == 0)
                        {
                            var owner = ownerShip.FirstOrDefault(t => t.ShipName == institutionInfo?.Name);
                            RoleResponse role = new RoleResponse()
                            {
                                InstitutionBusinessId = item.InstitutionBusinessId,
                                RoleBusinessId = item.BusinessId,
                                IsAdmin = roleInfo?.IsAdmin,
                                IsApprove = roleInfo?.IsApprove,
                                Name = roleInfo?.Name,
                                Oid = institutionInfo?.Oid,
                                Type = roleInfo?.Type,
                                ShipId = owner?.BusinessId,
                                ShipName = owner?.ShipName
                            };
                            userLoginResponse.RoleList.Add(role);
                        }

                    }
                    var firstRoleLogin = userLoginResponse.RoleList.Where(x => x.RoleBusinessId == firstRole.BusinessId).FirstOrDefault();
                    //激活角色标识
                    if (firstRoleLogin != null)
                    {
                        firstRoleLogin.IsActivate = true;
                    }

                    ////切换角色
                    //if (userLoginRequest.RoleBusinessId.HasValue)
                    //{
                    //    firstRoleLogin = userLoginResponse.RoleList.Where(x => x.RoleBusinessId == userLoginRequest.RoleBusinessId.Value).FirstOrDefault();
                    //}

                    //默认存第一个角色信息
                    Claim[] claims = new Claim[]
                    {
                        new Claim("Id",userInfo.Id.ToString()),
                        new Claim("IsAdmin",firstRoleLogin.IsAdmin.ToString()),
                        new Claim("BId",userInfo.BusinessId.ToString()),
                        new Claim("WorkNumber", userInfo.WorkNumber?.ToString() ?? string.Empty),
                        new Claim("Name",userInfo.Name?.ToString()),
                        new Claim("Oid",userInfo.Oid?.ToString()),
                        new Claim("BInstitutionId",firstRoleLogin.InstitutionBusinessId.ToString()),
                        new Claim("Phone",userInfo.Phone?.ToString()?? string.Empty),
                        new Claim("RoleBusinessId",firstRoleLogin.RoleBusinessId.ToString()),
                        new Claim("RoleType",firstRoleLogin.Type.ToString()),
                    };
                    var expores = int.Parse(AppsettingsHelper.GetValue("AccessToken:Expires"));
                    token = jwtService.CreateAccessToken(claims, expores);
                    userLoginResponse.Token = token;
                    //存入Redis
                    RedisUtil.Instance.Set(userInfo.Id.ToString(), token, expores);
                    return Result.Success(data: userLoginResponse);
                }
            }
            return Result.Fail("登录失败无效账号", (int)ResponseHttpCode.LoginFail);
            //HttpContentAccessFactory.Current.Response.Headers["Authorization"] = token;
        }

        /// <summary>
        /// 切换角色
        /// </summary>
        /// <param name="roleBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Result> ChangeRoleAsync(ChangRoleRequest changRoleRequest)
        {
            var token = string.Empty;
            var userInfo = GlobalCurrentUser;
            var allRole = await dbContext.Queryable<InstitutionRole>().Where(x => x.IsDelete == 1 && x.UserBusinessId == userInfo.UserBusinessId).ToListAsync();
            //角色ID
            var roleIds = allRole.Select(x => x.RoleBusinessId).ToList();
            //角色信息
            var roleList = await dbContext.Queryable<HNKC.CrewManagePlatform.SqlSugars.Models.Role>().Where(x => x.IsDelete == 1 && roleIds.Contains(x.BusinessId)).ToListAsync();
            //机构ID
            var institutionIds = allRole.Select(x => x.InstitutionBusinessId).ToList();
            //机构信息
            var institution = await dbContext.Queryable<Institution>().Where(x => x.IsDelete == 1 && institutionIds.Contains(x.BusinessId)).ToListAsync();
            var ownerShip = await dbContext.Queryable<OwnerShip>().Where(t => t.IsDelete == 1).ToListAsync();
            if (roleList.Count > 0)
            {

                UserLoginResponse userLoginResponse = new UserLoginResponse()
                {
                    BId = userInfo.UserBusinessId,
                    Name = userInfo.Name,
                    WorkNumber = userInfo.WorkNumber,
                    RoleList = new List<Models.Dtos.Role.RoleResponse>()
                };
                foreach (var item in allRole)
                {
                    var roleInfo = roleList.Where(x => x.BusinessId == item.RoleBusinessId).FirstOrDefault();
                    var institutionInfo = institution.Where(x => x.BusinessId == item.InstitutionBusinessId).FirstOrDefault();
                    if (userLoginResponse.RoleList.Count(x => x.RoleBusinessId == item.RoleBusinessId) == 0)
                    {
                        var owner = ownerShip.FirstOrDefault(t => t.ShipName == institutionInfo?.Name);
                        RoleResponse role = new RoleResponse()
                        {
                            InstitutionBusinessId = item.InstitutionBusinessId,
                            RoleBusinessId = item.BusinessId,
                            IsAdmin = roleInfo?.IsAdmin,
                            IsApprove = roleInfo?.IsApprove,
                            Name = roleInfo?.Name,
                            Oid = institutionInfo?.Oid,
                            Type = roleInfo?.Type,
                            ShipId = owner?.BusinessId,
                            ShipName = owner?.ShipName
                        };
                        userLoginResponse.RoleList.Add(role);
                    }

                }
                var loginRole = roleList.Where(x => x.BusinessId == changRoleRequest.RoleBusinessId).FirstOrDefault();
                if (loginRole == null)
                {
                    return Result.Fail("角色切换失败;当前用户不存在角色", (int)ResponseHttpCode.LoginFail);
                }
                var roleResponse = userLoginResponse.RoleList.Where(x => x.RoleBusinessId == loginRole.BusinessId).FirstOrDefault();
                if (roleResponse != null)
                {
                    roleResponse.IsActivate = true;
                }
                //默认存第一个角色信息
                Claim[] claims = new Claim[]
                {
                        new Claim("Id",userInfo.Id.ToString()),
                        new Claim("IsAdmin",loginRole.IsAdmin.ToString()),
                        new Claim("BId",userInfo.UserBusinessId.ToString()),
                        new Claim("WorkNumber",userInfo.WorkNumber?.ToString()),
                        new Claim("Name",userInfo.Name?.ToString()),
                        new Claim("Oid",roleResponse.Oid?.ToString()),
                        new Claim("BInstitutionId",changRoleRequest.InstitutionBusinessId.ToString()),
                        new Claim("Phone",userInfo.Phone?.ToString()),
                        new Claim("RoleBusinessId",loginRole.BusinessId.ToString()),
                        new Claim("RoleType",loginRole.Type.ToString()),
                };
                var expores = int.Parse(AppsettingsHelper.GetValue("AccessToken:Expires"));
                token = jwtService.CreateAccessToken(claims, expores);
                userLoginResponse.Token = token;
                //存入Redis
                RedisUtil.Instance.Set(userInfo.Id.ToString(), token, expores);
                return Result.Success(data: userLoginResponse);
            }
            return Result.Fail("角色切换失败", (int)ResponseHttpCode.LoginFail);
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

        public async Task<PageResult<UserResponse>> SearchUserAsync(PageRequest pageRequest)
        {
            PageResult<UserResponse> pageResult = new PageResult<UserResponse>();
            var data = await dbContext.Queryable<User>()
                 .LeftJoin<InstitutionRole>((a, b) => a.BusinessId == b.UserBusinessId)
                 .LeftJoin<HNKC.CrewManagePlatform.SqlSugars.Models.Role>((a, b, c) => b.RoleBusinessId == c.BusinessId)
                 .LeftJoin<HNKC.CrewManagePlatform.SqlSugars.Models.Institution>((a, b, c, d) => a.Oid == d.Oid)
                 .Where(a => a.IsDelete == 1 && a.IsLoginUser != 2)
                 .WhereIF(!string.IsNullOrWhiteSpace(pageRequest.KeyWords), a => a.Name.Contains(pageRequest.KeyWords)
                 || a.Phone.Contains(pageRequest.KeyWords))
                 .Select((a, b, c, d) => new UserResponse()
                 {
                     BId = a.BusinessId,
                     Name = a.Name,
                     Phone = a.Phone,
                     Remark = a.Remark,
                     Created = a.Created,
                     RoleName = c.Name,
                     Oid = a.Oid,
                     Gender = a.Gender,
                     WorkNumber = a.WorkNumber,
                     DepartmentName = d.ShortName
                 })
               .ToListAsync();

            List<UserResponse> replaceUser = new List<UserResponse>();
            foreach (var item in data)
            {
                if (replaceUser.Count(x => x.BId == item.BId) == 0)
                {
                    item.RoleName = string.Join(",", data.Where(x => x.BId == item.BId).Select(x => x.RoleName).ToList());
                    replaceUser.Add(item);
                }
            }

            pageResult.TotalCount = replaceUser.Count;
            pageResult.List = replaceUser.Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize).Take(pageRequest.PageSize).ToList();
            return pageResult;
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="addUserRequest"></param>
        /// <returns></returns>
        public async Task<Result> AddUserAsync(AddUserRequest addUserRequest)
        {
            var userInfo = await dbContext.Queryable<User>().Where(x => x.Phone == addUserRequest.Phone).FirstAsync();
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
                WorkNumber = addUserRequest.WorkNumber,
                Password = pwd,
                IsInsert = 1,
                Gender = addUserRequest.Gender.Value,
                Remark = addUserRequest.Remark
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
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="modifyUserResquest"></param>
        /// <returns></returns>
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
