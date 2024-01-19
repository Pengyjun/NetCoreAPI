using AutoMapper;
using GHMonitoringCenterApi.Application.Contracts.Dto.PersonnelOrganizeAdjust;
using GHMonitoringCenterApi.Application.Contracts.IService;
using GHMonitoringCenterApi.Application.Contracts.IService.OperationLog;
using GHMonitoringCenterApi.Application.Contracts.IService.PersonnelOrganizeAdjust;
using GHMonitoringCenterApi.Application.Contracts.IService.Role;
using GHMonitoringCenterApi.Application.Service.Role;
using GHMonitoringCenterApi.Domain.IRepository;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Shared.Const;
using GHMonitoringCenterApi.Domain.Shared.Enums;
using GHMonitoringCenterApi.Domain.Shared.Util;
using SqlSugar;
using UtilsSharp;

namespace GHMonitoringCenterApi.Application.Service.PersonnelOrganizeAdjust
{
    /// <summary>
    /// 
    /// 
    /// 
    /// 人员数据调整接口实现层
    /// </summary>
    public class PersonnelOrganizeAdjustService : IPersonnelOrganizeAdjustService
    {
        #region 依赖注入
        public IBaseService baseService { get; set; }
        public IRoleService roleService { get; set; }       
        public IBaseRepository<Domain.Models.User> baseUserRepository { get; set; }
        public IBaseRepository<InstitutionRole> baseInstitutionRoleRepository { get; set; }
        public IBaseRepository<UserExtended> baseUserExtendedRepository { get; set; }
        public IMapper mapper { get; set; }
        public ISqlSugarClient dbContent { get; set; }
        public ILogService logService { get; set; }
        private CurrentUser _currentUser { get { return _globalObject.CurrentUser; } }
        private readonly GlobalObject _globalObject;

        public PersonnelOrganizeAdjustService(IBaseService baseService, IRoleService roleService, IBaseRepository<Domain.Models.User> baseUserRepository, IBaseRepository<InstitutionRole> baseInstitutionRoleRepository, IBaseRepository<UserExtended> baseUserExtendedRepository, IMapper mapper, ISqlSugarClient dbContent, ILogService logService, GlobalObject globalObject)
        {
            this.baseService = baseService;
            this.roleService = roleService;
            this.baseUserRepository = baseUserRepository;
            this.baseInstitutionRoleRepository = baseInstitutionRoleRepository;
            this.baseUserExtendedRepository = baseUserExtendedRepository;
            this.mapper = mapper;
            this.dbContent = dbContent;
            this.logService = logService;
            _globalObject = globalObject;
        }


