using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.Information;
using GHMonitoringCenterApi.Application.Contracts.Dto.SearchUser;
using GHMonitoringCenterApi.Application.Contracts.Dto.Timing;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.User;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Principal;
using UtilsSharp;

using Model = GHMonitoringCenterApi.Domain.Models;

namespace GHMonitoringCenterApi.Application.Service.User
{

    /// <summary>
    /// 用户接口实现层
    /// </summary>
    public class UserService : IUserService
    {

        #region 依赖注入
        public IBaseRepository<Domain.Models.User> baseUserRepository { get; set; }
        public IBaseRepository<UserInstitution> baseUserInstitutionRepository { get; set; }
        public IBaseRepository<InstitutionRole> baseInstitutionRoleRepository { get; set; }
        public IBaseRepository<Institution> baseInstitutionRepository { get; set; }
        public IBaseService baseService { get; set; }
        public ISqlSugarClient dbContent { get; set; }
        public IMapper mapper { get; set; }
        public UserService(IBaseRepository<Model.User> baseUserRepository, IBaseRepository<UserInstitution> baseUserInstitutionRepository,
            IBaseRepository<InstitutionRole> baseInstitutionRoleRepository, IBaseService baseService,
            IBaseRepository<Institution> baseInstitutionRepository, ISqlSugarClient dbContent, IMapper mapper, GlobalObject globalObject)
        {
            this.baseUserRepository = baseUserRepository;
            this.baseUserInstitutionRepository = baseUserInstitutionRepository;
            this.baseInstitutionRoleRepository = baseInstitutionRoleRepository;
            this.baseService = baseService;
            this.dbContent = dbContent;
            this.baseInstitutionRepository = baseInstitutionRepository;
            this.mapper = mapper;
        }


        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="httpContext">可以传递null</param>
        /// <returns></returns>
        public CurrentUser GetUserInfoAsync(string token)
        {
            #region 旧的
            //CurrentUser user = new CurrentUser();
            //var redis = RedisUtil.Instance;
            //var tokenUrl = AppsettingsHelper.GetValue("ParseTokenUrl");
            //WebHelper webHelper = new WebHelper();
            //Dictionary<string, string> dir = new Dictionary<string, string>();
            //dir.Add("token", token);

            //var tokenResult = webHelper.DoPostAsync(tokenUrl, dir).GetAwaiter().GetResult();
            //if (tokenResult.Code == 200)
            //{
            //    var code = JObject.Parse(tokenResult.Result);
            //    var responseCode = ((Newtonsoft.Json.Linq.JValue)code["code"]).Value.ToString();
            //    if (responseCode == "401")
            //    {
            //        return user;
            //    }
            //    else if (responseCode == "200")
            //    {
            //        var account = ((Newtonsoft.Json.Linq.JValue)code["data"]["userAccount"]).Value.ToString();

            //        #region 先从Redis里面取出  如果没有再去查询
            //        //var key = token.ToString().TrimAll().ToMd5();
            //        if (redis.Exists(account))
            //        {
            //            var userValue = redis.Get(account);
            //            if (!string.IsNullOrWhiteSpace(userValue))
            //            {
            //                var userBaeInfo = JsonConvert.DeserializeObject<CurrentUser>(userValue);
            //                if (userBaeInfo != null)
            //                {
            //                    return userBaeInfo;
            //                }
            //            }
            //        }
            //        #endregion

            //        var expTimeStamp = TimeHelper.TimeStampToDateTime(((Newtonsoft.Json.Linq.JValue)code["data"]["expTimeStamp"]).Value.ToString());
            //        var userInfo = dbContent.Queryable<Model.User>().Single(x => x.LoginAccount == account);
            //        if (userInfo != null)
            //        {
            //            user.Id = userInfo.Id;
            //            user.Account = userInfo.LoginAccount;
            //            user.Name = userInfo.Name;
            //            user.RoleInfos = new List<RoleInfo>();

            //            #region 查询角色信息
            //            //查询角色信息
            //            var userInstitutionList = dbContent.Queryable<UserInstitution>().Where(x => x.IsDelete == 1 && x.UserId == userInfo.Id).ToList();
            //            if (userInstitutionList != null && userInstitutionList.Any())
            //            {
            //                //获取用户的所有机构信息
            //                var institutionIds = userInstitutionList.Select(x => x.InstitutionId).ToList();

            //                #region 人员组织调整的逻辑 一个人可以在A部门或者B部门 或者在A公司B公司
            //                var userAllDepartment = dbContent.Queryable<UserExtended>().Where(x => x.IsDelete == 1 && x.UserId == user.Id).ToList();
            //                if (userAllDepartment != null && userAllDepartment.Any())
            //                {
            //                    var userAllDepartmentId = userAllDepartment.Select(x => x.DepartmentId).ToList();
            //                    foreach (var item in userAllDepartmentId)
            //                    {
            //                        institutionIds.Add(item);
            //                    }
            //                }

            //                #endregion

            //                if (institutionIds != null && institutionIds.Any())
            //                {
            //                    //查询用户角色机构管理表
            //                    var institutionRoleList = dbContent.Queryable<InstitutionRole>().Where(x => x.IsDelete == 1
            //                    && x.UserId.Value == userInfo.Id
            //                    ).ToList();
            //                    if (institutionRoleList != null && institutionRoleList.Any())
            //                    {
            //                        //获取当前用户所对应的机构信息
            //                        var currentUserAllIntsitutionList = baseInstitutionRepository.AsQueryable().Where(x => x.IsDelete == 1
            //                         && institutionRoleList.Select(x => x.InstitutionId).Contains(x.PomId)).ToList();

            //                        //当前用户有多少个角色
            //                        var roleIds = institutionRoleList.Select(x => x.RoleId).ToList();
            //                        if (roleIds != null && roleIds.Any())
            //                        {
            //                            Institution institution = null;
            //                            var roleList = dbContent.Queryable<Model.Role>().Where(x => x.IsDelete == 1 && roleIds.Contains(x.Id)).ToList();
            //                            if (roleList != null && roleList.Any())
            //                            {
            //                                foreach (var item in roleList)
            //                                {

            //                                    var currentRoleInstitutionId = institutionRoleList.SingleOrDefault(x => x.UserId == user.Id && x.RoleId == item.Id)?.InstitutionId;

            //                                    if (currentRoleInstitutionId != null)
            //                                    {
            //                                        institution = currentUserAllIntsitutionList.SingleOrDefault(x => x.PomId == currentRoleInstitutionId);
            //                                    }
            //                                    RoleInfo roleInfo = new RoleInfo()
            //                                    {
            //                                        Id = item.Id,
            //                                        Name = item.Name,
            //                                        Type = item.Type.Value,
            //                                        IsAdmin = item.IsAdmin,
            //                                        IsApprove = item.IsApprove,
            //                                        InstitutionId = currentRoleInstitutionId.Value,
            //                                        Oid = institution?.Oid,
            //                                        Grule = institution?.Grule,
            //                                        Poid = institution?.Poid,
            //                                        DepartmentInfos = new DepartmentInfo()
            //                                        {
            //                                            Id = currentRoleInstitutionId.Value,
            //                                            Name = institution.Shortname,
            //                                            RoleId = item.Id
            //                                        }
            //                                    };
            //                                    user.RoleInfos.Add(roleInfo);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            #endregion


            //            #region 多机构逻辑 暂时不用
            //            //List<Guid?> userDepartmentIds = new List<Guid?>();
            //            //var userDepartment = dbContent.Queryable<UserExtended>().Where(x => x.IsDelete == 1 && x.UserId == user.Id).ToList();
            //            //if (userDepartment != null && userDepartment.Any())
            //            //{
            //            //    userDepartmentIds = userDepartment.Select(x => x.DepartmentId).ToList();
            //            //}
            //            //userDepartmentIds.Add(user.DepartmentId.Value);
            //            //var institutionInfo = dbContent.Queryable<Institution>().Single(x => x.IsDelete == 1 && userDepartmentIds x.PomId == user.DepartmentId);

            //            //List<Guid> userDepartmentIds = new List<Guid>();
            //            //var userDepartment = dbContent.Queryable<UserExtended>().Where(x => x.IsDelete == 1 && x.UserId == user.Id).ToList();
            //            //if (userDepartment.Any())
            //            //{
            //            //    userDepartmentIds = userDepartment.Select(x => x.DepartmentId.Value).ToList();
            //            //}
            //            //userDepartmentIds.Add(user.DepartmentId.Value);
            //            //var institutionInfo = dbContent.Queryable<Institution>().Single(x => x.IsDelete == 1 && userDepartmentIds x.PomId == user.DepartmentId);

            //            #endregion

            //            #region 用户信息存入Redis
            //            redis.Set(user.Account, user.ToJson(), TimeHelper.GetTimeSpan(DateTime.Now, expTimeStamp));
            //            #endregion
            //            return user;
            //        }
            //    }
            //}
            #endregion

            #region 新的token解析
            CurrentUser user = new CurrentUser();
            var redis = RedisUtil.Instance;
            var tokenUrl = AppsettingsHelper.GetValue("ParseTokenUrl") + token;
            WebHelper webHelper = new WebHelper();
            var tokenResult = webHelper.DoGetAsync(tokenUrl).GetAwaiter().GetResult();
            if (tokenResult.Code == 200)
            {
                var code = JObject.Parse(tokenResult.Result);
                var account = ((Newtonsoft.Json.Linq.JValue)code["account"]).Value.ToString();
                #region 先从Redis里面取出  如果没有再去查询
                //var key = token.ToString().TrimAll().ToMd5();
                if (redis.Exists(account))
                {
                    var userValue = redis.Get(account);
                    if (!string.IsNullOrWhiteSpace(userValue))
                    {
                        var userBaeInfo = JsonConvert.DeserializeObject<CurrentUser>(userValue);
                        if (userBaeInfo != null)
                        {
                            return userBaeInfo;
                        }
                    }
                }
                #endregion
                //过期时间 兼容data.expTimeStamp
                DateTime expTimeStamp;
                if (code["data"] != null && code["expTimeStamp"] != null)
                {
                    expTimeStamp = TimeHelper.TimeStampToDateTime(((Newtonsoft.Json.Linq.JValue)code["data"]["expTimeStamp"]).Value.ToString());
                }
                else
                {
                    expTimeStamp = TimeHelper.TimeStampToDateTime(((Newtonsoft.Json.Linq.JValue)code["expires"]).Value.ToString());
                }
                //var expTimeStamp = TimeHelper.TimeStampToDateTime(((Newtonsoft.Json.Linq.JValue)code["data"]["expTimeStamp"]).Value.ToString());
                var userInfo = dbContent.Queryable<Model.User>().Single(x => x.LoginAccount == account);
                if (userInfo != null)
                {
                    user.Id = userInfo.Id;
                    user.Account = userInfo.LoginAccount;
                    user.Name = userInfo.Name;
                    user.RoleInfos = new List<RoleInfo>();

                    #region 查询角色信息
                    //查询角色信息
                    var userInstitutionList = dbContent.Queryable<UserInstitution>().Where(x => x.IsDelete == 1 && x.UserId == userInfo.Id).ToList();
                    if (userInstitutionList != null && userInstitutionList.Any())
                    {
                        //获取用户的所有机构信息
                        var institutionIds = userInstitutionList.Select(x => x.InstitutionId).ToList();

                        #region 人员组织调整的逻辑 一个人可以在A部门或者B部门 或者在A公司B公司
                        var userAllDepartment = dbContent.Queryable<UserExtended>().Where(x => x.IsDelete == 1 && x.UserId == user.Id).ToList();
                        if (userAllDepartment != null && userAllDepartment.Any())
                        {
                            var userAllDepartmentId = userAllDepartment.Select(x => x.DepartmentId).ToList();
                            foreach (var item in userAllDepartmentId)
                            {
                                institutionIds.Add(item);
                            }
                        }
                        #endregion

                        if (institutionIds != null && institutionIds.Any())
                        {
                            //查询用户角色机构管理表
                            var institutionRoleList = dbContent.Queryable<InstitutionRole>().Where(x => x.IsDelete == 1
                            && x.UserId.Value == userInfo.Id
                            ).ToList();
                            if (institutionRoleList != null && institutionRoleList.Any())
                            {
                                //获取当前用户所对应的机构信息
                                var currentUserAllIntsitutionList = baseInstitutionRepository.AsQueryable().Where(x => x.IsDelete == 1
                                 && institutionRoleList.Select(x => x.InstitutionId).Contains(x.PomId)).ToList();

                                //当前用户有多少个角色
                                var roleIds = institutionRoleList.Select(x => x.RoleId).ToList();
                                if (roleIds != null && roleIds.Any())
                                {
                                    Institution institution = null;
                                    var roleList = dbContent.Queryable<Model.Role>().Where(x => x.IsDelete == 1 && roleIds.Contains(x.Id)).ToList();
                                    if (roleList != null && roleList.Any())
                                    {
                                        foreach (var item in roleList)
                                        {

                                            var currentRoleInstitutionId = institutionRoleList.SingleOrDefault(x => x.UserId == user.Id && x.RoleId == item.Id)?.InstitutionId;

                                            if (currentRoleInstitutionId != null)
                                            {
                                                institution = currentUserAllIntsitutionList.SingleOrDefault(x => x.PomId == currentRoleInstitutionId);
                                            }
                                            RoleInfo roleInfo = new RoleInfo()
                                            {
                                                Id = item.Id,
                                                Name = item.Name,
                                                Type = item.Type.Value,
                                                IsAdmin = item.IsAdmin,
                                                IsApprove = item.IsApprove,
                                                OperationType = item.OperationType,
                                                InstitutionId = currentRoleInstitutionId.Value,
                                                Oid = institution?.Oid,
                                                Grule = institution?.Grule,
                                                Poid = institution?.Poid,
                                                DepartmentInfos = new DepartmentInfo()
                                                {
                                                    Id = currentRoleInstitutionId.Value,
                                                    Name = institution.Shortname,
                                                    RoleId = item.Id
                                                }
                                            };
                                            user.RoleInfos.Add(roleInfo);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion




                    #region 用户信息存入Redis
                    redis.Set(user.Account, user.ToJson(), TimeHelper.GetTimeSpan(DateTime.Now, expTimeStamp));
                    #endregion
                    return user;
                }
            }
            #endregion
            return user;
        }
        #endregion

        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="searchUserRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<InformationResponseDto>>> SearchPersonnelInformationAsync(SearchUserRequseDto searchUserRequestDto, string oid)
        {
            ResponseAjaxResult<List<InformationResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<InformationResponseDto>>();
            List<InformationResponseDto> informationResponseDtos = new List<InformationResponseDto>();
            //List<int> total = 0;
            RefAsync<int> total = 0;
            List<Guid> companyIds = new List<Guid>();
            var institutionRoles = await dbContent.Queryable<InstitutionRole>().Where(x => x.IsDelete == 1).Select(x => new { x.RoleId, x.InstitutionId, x.UserId }).ToListAsync();
            var institutionRoleSingle = institutionRoles.Where(x => x.RoleId == searchUserRequestDto.RoleId)
                .Select(x => new InstitutionRole() { InstitutionId = x.InstitutionId, UserId = x.UserId }).GroupBy(x => x.RoleId).Select(x => x.First()).SingleOrDefault();
            var institutins = await dbContent.Queryable<Institution>().Where(x => x.IsDelete == 1).Select(x => new { x.PomId, x.Name, x.Ocode, x.Oid, x.Grule }).ToListAsync();
            if (institutionRoleSingle != null)
            {

                var curGrule = institutins.Where(x => x.PomId == institutionRoleSingle.InstitutionId).SingleOrDefault()?.Grule;
                var reverseGrule = curGrule.Split("-", StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray()[0];
                companyIds = institutins.Where(x => x.Grule.Contains(reverseGrule)).Select(x => x.PomId.Value).ToList();
                var user = institutionRoles
                  .Where(x => x.InstitutionId == institutionRoleSingle.InstitutionId && x.RoleId == searchUserRequestDto.RoleId);
                var userIds = user.Where(x => !string.IsNullOrWhiteSpace(x.UserId.ToString())).Select(x => x.UserId.Value).ToList();
                //List<Guid> userIds = new List<Guid>();
                //排除系统管理员
                userIds.Add(AppsettingsHelper.GetValue("IsAdmin:Id").ToGuid());
                #region 过滤掉已经添加的用户
                var currentUserInstitutionRole = await baseInstitutionRoleRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.RoleId == searchUserRequestDto.RoleId).ToListAsync();
                if (currentUserInstitutionRole != null && currentUserInstitutionRole.Any())
                {
                    var instituIds = currentUserInstitutionRole.Select(x => x.InstitutionId).Distinct().ToList();
                    if (instituIds.Any())
                    {
                        var useIds = await baseUserInstitutionRepository.AsQueryable().Where(x => instituIds.Contains(x.InstitutionId)).Select(x => x.UserId.Value).ToListAsync();
                        foreach (var item in useIds)
                        {
                            userIds.Add(item);
                        }
                    }
                }
                #endregion
                List<Model.User> usersList = new List<Model.User>();

                if (companyIds.Count > 0)
                {
                    usersList = await baseUserRepository.AsQueryable()
                       .Where(x => x.IsDelete == 1)
                       .WhereIF(userIds.Any(), x => !userIds.Contains(x.Id))
                       .ToListAsync();
                    usersList = usersList
                               .Where(x => !string.IsNullOrEmpty(x.DepartmentId.ToString()) ? companyIds.Contains(x.DepartmentId.Value) : companyIds.Contains(x.CompanyId.Value)).ToList();
                }
                #region 人员组织调整附加逻辑
                var userExtendedList = await dbContent.Queryable<UserExtended>().Where(x => x.IsDelete == 1 && x.DepartmentId == institutionRoleSingle.InstitutionId).ToListAsync();
                if (userExtendedList.Any())
                {
                    var ids = userExtendedList.Select(x => x.UserId).ToList();
                    var roleUser = institutionRoles.Where(x => ids.Contains(x.UserId) && x.RoleId ==
                    searchUserRequestDto.RoleId && x.InstitutionId == institutionRoleSingle.InstitutionId).Select(x => new InstitutionRole() { UserId = x.UserId }).ToList();
                    if (roleUser.Any())
                    {
                        ids = ids.Where(x => !roleUser.Select(x => x.UserId).Contains(x)).ToList();
                    }
                    total = total + ids.Count;
                    var userExtendedInfoList = dbContent.Queryable<Model.User>().Where(x => x.IsDelete == 1 && ids.Contains(x.Id)).ToList();
                    if (userExtendedInfoList.Any())
                    {
                        foreach (var item in userExtendedList)
                        {
                            if (userExtendedInfoList.SingleOrDefault(x => x.Id == item.UserId) != null)
                            {
                                Model.User userExtend = new Model.User()
                                {
                                    CompanyId = item.CompanyId,
                                    DepartmentId = item.DepartmentId,
                                    Name = userExtendedInfoList.SingleOrDefault(x => x.Id == item.UserId)?.Name,
                                    LoginAccount = userExtendedInfoList.SingleOrDefault(x => x.Id == item.UserId)?.LoginAccount,
                                    Phone = userExtendedInfoList.SingleOrDefault(x => x.Id == item.UserId)?.Phone,
                                    Id = userExtendedInfoList.SingleOrDefault(x => x.Id == item.UserId).Id,
                                };
                                usersList.Add(userExtend);
                            }

                        }
                    }
                }
                #endregion
                if (usersList.Any())
                {
                    var cIds = usersList.Select(x => x.CompanyId.Value).ToList();
                    var companyList = await dbContent.Queryable<Company>().Where(x => cIds.Contains(x.PomId.Value)).ToListAsync();
                    usersList = usersList
                       .WhereIF(!string.IsNullOrWhiteSpace(searchUserRequestDto.KeyWords), x => (!string.IsNullOrEmpty(x.Name) && SqlFunc.Contains(x.Name, searchUserRequestDto.KeyWords))
                       || (!string.IsNullOrEmpty(x.Phone) && SqlFunc.Contains(x.Phone, searchUserRequestDto.KeyWords)))
                       .ToList();
                    total = usersList.Count;
                    usersList = usersList.Skip((searchUserRequestDto.PageIndex - 1) * searchUserRequestDto.PageSize).Take(searchUserRequestDto.PageSize).ToList();
                    informationResponseDtos = mapper.Map<List<Model.User>, List<InformationResponseDto>>(usersList);
                    var udepIds = usersList.Select(x => x.DepartmentId).ToList();
                    var institutinsData = institutins.Where(x => !string.IsNullOrWhiteSpace(x.Ocode) && udepIds.Contains(x.PomId)).ToList();
                    for (int i = 0; i < informationResponseDtos.Count; i++)
                    {
                        informationResponseDtos[i].Company = companyList.FirstOrDefault(x => x.PomId == usersList[i].CompanyId.Value)?.Name;
                        informationResponseDtos[i].DepartmentName = institutins.FirstOrDefault(x => x.PomId == usersList[i]?.DepartmentId)?.Name;
                    }
                }
            }

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(informationResponseDtos);
            return responseAjaxResult;
        }
        #endregion

    }
}