        #endregion
        /// <summary>
        /// 获取已添加人员数据
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<SearchAddedPersonnelReponseDto>>> SearchAddedPersonnelAsync(SearchAddedPersonnelRequestDto searchAddedPersonnelRequestDto,Guid UserId , Guid CompanyId)
        {
            RefAsync<int> total = 0;
            ResponseAjaxResult<List<SearchAddedPersonnelReponseDto >> responseAjaxResult = new ResponseAjaxResult<List<SearchAddedPersonnelReponseDto>>();
            var institutionRoleList = await baseInstitutionRoleRepository.AsQueryable().Where(x=>x.UserId != null&&x.IsDelete==1).ToListAsync();
            var idList= institutionRoleList.Select(x => x.UserId).ToList();
            var personnelList = await dbContent.Queryable<Domain.Models.User>()
               .Where(x => x.IsDelete == 1 && idList.Contains(x.Id)&&x.Id !=AppsettingsHelper.GetValue("IsAdmin:Id").ToGuid())
               .WhereIF(!string.IsNullOrWhiteSpace(searchAddedPersonnelRequestDto.KeyWords),x=> SqlFunc.Contains(x.Name, searchAddedPersonnelRequestDto.KeyWords))              
               .ToListAsync();
            #region 权限判断
            var roleIds =await dbContent.Queryable<InstitutionRole>()
                .Where(x => x.InstitutionId == CompanyId&&x.UserId == UserId&&x.IsDelete == 1)              
                .ToListAsync();
            var roleId = roleIds.Select(x => x.RoleId).ToList();
            var type = await dbContent.Queryable<Domain.Models.Role>().Where(x => roleId.Contains(x.Id)).Select(x => x.Type).ToListAsync();
            if (!type.Contains(2)&&!type.Contains(1))
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_NOPERMISSION_FAIL, HttpStatusCode.NoPermission);
                return responseAjaxResult;
            }
            var institutions = await dbContent.Queryable<Institution>().Where(x => x.PomId == CompanyId).Select(x => x.Grule).SingleAsync();
            //var institution= await baseService.
            //SubPullDownAsync(CompanyId, false);
           var institution = await dbContent.Queryable<Institution>().Where(x => x.Grule.Contains(institutions)).ToListAsync();
            var institutionId = institution.Select(x=>x.PomId).ToList();
            var personnelLists =  personnelList.Where(x => institutionId.Contains(x.CompanyId)).ToList();
            #endregion
            var userList = mapper.Map<List<Domain.Models.User>, List<SearchAddedPersonnelReponseDto>>(personnelLists);
            var cIds = personnelList.Select(x => x.CompanyId).ToList();
            var dIds = personnelList.Select(x => x.DepartmentId).ToList();
            var companyList = await dbContent.Queryable<Institution>().Where(x => cIds.Contains(x.PomId)).ToListAsync();
            var institutionList =await dbContent.Queryable<Institution>().ToListAsync();
            var institutionIds =  institutionList.Where(x => dIds.Contains(x.PomId)).ToList();   
            foreach (var item in userList)
            {
                var oIds = institutionList.SingleOrDefault(x => x.PomId == item.DepartmentId);
                if (oIds != null)
                {
                    item.SCompanyName = institutionList.SingleOrDefault(x => x.Oid == oIds.Poid)?.Shortname;
                    item.CompanyName = companyList.SingleOrDefault(x => x.PomId == item.CompanyId)?.Shortname;
                    item.DepartmentName = institutionIds.SingleOrDefault(x => x.PomId == item.DepartmentId)?.Shortname;
                }
                
            }
            int skipCount = (searchAddedPersonnelRequestDto.PageIndex - 1) * searchAddedPersonnelRequestDto.PageSize;
            var userLists = userList
                .WhereIF(searchAddedPersonnelRequestDto.SecondaryUnit.HasValue, x => x.CompanyId == searchAddedPersonnelRequestDto.SecondaryUnit)
                .ToList();
           var user = userLists.Skip(skipCount).Take(searchAddedPersonnelRequestDto.PageSize).ToList();
            responseAjaxResult.Success();
            responseAjaxResult.Data = user;
            responseAjaxResult.Count = userLists.Count;
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取人员授权机构列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<List<AuthorizedInstitutionsReponseDto>>> SearchAuthorizedInstitutionsAsync(SearchAuthorizedInstitutionsRequestDto searchAuthorizedInstitutionsRequestDto)
        {
            RefAsync<int> total = 0;
            ResponseAjaxResult<List<AuthorizedInstitutionsReponseDto>> responseAjaxResult = new ResponseAjaxResult<List<AuthorizedInstitutionsReponseDto>>();
            List<AuthorizedInstitutionsReponseDto> authorizedInstitutionsReponseDtos = new List<AuthorizedInstitutionsReponseDto>();
            var userExtendedList = await baseUserExtendedRepository.AsQueryable()
                .Where(x => x.IsDelete == 1 && x.UserId == searchAuthorizedInstitutionsRequestDto.Id)
                .ToListAsync();
            var istitution = await dbContent.Queryable<Institution>().ToListAsync();
            #region 添加不可删除的数据
            var cmpanyIds = await baseUserRepository.AsQueryable().Where(x => x.IsDelete == 1 && x.Id == searchAuthorizedInstitutionsRequestDto.Id).SingleAsync();
           var companyName =  await dbContent.Queryable<Institution>().Where(x => x.PomId == cmpanyIds.DepartmentId && x.IsDelete == 1).Select(x=>x.Shortname).SingleAsync();
            AuthorizedInstitutionsReponseDto authorizedInstitutionsReponseDto = new AuthorizedInstitutionsReponseDto()
            {
                SCompanyName = istitution.SingleOrDefault(x=>x.IsDelete == 1&&x.PomId == cmpanyIds.CompanyId)?.Shortname,
                SCompanyId = cmpanyIds.CompanyId,
                DivisionId = cmpanyIds.DepartmentId.Value,
                DivisionName = companyName,
                
            };
            authorizedInstitutionsReponseDtos.Add(authorizedInstitutionsReponseDto);
            #endregion
            foreach (var item in userExtendedList)
            {
                 authorizedInstitutionsReponseDto = new AuthorizedInstitutionsReponseDto()
                {
                SCompanyName = istitution.SingleOrDefault(x=>x.IsDelete == 1&&x.PomId == item.CompanyId)?.Shortname,
                DivisionId = item.DepartmentId.Value,
                DivisionName = istitution.SingleOrDefault(x => x.IsDelete == 1 && x.PomId == item.DepartmentId)?.Shortname,
                SCompanyId = item.CompanyId
                 };            
                authorizedInstitutionsReponseDtos.Add(authorizedInstitutionsReponseDto);
            }        
            responseAjaxResult.Data = authorizedInstitutionsReponseDtos;
            responseAjaxResult.Count = authorizedInstitutionsReponseDtos.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        /// <summary>
        /// 添加机构授权列表
        /// </summary>
        /// <param name="addAuthorizedInstitutionsRequesDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> AddAuthorizedInstitutionsAsync(AddAuthorizedInstitutionsRequestDto addAuthorizedInstitutionsRequesDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var UserId = await dbContent.Queryable<UserExtended>().Where(x => x.UserId == addAuthorizedInstitutionsRequesDto.UserId&&x.IsDelete == 1).ToListAsync();
            if (UserId.Count>8)
            {
                responseAjaxResult.Success(ResponseMessage.OPERATION_ASSIGNMENT_FAIL);
                return responseAjaxResult;
            }
            var userInfo =await baseUserRepository.AsQueryable().SingleAsync(x => x.IsDelete == 1 && x.Id == addAuthorizedInstitutionsRequesDto.UserId);
            var userExtended = await baseUserExtendedRepository.AsQueryable()
                .Where(x=>x.IsDelete == 1 && x.UserId ==  addAuthorizedInstitutionsRequesDto.UserId  && x.DepartmentId ==addAuthorizedInstitutionsRequesDto.DivisionId&&x.CompanyId == addAuthorizedInstitutionsRequesDto.SubordinateCompaniesId).ToListAsync();
            if(userExtended.Count!=0)
            {
                responseAjaxResult.Success(ResponseMessage.OPERATION_DEPARTMENT_USER);
                return responseAjaxResult;
            }
            UserExtended userExtendeds = new UserExtended()
            {
                Id = GuidUtil.Next(),
                UserId = addAuthorizedInstitutionsRequesDto.UserId,
                DepartmentId = addAuthorizedInstitutionsRequesDto.DivisionId,
                CompanyId = addAuthorizedInstitutionsRequesDto.SubordinateCompaniesId
            };
            #region 日志信息
            var logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/系统管理/人员组织调整/组织分配",
                BusinessRemark = "/系统管理/人员组织调整/组织分配",
                OperationId = _currentUser.Id,
                OperationName = _currentUser.Name,
            };
            #endregion
             var flag = await baseUserExtendedRepository.AsInsertable(userExtendeds).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            if (flag>0)
            {
                var redis = RedisUtil.Instance;
                
                if (userInfo!=null&&!string.IsNullOrWhiteSpace(userInfo.LoginAccount))
                {
                    await redis.DelAsync(userInfo.LoginAccount);
                }
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_INSERT_SUCCESS);
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_INSERT_FAIL, Domain.Shared.Enums.HttpStatusCode.InsertFail);
            }
            return responseAjaxResult;
        }
        /// <summary>
        /// 删除人员授权机构列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<bool>> DeleteAuthorizedAutAsync(DeleteAddedPersonnelRequestDto deleteAddedPersonnelRequestDto)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new ResponseAjaxResult<bool>();
            var userExtendeds =  await dbContent.Queryable<UserExtended>()
                .Where(x => x.IsDelete == 1 && x.DepartmentId == deleteAddedPersonnelRequestDto.DepartmentId && x.UserId == deleteAddedPersonnelRequestDto.UserId&&x.CompanyId == deleteAddedPersonnelRequestDto.SubordinateCompaniesId)
                .SingleAsync();
            if (userExtendeds == null)
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DATA_NOTEXIST);
                return responseAjaxResult;
            }
            List<Guid> roleIdList = new List<Guid>();
            var userInfo = await baseUserRepository.AsQueryable().Where(x=>x.Id == deleteAddedPersonnelRequestDto.UserId&&x.IsDelete == 1).SingleAsync();
            var institutionRoleList = await dbContent
                .Queryable<InstitutionRole>().ToListAsync();
            var roleId = institutionRoleList.Where(x => x.IsDelete == 1 && x.InstitutionId == deleteAddedPersonnelRequestDto.DepartmentId && x.UserId == deleteAddedPersonnelRequestDto.UserId)
                .Select(x => x.RoleId).ToList();
            foreach (var item in roleId)
            {
                  var userRoleList =  institutionRoleList.Where(x => x.RoleId == item).ToList();
                  await roleService.RemoveRoleUserAsync(item.Value, deleteAddedPersonnelRequestDto.DepartmentId, deleteAddedPersonnelRequestDto.UserId);
                if(userRoleList.Count == 1)
                {
                    await roleService.RemoveAsync(item.Value);
                }
            }
            userExtendeds.IsDelete = 0;
            #region 日志信息
            var logDto = new LogInfo()
            {
                Id = GuidUtil.Increment(),
                BusinessModule = "/系统管理/人员组织调整/删除",
                BusinessRemark = "/系统管理/人员组织调整/删除",
                OperationId = _currentUser.Id,
                OperationName = _currentUser.Name,
            };
            #endregion
           var  flag= await baseUserExtendedRepository.AsUpdateable(userExtendeds).EnableDiffLogEvent(logDto).ExecuteCommandAsync();
            if (flag > 0)
            {
                var redis= RedisUtil.Instance;
                if (userInfo!=null&&!string.IsNullOrWhiteSpace(userInfo.LoginAccount))
                {
                    await redis.DelAsync(userInfo.LoginAccount);
                }                
                responseAjaxResult.Data = true;
                responseAjaxResult.Success(ResponseMessage.OPERATION_DELETE_SUCCESS);               
            }
            else
            {
                responseAjaxResult.Fail(ResponseMessage.OPERATION_DELETE_FAIL,Domain.Shared.Enums.HttpStatusCode.DeleteFail);
            }           
            return responseAjaxResult;
        }
        /// <summary>
        /// 人员组织调整获取所属公司下拉接口
        /// </summary>
        /// <param name="getCompanyPullDownAuthAsyncRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<GetCompanyPullDownAuthAsyncResponseDto>>> GetCompanyPullDownAuthAsync(GetCompanyPullDownAuthAsyncRequestDto getCompanyPullDownAuthAsyncRequestDto)
        {
            ResponseAjaxResult<List<GetCompanyPullDownAuthAsyncResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<GetCompanyPullDownAuthAsyncResponseDto>>();
            var grules  =  await dbContent.Queryable<Institution>().Where(x => x.IsDelete == 1 && x.PomId == getCompanyPullDownAuthAsyncRequestDto.Id).Select(x => x.Grule).SingleAsync();
            var institutionList =  await dbContent.Queryable<Institution>()
                .Where(x => x.Grule.ToString().Contains(grules.ToString()) && x.IsDelete == 1&&x.Ocode.Contains("0000P")&&x.Status !="4")
                .WhereIF(!string.IsNullOrWhiteSpace(getCompanyPullDownAuthAsyncRequestDto.Name), x => x.Name.Contains(getCompanyPullDownAuthAsyncRequestDto.Name))
                .Select(x=> new GetCompanyPullDownAuthAsyncResponseDto { Id = x.PomId.Value ,Name = x.Name})
                .ToListAsync();
            responseAjaxResult.Data = institutionList;
            responseAjaxResult.Count = institutionList.Count;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
    }
}