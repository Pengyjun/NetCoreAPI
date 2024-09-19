using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingDepartment;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeAccountingMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeDivision;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.BankCard;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.BusinessNoCpportunity;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceDetailCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.EscrowOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.NationalEconomy;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RelationalContracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RoomNumber;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ScientifiCNoProject;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using Newtonsoft.Json;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application.Service.SearchService
{
    /// <summary>
    /// 列表接口实现
    /// </summary>
    public class SearchService : ISearchService
    {
        private readonly ISqlSugarClient _dbContext;
        private readonly IMapper _mapper;
        private readonly IBaseService _baseService;
        private readonly IDataAuthorityService _dataAuthorityService;
        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="dataAuthorityService"></param>
        /// <param name="baseService"></param>
        public SearchService(ISqlSugarClient dbContext, IMapper mapper, IBaseService baseService, IDataAuthorityService dataAuthorityService)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._baseService = baseService;
            this._dataAuthorityService = dataAuthorityService;
        }
        /// <summary>
        /// 楼栋列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<LouDongSearchDto>>> GetSearchLouDongAsync(LouDongRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<LouDongSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<LouDong>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new LouDongSearchDto
                {
                    Id = cc.Id.ToString(),
                    BFormat = cc.ZFORMATINF,
                    Code = cc.ZBLDG,
                    Name = cc.ZBLDG_NAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效"
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取楼栋详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<LouDongDetailsDto>> GetLouDongDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<LouDongDetailsDto>();

            var result = await _dbContext.Queryable<LouDong>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new LouDongDetailsDto
                {
                    BFormat = cc.ZFORMATINF,
                    Code = cc.ZBLDG,
                    Name = cc.ZBLDG_NAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    PjectMDCode = cc.ZPROJECT,
                    SourceSystem = cc.ZSYSTEM
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<UserSearchDetailsDto>>> GetUserSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<UserSearchDetailsDto>>();
            RefAsync<int> total = 0;

            //过滤条件
            DeserializeObjectUser filterCondition = new DeserializeObjectUser();
            if (requestDto.FilterConditionJson != null)
            {
                filterCondition = JsonConvert.DeserializeObject<DeserializeObjectUser>(requestDto.FilterConditionJson);
            }

            //获取人员信息
            var userInfos = await _dbContext.Queryable<User>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), u => u.NAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Phone), u => u.PHONE.Contains(filterCondition.Phone))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EmpCode), u => u.EMP_CODE.Contains(filterCondition.EmpCode))
                .WhereIF(filterCondition != null && filterCondition.Ids != null && filterCondition.Ids.Any(), u => filterCondition.Ids.Contains(u.Id.ToString()))
                .Where(u => u.IsDelete == 1)
                .Select(u => new UserSearchDetailsDto
                {
                    Id = u.Id.ToString(),
                    Name = u.NAME,
                    CertNo = u.CERT_NO,
                    OfficeDepId = u.OFFICE_DEPID,
                    Email = u.EMAIL,
                    EmpCode = u.EMP_CODE,
                    Enable = u.Enable == 1 ? "启用" : "禁用",
                    Phone = u.PHONE,
                    Attribute1 = u.ATTRIBUTE1,
                    Attribute2 = u.ATTRIBUTE2,
                    Attribute3 = u.ATTRIBUTE3,
                    Attribute4 = u.ATTRIBUTE4,
                    Attribute5 = u.ATTRIBUTE5,
                    Birthday = u.BIRTHDAY,
                    CertType = u.CERT_TYPE,
                    DispatchunitName = u.DISPATCHUNITNAME,
                    DispatchunitShortName = u.DISPATCHUNITSHORTNAME,
                    EmpSort = u.EMP_SORT,
                    EnName = u.EN_NAME,
                    EntryTime = u.ENTRY_TIME,
                    Externaluser = u.EXTERNALUSER,
                    Fax = u.FAX,
                    HighEstGrade = u.HIGHESTGRADE,
                    HrEmpCode = u.HR_EMP_CODE,
                    JobName = u.JOB_NAME,
                    JobType = u.JOB_TYPE,
                    NameSpell = u.NAME_SPELL,
                    Nation = u.NATION,
                    Nationality = u.NATIONALITY,
                    OfficeNum = u.OFFICE_NUM,
                    PoliticsFace = u.POLITICSFACE,
                    PositionGrade = u.POSITION_GRADE,
                    PositionGradeNorm = u.POSITIONGRADENORM,
                    PositionName = u.POSITION_NAME,
                    Positions = u.POSITIONS,
                    SameHighEstGrade = u.SAMEHIGHESTGRADE,
                    Sex = u.SEX == "01" ? "男性" : "女性",
                    Sno = u.SNO,
                    SubDepts = u.SUB_DEPTS,
                    Tel = u.TEL,
                    UserLogin = u.USER_LOGIN
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            if (userInfos != null && userInfos.Any())
            {
                //机构信息
                var institutions = await _dbContext.Queryable<Institution>()
                    .Where(t => t.IsDelete == 1)
                    .Select(t => new UInstutionDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
                    .ToListAsync();

                #region 处理其他基本信息数据
                //国籍
                var cr = userInfos.Where(x => !string.IsNullOrWhiteSpace(x.CountryRegion)).Select(x => x.CountryRegion).ToList();
                var contryRegion = await _dbContext.Queryable<CountryRegion>()
                    .Where(t => t.IsDelete == 1 && cr.Contains(t.ZCOUNTRYCODE))
                    .Select(t => new { t.ZCOUNTRYCODE, t.ZCOUNTRYNAME })
                    .ToListAsync();

                //用户状态
                var us = userInfos.Where(x => !string.IsNullOrWhiteSpace(x.UserInfoStatus)).Select(x => x.UserInfoStatus).ToList();
                var uStatus = await _dbContext.Queryable<UserStatus>().Where(t => t.IsDelete == 1 && us.Contains(t.OneCode)).Select(x => new { x.OneCode, x.OneName }).ToListAsync();

                //民族

                #endregion

                foreach (var uInfo in userInfos)
                {
                    uInfo.CompanyName = GetUserCompany(uInfo.OfficeDepId, institutions);
                    uInfo.OfficeDepIdName = institutions.FirstOrDefault(x => x.Oid == uInfo.OfficeDepId)?.Name;
                    uInfo.UserInfoStatus = uStatus.FirstOrDefault(x => x.OneCode == uInfo.UserInfoStatus)?.OneName;
                    uInfo.CountryRegion = contryRegion.FirstOrDefault(x => x.ZCOUNTRYCODE == uInfo.CountryRegion)?.ZCOUNTRYNAME;
                }
            }

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(userInfos);
            return responseAjaxResult;
        }
        /// <summary>
        /// 用户详情id
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<UserSearchDetailsDto>> GetUserDetailsAsync(string uId)
        {
            var responseAjaxResult = new ResponseAjaxResult<UserSearchDetailsDto>();

            var uDetails = await _dbContext.Queryable<User>()
                .LeftJoin<UserStatus>((u, us) => u.EMP_STATUS == us.OneCode)
                .LeftJoin<Institution>((u, us, ins) => u.OFFICE_DEPID == ins.OID)
                .Where((u, us, ins) => !string.IsNullOrWhiteSpace(u.EMP_STATUS) && u.IsDelete == 1 && u.Id.ToString() == uId)
                .Select((u, us, ins) => new UserSearchDetailsDto
                {
                    Name = u.NAME,
                    CertNo = u.CERT_NO,
                    OfficeDepId = u.OFFICE_DEPID,
                    Email = u.EMAIL,
                    EmpCode = u.EMP_CODE,
                    Enable = u.Enable == 1 ? "启用" : "禁用",
                    UserInfoStatus = us.OneName,
                    Phone = u.PHONE,
                    OfficeDepIdName = ins.NAME,
                    Attribute1 = u.ATTRIBUTE1,
                    Attribute2 = u.ATTRIBUTE2,
                    Attribute3 = u.ATTRIBUTE3,
                    Attribute4 = u.ATTRIBUTE4,
                    Attribute5 = u.ATTRIBUTE5,
                    Birthday = u.BIRTHDAY,
                    CertType = u.CERT_TYPE,
                    DispatchunitName = u.DISPATCHUNITNAME,
                    DispatchunitShortName = u.DISPATCHUNITSHORTNAME,
                    EmpSort = u.EMP_SORT,
                    EnName = u.EN_NAME,
                    EntryTime = u.ENTRY_TIME,
                    Externaluser = u.EXTERNALUSER,
                    Fax = u.FAX,
                    HighEstGrade = u.HIGHESTGRADE,
                    HrEmpCode = u.HR_EMP_CODE,
                    JobName = u.JOB_NAME,
                    JobType = u.JOB_TYPE,
                    NameSpell = u.NAME_SPELL,
                    Nation = u.NATION,
                    Nationality = u.NATIONALITY,
                    OfficeNum = u.OFFICE_NUM,
                    PoliticsFace = u.POLITICSFACE,
                    PositionGrade = u.POSITION_GRADE,
                    PositionGradeNorm = u.POSITIONGRADENORM,
                    PositionName = u.POSITION_NAME,
                    Positions = u.POSITIONS,
                    SameHighEstGrade = u.SAMEHIGHESTGRADE,
                    Sex = u.SEX == "01" ? "男性" : "女性",
                    Sno = u.SNO,
                    SubDepts = u.SUB_DEPTS,
                    Tel = u.TEL,
                    UserLogin = u.USER_LOGIN
                })
                .FirstAsync();

            #region 其他基本信息
            //获取机构信息
            var institutions = await _dbContext.Queryable<Institution>()
                .Where(t => t.IsDelete == 1)
                .Select(t => new UInstutionDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
                .ToListAsync();
            uDetails.CompanyName = GetUserCompany(uDetails.OfficeDepId, institutions);

            //国籍
            var name = await _dbContext.Queryable<CountryRegion>().FirstAsync(t => t.IsDelete == 1 && uDetails.Nationality == t.ZCOUNTRYCODE);
            uDetails.Nationality = name == null ? null : name.ZCOUNTRYNAME;

            #endregion

            responseAjaxResult.SuccessResult(uDetails);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取用户所属公司
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="uInstutionDtos"></param>
        /// <returns></returns>
        public string GetUserCompany(string oid, List<UInstutionDto> uInstutionDtos)
        {
            var uInsInfo = uInstutionDtos.FirstOrDefault(x => x.Oid == oid);
            if (uInsInfo != null)
            {
                //获取用户机构全部规则  反查机构信息
                if (!string.IsNullOrWhiteSpace(uInsInfo.Grule))
                {
                    var ruleIds = uInsInfo.Grule.Trim('-').Split('-').ToList();
                    var companyNames = ruleIds
                        .Select(id => uInstutionDtos.FirstOrDefault(inst => inst.Oid == id))
                        .Where(inst => inst != null)
                        .Select(inst => inst?.Name)
                        .ToList();

                    if (companyNames.Any())
                    {
                        return string.Join("/", companyNames);
                    }
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 机构数列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<InstitutionDetatilsDto>>> GetInstitutionAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<InstitutionDetatilsDto>>();
            var result = new List<InstitutionDetatilsDto>();
            List<string> oids = new();

            //过滤条件
            DeserializeObjectUser filterCondition = new DeserializeObjectUser();
            if (requestDto.FilterConditionJson != null)
            {
                filterCondition = JsonConvert.DeserializeObject<DeserializeObjectUser>(requestDto.FilterConditionJson);
            }

            //机构树初始化
            var institutions = await _dbContext.Queryable<Institution>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), t => t.NAME.Contains(filterCondition.Name))
                .Where(t => t.IsDelete == 1)
                .OrderBy(t => Convert.ToInt32(t.SNO))
                .ToListAsync();

            if (filterCondition != null)
            {
                //机构ids不为空
                if (filterCondition.Ids != null && filterCondition.Ids.Any())
                {
                    //得到所有符合条件的机构ids
                    var ids = institutions
                        .WhereIF(filterCondition != null && filterCondition.Ids != null && filterCondition.Ids.Any(), t => filterCondition.Ids.Contains(t.Id.ToString()))
                        .Where(t => t.IsDelete == 1)
                        .Select(x => x.Id)
                        .ToList();

                    //得到所有符合条件的机构rules
                    var rules = institutions.Where(x => ids.Contains(x.Id)).Select(x => x.GRULE).ToList();

                    //拆分rules得到所有符合条件的oids
                    foreach (var r in rules)
                    {
                        if (!string.IsNullOrWhiteSpace(r)) oids.AddRange(r.Split('-'));
                    }
                }
            }
            //其他数据
            var otherNodes = institutions
                .WhereIF(oids != null && oids.Any(), ins => oids.Contains(ins.OID))
                .Where(ins => ins.OID != "101162350")
                .Select(ins => new InstitutionDetatilsDto
                {
                    Id = ins.Id.ToString(),
                    BizDomain = ins.BIZDOMAIN,
                    BizType = ins.BIZTYPE,
                    Carea = ins.CAREA,
                    Code = ins.OCODE,
                    Coid = ins.COID,
                    Crossorgan = ins.CROSSORGAN,
                    EnglishName = ins.ENGLISHNAME,
                    EnglishShortName = ins.ENGLISHSHORTNAME,
                    EntClass = ins.ENTCLASS,
                    GlobalSno = ins.GLOBAL_SNO,
                    Goid = ins.GOID,
                    Gpoid = ins.GPOID,
                    Grade = ins.GRADE,
                    IsIndependent = ins.IS_INDEPENDENT,
                    MdmCode = ins.MDM_CODE,
                    Mrut = ins.MRUT,
                    Name = ins.NAME,
                    Note = ins.NOTE,
                    Oid = ins.OID,
                    Oper = ins.OPER,
                    Organemp = ins.ORGANEMP,
                    OrganGrade = ins.ORGANGRADE,
                    OrgGrade = ins.ORGGRADE,
                    OrgProvince = ins.ORGPROVINCE,
                    Orule = ins.ORULE,
                    OSecBid = ins.O2BID,
                    POid = ins.POID,
                    ProjectManType = ins.PROJECTMANTYPE,
                    ProjectScale = ins.PROJECTSCALE,
                    ProjectType = ins.PROJECTTYPE,
                    QyGrade = ins.QYGRADE,
                    RegisterCode = ins.REGISTERCODE,
                    RoId = ins.ROID,
                    Rown = ins.ROWN,
                    ShareHoldings = ins.SHAREHOLDINGS,
                    ShortName = ins.SHORTNAME,
                    Sno = ins.SNO,
                    StartDate = ins.STARTDATE,
                    Status = ins.STATUS,
                    TemorganName = ins.TEMORGANNAME,
                    TerritoryPro = ins.TERRITORYPRO,
                    Type = ins.TYPE,
                    TypeExt = ins.TYPEEXT,
                    Version = ins.VERSION
                })
                .ToList();

            //根节点
            var rootNode = institutions
                .WhereIF(oids != null && oids.Any(), ins => oids.Contains(ins.OID))
                .Where(ins => ins.OID == "101162350")
                .Select(ins => new InstitutionDetatilsDto
                {
                    Id = ins.Id.ToString(),
                    BizDomain = ins.BIZDOMAIN,
                    BizType = ins.BIZTYPE,
                    Carea = ins.CAREA,
                    Code = ins.OCODE,
                    Coid = ins.COID,
                    Crossorgan = ins.CROSSORGAN,
                    EnglishName = ins.ENGLISHNAME,
                    EnglishShortName = ins.ENGLISHSHORTNAME,
                    EntClass = ins.ENTCLASS,
                    GlobalSno = ins.GLOBAL_SNO,
                    Goid = ins.GOID,
                    Gpoid = ins.GPOID,
                    Grade = ins.GRADE,
                    IsIndependent = ins.IS_INDEPENDENT,
                    MdmCode = ins.MDM_CODE,
                    Mrut = ins.MRUT,
                    Name = ins.NAME,
                    Note = ins.NOTE,
                    Oid = ins.OID,
                    Oper = ins.OPER,
                    Organemp = ins.ORGANEMP,
                    OrganGrade = ins.ORGANGRADE,
                    OrgGrade = ins.ORGGRADE,
                    OrgProvince = ins.ORGPROVINCE,
                    Orule = ins.ORULE,
                    OSecBid = ins.O2BID,
                    POid = ins.POID,
                    ProjectManType = ins.PROJECTMANTYPE,
                    ProjectScale = ins.PROJECTSCALE,
                    ProjectType = ins.PROJECTTYPE,
                    QyGrade = ins.QYGRADE,
                    RegisterCode = ins.REGISTERCODE,
                    RoId = ins.ROID,
                    Rown = ins.ROWN,
                    ShareHoldings = ins.SHAREHOLDINGS,
                    ShortName = ins.SHORTNAME,
                    Sno = ins.SNO,
                    StartDate = ins.STARTDATE,
                    Status = ins.STATUS,
                    TemorganName = ins.TEMORGANNAME,
                    TerritoryPro = ins.TERRITORYPRO,
                    Type = ins.TYPE,
                    TypeExt = ins.TYPEEXT,
                    Version = ins.VERSION,
                    Children = GetChildren(ins.OID, otherNodes)
                }).FirstOrDefault();
            result.Add(rootNode);

            responseAjaxResult.SuccessResult(result);
            responseAjaxResult.Count = result.Count;
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="gpOid"></param>
        /// <param name="children"></param>
        /// <returns></returns>
        public List<InstitutionDetatilsDto> GetChildren(string gpOid, List<InstitutionDetatilsDto> children)
        {
            var childs = children.Where(x => x.Gpoid == gpOid).ToList();
            foreach (var child in childs)
            {
                child.Children = GetChildren(child.Oid, children);
            }
            return childs;
        }

        /// <summary>
        /// 获取机构详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<InstitutionDetatilsDto>> GetInstitutionDetailsAsync(string Id)
        {
            var responseAjaxResult = new ResponseAjaxResult<InstitutionDetatilsDto>();

            var insDetails = await _dbContext.Queryable<Institution>()
                .Where((ins) => ins.IsDelete == 1 && ins.Id.ToString() == Id)
                .Select((ins) => new InstitutionDetatilsDto
                {
                    BizDomain = ins.BIZDOMAIN,
                    BizType = ins.BIZTYPE,
                    Carea = ins.CAREA,
                    Code = ins.OCODE,
                    Coid = ins.COID,
                    Crossorgan = ins.CROSSORGAN,
                    EnglishName = ins.ENGLISHNAME,
                    EnglishShortName = ins.ENGLISHSHORTNAME,
                    EntClass = ins.ENTCLASS,
                    GlobalSno = ins.GLOBAL_SNO,
                    Goid = ins.GOID,
                    Gpoid = ins.GPOID,
                    Grade = ins.GRADE,
                    IsIndependent = ins.IS_INDEPENDENT,
                    MdmCode = ins.MDM_CODE,
                    Mrut = ins.MRUT,
                    Name = ins.NAME,
                    Note = ins.NOTE,
                    Oid = ins.OID,
                    Oper = ins.OPER,
                    Organemp = ins.ORGANEMP,
                    OrganGrade = ins.ORGANGRADE,
                    OrgGrade = ins.ORGGRADE,
                    OrgProvince = ins.ORGPROVINCE,
                    Orule = ins.ORULE,
                    OSecBid = ins.O2BID,
                    POid = ins.POID,
                    ProjectManType = ins.PROJECTMANTYPE,
                    ProjectScale = ins.PROJECTSCALE,
                    ProjectType = ins.PROJECTTYPE,
                    QyGrade = ins.QYGRADE,
                    RegisterCode = ins.REGISTERCODE,
                    RoId = ins.ROID,
                    Rown = ins.ROWN,
                    ShareHoldings = ins.SHAREHOLDINGS,
                    ShortName = ins.SHORTNAME,
                    Sno = ins.SNO,
                    StartDate = ins.STARTDATE,
                    Status = ins.STATUS,
                    TemorganName = ins.TEMORGANNAME,
                    TerritoryPro = ins.TERRITORYPRO,
                    Type = ins.TYPE,
                    TypeExt = ins.TYPEEXT,
                    Version = ins.VERSION
                })
                .FirstAsync();

            #region 处理其他基本信息数据

            #endregion


            responseAjaxResult.SuccessResult(insDetails);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectDetailsDto>>> GetProjectSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectDetailsDto>>();
            RefAsync<int> total = 0;

            var proList = await _dbContext.Queryable<Project>()
                .Where((pro) => pro.IsDelete == 1)
                .Select((pro) => new ProjectDetailsDto
                {
                    Id = pro.Id.ToString(),
                    Abbreviation = pro.ZHEREINAFTER,
                    Country = pro.ZZCOUNTRY,
                    Currency = pro.ZZCURRENCY,
                    EngineeringName = pro.ZENG,
                    ForeignName = pro.ZPROJENAME,
                    Location = pro.ZPROJLOC,
                    MDCode = pro.ZPROJECT,
                    Name = pro.ZPROJNAME,
                    PjectOrg = pro.ZPRO_ORG,
                    PjectOrgBP = pro.ZPRO_BP,
                    PlanCompletionDate = pro.ZFINDATE,
                    PlanStartDate = pro.ZSTARTDATE,
                    ResolutionTime = pro.ZAPVLDATE,
                    ResolutionNo = pro.ZAPPROVAL,
                    State = pro.ZSTATE == "1" ? "有效" : "无效",
                    Type = pro.ZPROJTYPE,
                    UnitSec = pro.Z2NDORG,
                    Year = pro.ZPROJYEAR,
                    AcquisitionTime = pro.ZLDLOCGT,
                    Applicant = pro.ZINSURED,
                    BChangeTime = pro.ZCBR,
                    BDep = pro.ZBIZDEPT,
                    BidDisclosureNo = pro.ZAWARDP,
                    BTypeOfCCCC = pro.ZCPBC,
                    ConsolidatedTable = pro.ZCS,
                    CreateDate = pro.ZCREATE_AT,
                    CustodianName = pro.ZCUSTODIAN,
                    DueDate = pro.ZLFINDATE,
                    EndDateOfInsure = pro.ZIFINDATE,
                    FundEstablishmentDate = pro.ZFSTARTDATE,
                    FundExpirationDate = pro.ZFFINDATE,
                    FundManager = pro.ZFUNDMTYPE,
                    FundMDCode = pro.ZFUND,
                    FundName = pro.ZFUNDNAME,
                    FundNo = pro.ZFUNDNO,
                    FundOrgForm = pro.ZFUNDORGFORM,
                    Invest = pro.ZINVERSTOR,
                    IsJoint = pro.ZWINNINGC,
                    LandTransactionNo = pro.ZLDLOC,
                    LeaseStartDate = pro.ZLSTARTDATE,
                    Management = pro.ZMANAGE_MODE,
                    NameOfInsureOrg = pro.ZINSURANCE,
                    NameOfLeased = pro.ZLEASESNAME,
                    OrgMethod = pro.ZPOS,
                    ParticipateInUnitSecs = pro.ZCY2NDORG,
                    PolicyNo = pro.ZPOLICYNO,
                    ReasonForDeactivate = pro.ZSTOPREASON,
                    ResponsibleParty = pro.ZRESP,
                    SourceOfIncome = pro.ZSI,
                    StartDateOfInsure = pro.ZISTARTDATE,
                    SupMDCode = pro.ZPROJECTUP,
                    TaxMethod = pro.ZTAXMETHOD,
                    TenantName = pro.ZLESSEE,
                    TenantType = pro.ZLESSEETYPE,
                    TradingSituation = pro.ZTRADER,
                    WinningBidder = pro.ZAWARDMAI
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(proList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ProjectDetailsDto>> GetProjectDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<ProjectDetailsDto>();

            var result = await _dbContext.Queryable<Project>()
                .LeftJoin<Institution>((pro, ins) => pro.ZPRO_ORG == ins.OID)
                .Where((pro, ins) => pro.IsDelete == 1 && pro.Id.ToString() == id)
                .Select((pro, ins) => new ProjectDetailsDto
                {
                    Abbreviation = pro.ZHEREINAFTER,
                    Country = pro.ZZCOUNTRY,
                    Currency = pro.ZZCURRENCY,
                    EngineeringName = pro.ZENG,
                    ForeignName = pro.ZPROJENAME,
                    Location = pro.ZPROJLOC,
                    MDCode = pro.ZPROJECT,
                    Name = pro.ZPROJNAME,
                    PjectOrg = pro.ZPRO_ORG,
                    PjectOrgBP = pro.ZPRO_BP,
                    PlanCompletionDate = pro.ZFINDATE,
                    PlanStartDate = pro.ZSTARTDATE,
                    ResolutionTime = pro.ZAPVLDATE,
                    ResolutionNo = pro.ZAPPROVAL,
                    State = pro.ZSTATE == "1" ? "有效" : "无效",
                    Type = pro.ZPROJTYPE,
                    UnitSec = pro.Z2NDORG,
                    Year = pro.ZPROJYEAR,
                    AcquisitionTime = pro.ZLDLOCGT,
                    Applicant = pro.ZINSURED,
                    BChangeTime = pro.ZCBR,
                    BDep = pro.ZBIZDEPT,
                    BidDisclosureNo = pro.ZAWARDP,
                    BTypeOfCCCC = pro.ZCPBC,
                    ConsolidatedTable = pro.ZCS,
                    CreateDate = pro.ZCREATE_AT,
                    CustodianName = pro.ZCUSTODIAN,
                    DueDate = pro.ZLFINDATE,
                    EndDateOfInsure = pro.ZIFINDATE,
                    FundEstablishmentDate = pro.ZFSTARTDATE,
                    FundExpirationDate = pro.ZFFINDATE,
                    FundManager = pro.ZFUNDMTYPE,
                    FundMDCode = pro.ZFUND,
                    FundName = pro.ZFUNDNAME,
                    FundNo = pro.ZFUNDNO,
                    FundOrgForm = pro.ZFUNDORGFORM,
                    Invest = pro.ZINVERSTOR,
                    IsJoint = pro.ZWINNINGC,
                    LandTransactionNo = pro.ZLDLOC,
                    LeaseStartDate = pro.ZLSTARTDATE,
                    Management = pro.ZMANAGE_MODE,
                    NameOfInsureOrg = pro.ZINSURANCE,
                    NameOfLeased = pro.ZLEASESNAME,
                    OrgMethod = pro.ZPOS,
                    ParticipateInUnitSecs = pro.ZCY2NDORG,
                    PolicyNo = pro.ZPOLICYNO,
                    ReasonForDeactivate = pro.ZSTOPREASON,
                    ResponsibleParty = pro.ZRESP,
                    SourceOfIncome = pro.ZSI,
                    StartDateOfInsure = pro.ZISTARTDATE,
                    SupMDCode = pro.ZPROJECTUP,
                    TaxMethod = pro.ZTAXMETHOD,
                    TenantName = pro.ZLESSEE,
                    TenantType = pro.ZLESSEETYPE,
                    TradingSituation = pro.ZTRADER,
                    WinningBidder = pro.ZAWARDMAI
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 往来单位列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<CorresUnitDetailsDto>>> GetCorresUnitSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<CorresUnitDetailsDto>>();
            RefAsync<int> total = 0;

            var corresUnitList = await _dbContext.Queryable<CorresUnit>()
                .Where((cu) => cu.IsDelete == 1)
                .Select((cu) => new CorresUnitDetailsDto
                {
                    Id = cu.Id.ToString(),
                    CategoryUnit = cu.ZBPTYPE,
                    City = cu.ZCITY,
                    Country = cu.ZZCOUNTRY,
                    Name = cu.ZBPNAME_ZH,
                    NameEnglish = cu.ZBPNAME_EN,
                    NameInLLanguage = cu.ZBPNAME_LOC,
                    County = cu.ZCOUNTY,
                    NatureOfUnit = cu.ZBPNATURE,
                    Province = cu.ZPROVINCE,
                    TypeOfUnit = cu.ZBPKINDS,
                    AbroadRegistrationNo = cu.ZOSRNO,
                    AbroadSocialSecurityNo = cu.ZSSNO,
                    AccUnitCode = cu.ZACORGNO,
                    BRegistrationNo = cu.ZBRNO,
                    ChangeTime = cu.ZCHAT,
                    CreateBy = cu.ZCRBY,
                    CreatTime = cu.ZCRAT,
                    DealUnitMDCode = cu.ZBP,
                    EnterpriseNature = cu.ZETPSPROPERTY,
                    IdNo = cu.ZIDNO,
                    IsGroupUnit = cu.ZINCLIENT,
                    ModifiedBy = cu.ZCHBY,
                    OrgCode = cu.ZOIBC,
                    OrgMDCode = cu.ZORG,
                    RegistrationNo = cu.ZUSCC,
                    SourceSystem = cu.ZSYSTEM,
                    SupLegalEntity = cu.ZCOMPYREL,
                    TaxpayerIdentifyNo = cu.ZTRNO,
                    UnitSec = cu.Z2NDORG,
                    StatusOfUnit = cu.ZBPSTATE == "01" ? "启用" : "停用"
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            #region 其他基本

            #endregion

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(corresUnitList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取往来单位详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<CorresUnitDetailsDto>> GetCorresUnitDetailAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<CorresUnitDetailsDto>();

            var result = await _dbContext.Queryable<CorresUnit>()
                .Where((cu) => cu.IsDelete == 1 && cu.Id.ToString() == id)
                .Select((cu) => new CorresUnitDetailsDto
                {
                    CategoryUnit = cu.ZBPTYPE,
                    City = cu.ZCITY,
                    Country = cu.ZZCOUNTRY,
                    Name = cu.ZBPNAME_ZH,
                    NameEnglish = cu.ZBPNAME_EN,
                    NameInLLanguage = cu.ZBPNAME_LOC,
                    County = cu.ZCOUNTY,
                    NatureOfUnit = cu.ZBPNATURE,
                    Province = cu.ZPROVINCE,
                    TypeOfUnit = cu.ZBPKINDS,
                    AbroadRegistrationNo = cu.ZOSRNO,
                    AbroadSocialSecurityNo = cu.ZSSNO,
                    AccUnitCode = cu.ZACORGNO,
                    BRegistrationNo = cu.ZBRNO,
                    ChangeTime = cu.ZCHAT,
                    CreateBy = cu.ZCRBY,
                    CreatTime = cu.ZCRAT,
                    DealUnitMDCode = cu.ZBP,
                    EnterpriseNature = cu.ZETPSPROPERTY,
                    IdNo = cu.ZIDNO,
                    IsGroupUnit = cu.ZINCLIENT,
                    ModifiedBy = cu.ZCHBY,
                    OrgCode = cu.ZOIBC,
                    OrgMDCode = cu.ZORG,
                    RegistrationNo = cu.ZUSCC,
                    SourceSystem = cu.ZSYSTEM,
                    SupLegalEntity = cu.ZCOMPYREL,
                    TaxpayerIdentifyNo = cu.ZTRNO,
                    UnitSec = cu.Z2NDORG,
                    StatusOfUnit = cu.ZBPSTATE == "01" ? "启用" : "停用"
                })
                 .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取国家地区列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<CountryRegionDetailsDto>>> GetCountryRegionSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<CountryRegionDetailsDto>>();
            RefAsync<int> total = 0;

            var corresUnitList = await _dbContext.Queryable<CountryRegion>()
                .Where((cr) => cr.IsDelete == 1)
                .Select((cr) => new CountryRegionDetailsDto
                {
                    Id = cr.Id.ToString(),
                    Country = cr.ZCOUNTRYCODE,
                    DigitCode = cr.ZGBNUM,
                    Name = cr.ZCOUNTRYNAME,
                    NameEnglish = cr.ZCOUNTRYENAME,
                    NationalCode = cr.ZGBCHAR,
                    State = cr.ZSTATE,
                    AreaCode = cr.ZAREACODE,
                    CCCCCenterCode = cr.ZCRCCODE,
                    ContinentCode = cr.ZCONTINENTCODE,
                    DataIdentifier = cr.ZDELETE,
                    RoadGongJ = cr.ZBRGJ,
                    RoadGuoZiW = cr.ZBRGZW,
                    RoadHaiW = cr.ZBRHW,
                    Version = cr.ZVERSION
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(corresUnitList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取国家地区详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<CountryRegionDetailsDto>> GetCountryRegionDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<CountryRegionDetailsDto>();

            var result = await _dbContext.Queryable<CountryRegion>()
                .Where((cr) => cr.IsDelete == 1 && cr.Id.ToString() == id)
                .Select((cr) => new CountryRegionDetailsDto
                {
                    Country = cr.ZCOUNTRYCODE,
                    DigitCode = cr.ZGBNUM,
                    Name = cr.ZCOUNTRYNAME,
                    NameEnglish = cr.ZCOUNTRYENAME,
                    NationalCode = cr.ZGBCHAR,
                    State = cr.ZSTATE,
                    AreaCode = cr.ZAREACODE,
                    CCCCCenterCode = cr.ZCRCCODE,
                    ContinentCode = cr.ZCONTINENTCODE,
                    DataIdentifier = cr.ZDELETE,
                    RoadGongJ = cr.ZBRGJ,
                    RoadGuoZiW = cr.ZBRGZW,
                    RoadHaiW = cr.ZBRHW,
                    Version = cr.ZVERSION
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 大洲列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<CountryContinentDetailsDto>>> GetCountryContinentSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<CountryContinentDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<CountryContinent>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new CountryContinentDetailsDto
                {
                    Id = cc.Id.ToString(),
                    ContinentCode = cc.ZCONTINENTCODE,
                    Name = cc.ZCONTINENTNAME,
                    RegionalDescr = cc.ZAREANAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    AreaCode = cc.ZAREACODE,
                    DataIdentifier = cc.ZDELETE,
                    Version = cc.ZVERSION
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取大洲详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<CountryContinentDetailsDto>> GetCountryContinentDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<CountryContinentDetailsDto>();

            var result = await _dbContext.Queryable<CountryContinent>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new CountryContinentDetailsDto
                {
                    ContinentCode = cc.ZCONTINENTCODE,
                    Name = cc.ZCONTINENTNAME,
                    RegionalDescr = cc.ZAREANAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    AreaCode = cc.ZAREACODE,
                    DataIdentifier = cc.ZDELETE,
                    Version = cc.ZVERSION
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取金融机构列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<FinancialInstitutionDetailsDto>>> GetFinancialInstitutionSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<FinancialInstitutionDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<FinancialInstitution>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new FinancialInstitutionDetailsDto
                {
                    Id = cc.Id.ToString(),
                    City = cc.ZCITY,
                    Country = cc.ZZCOUNTRY,
                    County = cc.ZCOUNTY,
                    EnglishName = cc.ZFINAME_E,
                    Name = cc.ZBANKNAME,
                    NameOfOrg = cc.ZFINAME,
                    Province = cc.ZPROVINCE,
                    TypesOfAbroadOrg = cc.ZOFITYPE,
                    TypesOfOrg = cc.ZDFITYPE,
                    State = cc.ZDATSTATE == "1" ? "有效" : "无效",
                    BankNo = cc.ZBANKN,
                    DataIdentifier = cc.ZDELETE,
                    MDCode = cc.ZFINC,
                    MDCodeofOrg = cc.ZORG,
                    RegistrationNo = cc.ZUSCC,
                    No = cc.ZBANK,
                    SubmitBy = cc.ZFZCHBY,
                    SubmitTime = cc.ZFZCHAT,
                    SwiftCode = cc.ZSWIFTCOD,
                    UnitSec = cc.ZFIN2NDORG
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取金融机构详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<FinancialInstitutionDetailsDto>> GetFinancialInstitutionDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<FinancialInstitutionDetailsDto>();

            var result = await _dbContext.Queryable<FinancialInstitution>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new FinancialInstitutionDetailsDto
                {
                    City = cc.ZCITY,
                    Country = cc.ZZCOUNTRY,
                    County = cc.ZCOUNTY,
                    EnglishName = cc.ZFINAME_E,
                    Name = cc.ZBANKNAME,
                    NameOfOrg = cc.ZFINAME,
                    Province = cc.ZPROVINCE,
                    TypesOfAbroadOrg = cc.ZOFITYPE,
                    TypesOfOrg = cc.ZDFITYPE,
                    State = cc.ZDATSTATE == "1" ? "有效" : "无效",
                    BankNo = cc.ZBANKN,
                    DataIdentifier = cc.ZDELETE,
                    MDCode = cc.ZFINC,
                    MDCodeofOrg = cc.ZORG,
                    RegistrationNo = cc.ZUSCC,
                    No = cc.ZBANK,
                    SubmitBy = cc.ZFZCHBY,
                    SubmitTime = cc.ZFZCHAT,
                    SwiftCode = cc.ZSWIFTCOD,
                    UnitSec = cc.ZFIN2NDORG
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 物资设备分类编码列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DeviceClassCodeDetailsDto>>> GetDeviceClassCodeSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DeviceClassCodeDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<DeviceClassCode>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new DeviceClassCodeDetailsDto
                {
                    Id = cc.Id.ToString(),
                    AliasName = cc.ZCALIAS,
                    Code = cc.ZCLASS,
                    Description = cc.ZCDESC,
                    Name = cc.ZCNAME,
                    UnitOfMeasurement = cc.ZMSEHI,
                    State = cc.ZUSSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE,
                    Level = cc.ZCLEVEL,
                    SortRule = cc.ZSORT,
                    SupCode = cc.ZCLASSUP
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 物资设备分类编码明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<DeviceClassCodeDetailsDto>> GetDeviceClassCodeDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<DeviceClassCodeDetailsDto>();

            var result = await _dbContext.Queryable<DeviceClassCode>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new DeviceClassCodeDetailsDto
                {
                    AliasName = cc.ZCALIAS,
                    Code = cc.ZCLASS,
                    Description = cc.ZCDESC,
                    Name = cc.ZCNAME,
                    UnitOfMeasurement = cc.ZMSEHI,
                    State = cc.ZUSSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE,
                    Level = cc.ZCLEVEL,
                    SortRule = cc.ZSORT,
                    SupCode = cc.ZCLASSUP
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 发票类型列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<InvoiceTypeDetailshDto>>> GetInvoiceTypeSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<InvoiceTypeDetailshDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<InvoiceType>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new InvoiceTypeDetailshDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZINVTCODE,
                    Name = cc.ZINVTNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE,
                    Version = cc.ZVERSION,
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 发票类型明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<InvoiceTypeDetailshDto>> GetInvoiceTypeDetailsASync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<InvoiceTypeDetailshDto>();

            var result = await _dbContext.Queryable<InvoiceType>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new InvoiceTypeDetailshDto
                {
                    Code = cc.ZINVTCODE,
                    Name = cc.ZINVTNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE,
                    Version = cc.ZVERSION,
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 科研项目列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ScientifiCNoProjectDetailsDto>>> GetScientifiCNoProjectSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ScientifiCNoProjectDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<ScientifiCNoProject>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new ScientifiCNoProjectDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZSRPN,
                    CurrencyOfCost = cc.ZPROJCOSTCUR,
                    ForeignName = cc.ZSRPN_FN,
                    IsHighTech = cc.ZHITECH == "1" ? "是" : "否",
                    MDCode = cc.ZSRP,
                    PjectState = cc.ZKPSTATE,
                    TotalCost = cc.ZPROJCOST,
                    Year = cc.ZPROJYEA,
                    IsOutsourced = cc.ZOUTSOURCING == "1" ? "是" : "否",
                    PlanEndDate = cc.ZPFINDATE,
                    PlanStartDate = cc.ZPSTARTDATE,
                    ProfessionalType = cc.ZMAJORTYPE,
                    State = cc.ZPSTATE,
                    SupMDCode = cc.ZSRPUP,
                    TypeCode = cc.ZSRPCLASS
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 科研项目明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ScientifiCNoProjectDetailsDto>> GetScientifiCNoProjectDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<ScientifiCNoProjectDetailsDto>();

            var result = await _dbContext.Queryable<ScientifiCNoProject>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new ScientifiCNoProjectDetailsDto
                {
                    Name = cc.ZSRPN,
                    CurrencyOfCost = cc.ZPROJCOSTCUR,
                    ForeignName = cc.ZSRPN_FN,
                    IsHighTech = cc.ZHITECH == "1" ? "是" : "否",
                    MDCode = cc.ZSRP,
                    PjectState = cc.ZKPSTATE,
                    TotalCost = cc.ZPROJCOST,
                    Year = cc.ZPROJYEA,
                    IsOutsourced = cc.ZOUTSOURCING == "1" ? "是" : "否",
                    PlanEndDate = cc.ZPFINDATE,
                    PlanStartDate = cc.ZPSTARTDATE,
                    ProfessionalType = cc.ZMAJORTYPE,
                    State = cc.ZPSTATE,
                    SupMDCode = cc.ZSRPUP,
                    TypeCode = cc.ZSRPCLASS
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取房号列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<RoomNumberDetailsDto>>> GetRoomNumberSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<RoomNumberDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<RoomNumber>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new RoomNumberDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZROOM_NAME,
                    BFormat = cc.ZFORMAT,
                    BuildCode = cc.ZBLDG,
                    Code = cc.ZROOM,
                    PjectMDCode = cc.ZPROJECT,
                    SourceSystem = cc.ZSYSTEM,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    UnitOfBuild = cc.ZRE_UNIT,
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取房号详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<RoomNumberDetailsDto>> GetRoomNumberDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<RoomNumberDetailsDto>();

            var result = await _dbContext.Queryable<RoomNumber>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new RoomNumberDetailsDto
                {
                    Name = cc.ZROOM_NAME,
                    BFormat = cc.ZFORMAT,
                    BuildCode = cc.ZBLDG,
                    Code = cc.ZROOM,
                    PjectMDCode = cc.ZPROJECT,
                    SourceSystem = cc.ZSYSTEM,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    UnitOfBuild = cc.ZRE_UNIT,
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 语言语种列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<LanguageDetailsDto>>> GetLanguageSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<LanguageDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<Language>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new LanguageDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZLANG_ZH,
                    DirCode = cc.ZLANG_BIB,
                    EnglishName = cc.ZLANG_EN,
                    TermCode = cc.ZLANG_TER,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE,
                    Version = cc.ZVERSION
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 语言语种明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<LanguageDetailsDto>> GetLanguageDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<LanguageDetailsDto>();

            var result = await _dbContext.Queryable<Language>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new LanguageDetailsDto
                {
                    Name = cc.ZLANG_ZH,
                    DirCode = cc.ZLANG_BIB,
                    EnglishName = cc.ZLANG_EN,
                    TermCode = cc.ZLANG_TER,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE,
                    Version = cc.ZVERSION
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取银行账号列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BankCardDetailsDto>>> GetBankCardSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<BankCardDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<BankCard>()
                .Select((cc) => new BankCardDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZKOINH,
                    AccountCurrency = cc.ZCURR,
                    AccountStatus = cc.ZBANKSTA,
                    BankAccount = cc.ZBANKN,
                    BankNoPK = cc.ZBANK,
                    City = cc.ZCITY2,
                    Country = cc.ZZCOUNTR2,
                    County = cc.ZCOUNTY2,
                    DealUnitCode = cc.ZBP,
                    FinancialOrgCode = cc.ZFINC,
                    FinancialOrgName = cc.ZFINAME,
                    Province = cc.ZPROVINC2
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取银行账号详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<BankCardDetailsDto>> GetBankCardDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<BankCardDetailsDto>();

            var result = await _dbContext.Queryable<BankCard>()
                .Where((cc) => cc.Id.ToString() == id)
                .Select((cc) => new BankCardDetailsDto
                {
                    Name = cc.ZKOINH,
                    AccountCurrency = cc.ZCURR,
                    AccountStatus = cc.ZBANKSTA,
                    BankAccount = cc.ZBANKN,
                    BankNoPK = cc.ZBANK,
                    City = cc.ZCITY2,
                    Country = cc.ZZCOUNTR2,
                    County = cc.ZCOUNTY2,
                    DealUnitCode = cc.ZBP,
                    FinancialOrgCode = cc.ZFINC,
                    FinancialOrgName = cc.ZFINAME,
                    Province = cc.ZPROVINC2
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取物资设备明细编码
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DeviceDetailCodeDetailsDto>>> GetDeviceDetailCodeSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DeviceDetailCodeDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<DeviceDetailCode>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new DeviceDetailCodeDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZMNAME,
                    Descption = cc.ZMNAMES,
                    MDCode = cc.ZMATERIAL,
                    ProductNameCode = cc.ZCLASS,
                    IsCode = cc.ZOFTENCODE == "1" ? "是" : "否",
                    Remark = cc.ZREMARK,
                    State = cc.ZSTATE == "1" ? "有效" : "无效"
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取物资设备明细编码 详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<DeviceDetailCodeDetailsDto>> GetDeviceDetailCodeDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<DeviceDetailCodeDetailsDto>();

            var result = await _dbContext.Queryable<DeviceDetailCode>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new DeviceDetailCodeDetailsDto
                {
                    Name = cc.ZMNAME,
                    Descption = cc.ZMNAMES,
                    MDCode = cc.ZMATERIAL,
                    ProductNameCode = cc.ZCLASS,
                    IsCode = cc.ZOFTENCODE == "1" ? "是" : "否",
                    Remark = cc.ZREMARK,
                    State = cc.ZSTATE == "1" ? "有效" : "无效"
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取核算部门列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<AccountingDepartmentDetailsDto>>> GetAccountingDepartmentSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<AccountingDepartmentDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<AccountingDepartment>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new AccountingDepartmentDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZDNAME_CHS,
                    AccDepCode = cc.ZDCODE,
                    AccDepELName = cc.ZDNAME_EN,
                    AccDepTCCName = cc.ZDNAME_CHT,
                    State = cc.ZDATSTATE == "1" ? "停用" : "未停用",
                    AccDepId = cc.ZDID,
                    AccOrgCode = cc.ZACORGNO,
                    AccOrgId = cc.ZACID,
                    DataIdentifier = cc.ZDELETE == "1" ? "删除" : "正常",
                    SupAccDepId = cc.ZDPARENTID
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取核算部门详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<AccountingDepartmentDetailsDto>> GetAccountingDepartmentDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<AccountingDepartmentDetailsDto>();

            var result = await _dbContext.Queryable<AccountingDepartment>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new AccountingDepartmentDetailsDto
                {
                    Name = cc.ZDNAME_CHS,
                    AccDepCode = cc.ZDCODE,
                    AccDepELName = cc.ZDNAME_EN,
                    AccDepTCCName = cc.ZDNAME_CHT,
                    State = cc.ZDATSTATE == "1" ? "停用" : "未停用",
                    AccDepId = cc.ZDID,
                    AccOrgCode = cc.ZACORGNO,
                    AccOrgId = cc.ZACID,
                    DataIdentifier = cc.ZDELETE == "1" ? "删除" : "正常",
                    SupAccDepId = cc.ZDPARENTID
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取委托关系列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<RelationalContractsDetailsDto>>> GetRelationalContractsSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<RelationalContractsDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<RelationalContracts>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new RelationalContractsDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZDELEGATE_ORG,
                    DetailedLine = cc.ZNUMC4,
                    MDCode = cc.MDM_CODE,
                    Status = cc.ZDELEGATE_STATE,
                    OrgCode = cc.OID,
                    AccOrgCode = cc.ZACO,
                    TreeCode = cc.ZTREEID,
                    Version = cc.ZTREEVER,
                    ViewIdentification = cc.ZMVIEW_FLAG
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取委托关系详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<RelationalContractsDetailsDto>> GetRelationalContractsDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<RelationalContractsDetailsDto>();

            var result = await _dbContext.Queryable<RelationalContracts>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new RelationalContractsDetailsDto
                {
                    Code = cc.ZDELEGATE_ORG,
                    DetailedLine = cc.ZNUMC4,
                    MDCode = cc.MDM_CODE,
                    Status = cc.ZDELEGATE_STATE,
                    OrgCode = cc.OID,
                    AccOrgCode = cc.ZACO,
                    TreeCode = cc.ZTREEID,
                    Version = cc.ZTREEVER,
                    ViewIdentification = cc.ZMVIEW_FLAG
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交区域总部列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<RegionalDetailsDto>>> GetRegionalSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<RegionalDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<Regional>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new RegionalDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZCRHCODE,
                    AreaRange = cc.ZCRHSCOPE,
                    Description = cc.ZCRHNAME,
                    Name = cc.ZCRHABBR,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    Version = cc.ZVERSION,
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除"
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交区域总部详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<RegionalDetailsDto>> GetRegionalDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<RegionalDetailsDto>();

            var result = await _dbContext.Queryable<Regional>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new RegionalDetailsDto
                {
                    Code = cc.ZCRHCODE,
                    AreaRange = cc.ZCRHSCOPE,
                    Description = cc.ZCRHNAME,
                    Name = cc.ZCRHABBR,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    Version = cc.ZVERSION,
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除"
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取计量单位列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<UnitMeasurementDetailsDto>>> GetUnitMeasurementSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<UnitMeasurementDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<UnitMeasurement>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new UnitMeasurementDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZUNITCODE,
                    Name = cc.ZUNITNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    Version = cc.ZVERSION,
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除"
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取计量单位详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<UnitMeasurementDetailsDto>> GetUnitMeasurementDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<UnitMeasurementDetailsDto>();

            var result = await _dbContext.Queryable<UnitMeasurement>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new UnitMeasurementDetailsDto
                {
                    Code = cc.ZUNITCODE,
                    Name = cc.ZUNITNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    Version = cc.ZVERSION,
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除"
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系 列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectClassificationDetailsDto>>> GetProjectClassificationSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectClassificationDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<ProjectClassification>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new ProjectClassificationDetailsDto
                {
                    Id = cc.Id.ToString(),
                    BSectorSecName = cc.ZBUSTD2NAME,
                    BSectorRemark = cc.ZBUSTDREMARKS,
                    BSectorOneName = cc.ZBUSTD1NAME,
                    BSectorThirdName = cc.ZBUSTD3NAME,
                    BusinessRemark = cc.ZZCPBREMARKS,
                    CCCCBTypeName = cc.Z12TOPBNAME,
                    CCCCBTypeSecName = cc.ZCPBC2NAME,
                    CCCCBTypeThirdName = cc.ZCPBC3NAME,
                    CCCCRiverLakeAndSeaName = cc.ZRRLSNAME,
                    ChanYeOneName = cc.ZICSTD1NAME,
                    ChanYeRemark = cc.ZICSTDREMARKS,
                    ChanYeSecName = cc.ZICSTD2NAME,
                    ChanYeThirdName = cc.ZICSTD3NAME,
                    BSectorOneCode = cc.ZBUSTD1ID,
                    BSectorSecCode = cc.ZBUSTD2ID,
                    BSectorThirdCode = cc.ZBUSTD3ID,
                    CCCCBTypeCode = cc.Z12TOPBID,
                    CCCCBTypeOneCode = cc.ZCPBC1ID,
                    CCCCBTypeSecCode = cc.ZCPBC2ID,
                    CCCCBTypeThirdCode = cc.ZCPBC3ID,
                    CCCCRiverLakeAndSeaCode = cc.ZRRLSID,
                    ChanYeOneCode = cc.ZICSTD1ID,
                    ChanYeSecCode = cc.ZICSTD2ID,
                    Name = cc.ZCPBC1NAME,
                    ThirdNewBType = cc.ZNEW3TOB == "1" ? "是" : "否"
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<ProjectClassificationDetailsDto>> GetProjectClassificationDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<ProjectClassificationDetailsDto>();

            var result = await _dbContext.Queryable<ProjectClassification>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new ProjectClassificationDetailsDto
                {
                    BSectorSecName = cc.ZBUSTD2NAME,
                    BSectorRemark = cc.ZBUSTDREMARKS,
                    BSectorOneName = cc.ZBUSTD1NAME,
                    BSectorThirdName = cc.ZBUSTD3NAME,
                    BusinessRemark = cc.ZZCPBREMARKS,
                    CCCCBTypeName = cc.Z12TOPBNAME,
                    CCCCBTypeSecName = cc.ZCPBC2NAME,
                    CCCCBTypeThirdName = cc.ZCPBC3NAME,
                    CCCCRiverLakeAndSeaName = cc.ZRRLSNAME,
                    ChanYeOneName = cc.ZICSTD1NAME,
                    ChanYeRemark = cc.ZICSTDREMARKS,
                    ChanYeSecName = cc.ZICSTD2NAME,
                    ChanYeThirdName = cc.ZICSTD3NAME,
                    BSectorOneCode = cc.ZBUSTD1ID,
                    BSectorSecCode = cc.ZBUSTD2ID,
                    BSectorThirdCode = cc.ZBUSTD3ID,
                    CCCCBTypeCode = cc.Z12TOPBID,
                    CCCCBTypeOneCode = cc.ZCPBC1ID,
                    CCCCBTypeSecCode = cc.ZCPBC2ID,
                    CCCCBTypeThirdCode = cc.ZCPBC3ID,
                    CCCCRiverLakeAndSeaCode = cc.ZRRLSID,
                    ChanYeOneCode = cc.ZICSTD1ID,
                    ChanYeSecCode = cc.ZICSTD2ID,
                    Name = cc.ZCPBC1NAME,
                    ThirdNewBType = cc.ZNEW3TOB == "1" ? "是" : "否"
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取中交区域中心列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<RegionalCenterDetailsDto>>> GetRegionalCenterSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<RegionalCenterDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<RegionalCenter>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new RegionalCenterDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZCRCCODE,
                    Description = cc.ZCRCNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    Version = cc.ZVERSION
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取中交区域中心详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<RegionalCenterDetailsDto>> GetRegionalCenterDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<RegionalCenterDetailsDto>();

            var result = await _dbContext.Queryable<RegionalCenter>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new RegionalCenterDetailsDto
                {
                    Code = cc.ZCRCCODE,
                    Description = cc.ZCRCNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    Version = cc.ZVERSION
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取国民经济行业分类列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<NationalEconomyDetailsDto>>> GetNationalEconomySearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<NationalEconomyDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<NationalEconomy>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new NationalEconomyDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZNEQCODE,
                    Descption = cc.ZNEQDESC,
                    Name = cc.ZNEQNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    SupCode = cc.ZNEQCODEUP,
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    Version = cc.ZVERSION
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取国民经济行业分类详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<NationalEconomyDetailsDto>> GetNationalEconomyDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<NationalEconomyDetailsDto>();

            var result = await _dbContext.Queryable<NationalEconomy>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new NationalEconomyDetailsDto
                {
                    Code = cc.ZNEQCODE,
                    Descption = cc.ZNEQDESC,
                    Name = cc.ZNEQNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    SupCode = cc.ZNEQCODEUP,
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    Version = cc.ZVERSION
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取行政机构和核算机构映射关系 列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<AdministrativeAccountingMapperDetailsDto>>> GetAdministrativeAccountingMapperSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<AdministrativeAccountingMapperDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<AdministrativeAccountingMapper>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new AdministrativeAccountingMapperDetailsDto
                {
                    Id = cc.Id.ToString(),
                    AccOrgCode = cc.ZAORGNO,
                    AccOrgId = cc.ZAID,
                    AdministrativeOrgCode = cc.ZORGCODE,
                    AdministrativeOrgId = cc.ZORGID,
                    DataIdentifier = cc.ZDELETE,
                    KeyId = cc.ZID
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取行政机构和核算机构映射关系 明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<AdministrativeAccountingMapperDetailsDto>> GetAdministrativeAccountingMapperDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<AdministrativeAccountingMapperDetailsDto>();

            var result = await _dbContext.Queryable<AdministrativeAccountingMapper>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new AdministrativeAccountingMapperDetailsDto
                {
                    AccOrgCode = cc.ZAORGNO,
                    AccOrgId = cc.ZAID,
                    AdministrativeOrgCode = cc.ZORGCODE,
                    AdministrativeOrgId = cc.ZORGID,
                    DataIdentifier = cc.ZDELETE,
                    KeyId = cc.ZID
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;

        }
        /// <summary>
        /// 多组织-税务代管组织(行政)列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<EscrowOrganizationDetailsDto>>> GetEscrowOrganizationSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<EscrowOrganizationDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<EscrowOrganization>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new EscrowOrganizationDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Country = cc.CAREA,
                    IsIndependenceAcc = cc.IS_INDEPENDENT,
                    LocationOfOrg = cc.ORGPROVINCE,
                    Name = cc.NAME,
                    NameEnglish = cc.ENGLISHNAME,
                    NameLLanguage = cc.ZZTNAME_LOC,
                    NodeSequence = cc.SNO,
                    OrgStatus = cc.STATUS,
                    RegionalAttr = cc.TERRITORYPRO,
                    Remark = cc.NOTE,
                    Shareholding = cc.SHAREHOLDINGS,
                    ShortNameChinese = cc.SHORTNAME,
                    TelAddress = cc.ZADDRESS,
                    HROrgMDCode = cc.OID,
                    OrgAttr = cc.TYPE,
                    OrgChildAttr = cc.TYPEEXT,
                    OrgCode = cc.OCODE,
                    OrgGruleCode = cc.ORULE,
                    OrgMDCode = cc.MDM_CODE,
                    RegistrationNo = cc.REGISTERCODE,
                    ShortNameEnglish = cc.ENGLISHSHORTNAME,
                    ShortNameLLanguage = cc.ZZTSHNAME_LOC,
                    SupHROrgMDCode = cc.POID,
                    SupOrgMDCode = cc.ZORGUP,
                    TreeLevel = cc.GRADE,
                    UnitSec = cc.GPOID,
                    ViewIdentification = cc.VIEW_FLAG,
                    ZINSTID = cc.ZINSTID
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-税务代管组织(行政) 详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<EscrowOrganizationDetailsDto>> GetEscrowOrganizationDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<EscrowOrganizationDetailsDto>();

            var result = await _dbContext.Queryable<EscrowOrganization>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new EscrowOrganizationDetailsDto
                {
                    Country = cc.CAREA,
                    IsIndependenceAcc = cc.IS_INDEPENDENT,
                    LocationOfOrg = cc.ORGPROVINCE,
                    Name = cc.NAME,
                    NameEnglish = cc.ENGLISHNAME,
                    NameLLanguage = cc.ZZTNAME_LOC,
                    NodeSequence = cc.SNO,
                    OrgStatus = cc.STATUS,
                    RegionalAttr = cc.TERRITORYPRO,
                    Remark = cc.NOTE,
                    Shareholding = cc.SHAREHOLDINGS,
                    ShortNameChinese = cc.SHORTNAME,
                    TelAddress = cc.ZADDRESS,
                    HROrgMDCode = cc.OID,
                    OrgAttr = cc.TYPE,
                    OrgChildAttr = cc.TYPEEXT,
                    OrgCode = cc.OCODE,
                    OrgGruleCode = cc.ORULE,
                    OrgMDCode = cc.MDM_CODE,
                    RegistrationNo = cc.REGISTERCODE,
                    ShortNameEnglish = cc.ENGLISHSHORTNAME,
                    ShortNameLLanguage = cc.ZZTSHNAME_LOC,
                    SupHROrgMDCode = cc.POID,
                    SupOrgMDCode = cc.ZORGUP,
                    TreeLevel = cc.GRADE,
                    UnitSec = cc.GPOID,
                    ViewIdentification = cc.VIEW_FLAG,
                    ZINSTID = cc.ZINSTID
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 商机项目(不含境外商机项目) 列表  国家地区区分  142境内，142以为境外
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="isJingWai"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BusinessNoCpportunityDetailsDto>>> GetBusinessNoCpportunitySearchAsync(FilterCondition requestDto, bool isJingWai)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<BusinessNoCpportunityDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<BusinessCpportunity>()
                .WhereIF((isJingWai), (cc) => cc.ZZCOUNTRY == "142")
                .WhereIF((!isJingWai), (cc) => cc.ZZCOUNTRY != "142")
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new BusinessNoCpportunityDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Country = cc.ZZCOUNTRY,
                    BPjectForeignName = cc.ZBOPN_EN,
                    BPjectMDCode = cc.ZBOP,
                    Name = cc.ZBOPN,
                    PjectType = cc.ZPROJTYPE,
                    QualificationUnit = cc.ZORG_QUAL,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    TaxationMethod = cc.ZTAXMETHOD,
                    BTypeOfCCCCProjects = cc.ZCPBC,
                    ParticipatingUnits = cc.ZCY2NDORG,
                    PjectLocation = cc.ZPROJLOC,
                    StartTrackingDate = cc.ZSFOLDATE,
                    TrackingUnit = cc.ZORG,
                    UnitSec = cc.Z2NDORG
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 商机项目(含/不含 境外商机项目) 详情  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<BusinessNoCpportunityDetailsDto>> GetBusinessNoCpportunityDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<BusinessNoCpportunityDetailsDto>();

            var result = await _dbContext.Queryable<BusinessCpportunity>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new BusinessNoCpportunityDetailsDto
                {
                    Country = cc.ZZCOUNTRY,
                    BPjectForeignName = cc.ZBOPN_EN,
                    BPjectMDCode = cc.ZBOP,
                    Name = cc.ZBOPN,
                    PjectType = cc.ZPROJTYPE,
                    QualificationUnit = cc.ZORG_QUAL,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    TaxationMethod = cc.ZTAXMETHOD,
                    BTypeOfCCCCProjects = cc.ZCPBC,
                    ParticipatingUnits = cc.ZCY2NDORG,
                    PjectLocation = cc.ZPROJLOC,
                    StartTrackingDate = cc.ZSFOLDATE,
                    TrackingUnit = cc.ZORG,
                    UnitSec = cc.Z2NDORG
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取境内行政区划 列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<AdministrativeDivisionDetailsDto>>> GetAdministrativeDivisionSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<AdministrativeDivisionDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<AdministrativeDivision>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new AdministrativeDivisionDetailsDto
                {
                    Id = cc.Id.ToString(),
                    CodeOfCCCCRegional = cc.ZCRHCODE,
                    RegionalismCode = cc.ZADDVSCODE,
                    RegionalismLevel = cc.ZADDVSLEVEL,
                    Name = cc.ZADDVSNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    SupRegionalismCode = cc.ZADDVSUP,
                    Version = cc.ZVERSION
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取境内行政区划 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<AdministrativeDivisionDetailsDto>> GetAdministrativeDivisionDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<AdministrativeDivisionDetailsDto>();

            var result = await _dbContext.Queryable<AdministrativeDivision>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new AdministrativeDivisionDetailsDto
                {
                    CodeOfCCCCRegional = cc.ZCRHCODE,
                    RegionalismCode = cc.ZADDVSCODE,
                    RegionalismLevel = cc.ZADDVSLEVEL,
                    Name = cc.ZADDVSNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    SupRegionalismCode = cc.ZADDVSUP,
                    Version = cc.ZVERSION
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-核算机构 列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<AccountingOrganizationDetailsDto>>> GetAccountingOrganizationSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<AccountingOrganizationDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<AccountingOrganization>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new AccountingOrganizationDetailsDto
                {
                    Id = cc.Id.ToString(),
                    MDM_CODE = cc.MDM_CODE,
                    ZACNAME_CHS = cc.ZACNAME_CHS,
                    ZACNAME_EN = cc.ZACNAME_EN,
                    ZACNAME_LOC = cc.ZACNAME_LOC,
                    ZACO = cc.ZACO,
                    ZACSHORTNAME_CHS = cc.ZACSHORTNAME_CHS,
                    ZACSHORTNAME_EN = cc.ZACSHORTNAME_EN,
                    ZACSHORTNAME_LOC = cc.ZACSHORTNAME_LOC,
                    ZBRGZW = cc.ZBRGZW,
                    ZBRHW = cc.ZBRHW,
                    ZCONTINENTCODE = cc.ZCONTINENTCODE,
                    ZCYNAME = cc.ZCYNAME,
                    ZHTE = cc.ZHTE,
                    ZH_IN_OUT = cc.ZH_IN_OUT,
                    ZOSTATE = cc.ZOSTATE,
                    ZREMARK = cc.ZREMARK,
                    ZRPNATURE = cc.ZRPNATURE,
                    ZSYSTEM = cc.ZSYSTEM,
                    ZYORGSTATE = cc.ZYORGSTATE,
                    ZZTNAME_EN = cc.ZZTNAME_EN,
                    ZZTNAME_LOC = cc.ZZTNAME_LOC,
                    ZZTNAME_ZH = cc.ZZTNAME_ZH,
                    ZZTSHNAME_CHS = cc.ZZTSHNAME_CHS,
                    ZZTSHNAME_EN = cc.ZZTSHNAME_EN,
                    ZZTSHNAME_LOC = cc.ZZTSHNAME_LOC,
                    VIEW_FLAG = cc.VIEW_FLAG,
                    ZACCOUNT_DATE = cc.ZACCOUNT_DATE,
                    ZACDISABLEYEAR = cc.ZACDISABLEYEAR,
                    ZACID = cc.ZACID,
                    ZACISDETAIL = cc.ZACISDETAIL,
                    ZACJTHBFWN = cc.ZACJTHBFWN,
                    ZACLAYER = cc.ZACLAYER,
                    ZACORGNO = cc.ZACORGNO,
                    ZACPARENTCODE = cc.ZACPARENTCODE,
                    ZACPARENTID = cc.ZACPARENTID,
                    ZACPATH = cc.ZACPATH,
                    ZACSORTORDER = cc.ZACSORTORDER,
                    ZAORGSTATE = cc.ZAORGSTATE,
                    ZAPPROVAL_ORG = cc.ZAPPROVAL_ORG,
                    ZBBID = cc.ZBBID,
                    ZBTID = cc.ZBTID,
                    ZBUSINESS_RECOCATION = cc.ZBUSINESS_RECOCATION,
                    ZTREEID = cc.ZTREEID,
                    ZBUSINESS_UNIT = cc.ZBUSINESS_UNIT,
                    ZCWYGL = cc.ZCWYGL,
                    ZCWYGL_REA = cc.ZCWYGL_REA,
                    ZDCID = cc.ZDCID,
                    ZDEL_MAP = cc.ZDEL_MAP,
                    ZDEL_REA = cc.ZDEL_REA,
                    ZIVFLGID = cc.ZIVFLGID,
                    ZNBFYL = cc.ZNBFYL,
                    ZORGATTR = cc.ZORGATTR,
                    ZORGCHILDATTR = cc.ZORGCHILDATTR,
                    ZORGLOC = cc.ZORGLOC,
                    ZQYBBDAT = cc.ZQYBBDAT,
                    ZREGIONAL = cc.ZREGIONAL,
                    ZREPORT_FLAG = cc.ZREPORT_FLAG,
                    ZREPORT_NODE = cc.ZREPORT_NODE,
                    ZREPORT_TIME = cc.ZREPORT_TIME,
                    ZRULE = cc.ZRULE,
                    ZSCENTER = cc.ZSCENTER,
                    ZSNO = cc.ZSNO,
                    ZTAXMETHOD = cc.ZTAXMETHOD,
                    ZTAXPAYER_CATEGORY = cc.ZTAXPAYER_CATEGORY,
                    ZTAX_ORGANIZATION = cc.ZTAX_ORGANIZATION,
                    ZTORG = cc.ZTORG,
                    ZTREEVER = cc.ZTREEVER,
                    ZTRNO = cc.ZTRNO,
                    ZUNAME = cc.ZUNAME,
                    ZZCURRENCY = cc.ZZCURRENCY
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-核算机构 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<AccountingOrganizationDetailsDto>> GetAccountingOrganizationDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<AccountingOrganizationDetailsDto>();

            var result = await _dbContext.Queryable<AccountingOrganization>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new AccountingOrganizationDetailsDto
                {
                    MDM_CODE = cc.MDM_CODE,
                    ZACNAME_CHS = cc.ZACNAME_CHS,
                    ZACNAME_EN = cc.ZACNAME_EN,
                    ZACNAME_LOC = cc.ZACNAME_LOC,
                    ZACO = cc.ZACO,
                    ZACSHORTNAME_CHS = cc.ZACSHORTNAME_CHS,
                    ZACSHORTNAME_EN = cc.ZACSHORTNAME_EN,
                    ZACSHORTNAME_LOC = cc.ZACSHORTNAME_LOC,
                    ZBRGZW = cc.ZBRGZW,
                    ZBRHW = cc.ZBRHW,
                    ZCONTINENTCODE = cc.ZCONTINENTCODE,
                    ZCYNAME = cc.ZCYNAME,
                    ZHTE = cc.ZHTE,
                    ZH_IN_OUT = cc.ZH_IN_OUT,
                    ZOSTATE = cc.ZOSTATE,
                    ZREMARK = cc.ZREMARK,
                    ZRPNATURE = cc.ZRPNATURE,
                    ZSYSTEM = cc.ZSYSTEM,
                    ZYORGSTATE = cc.ZYORGSTATE,
                    ZZTNAME_EN = cc.ZZTNAME_EN,
                    ZZTNAME_LOC = cc.ZZTNAME_LOC,
                    ZZTNAME_ZH = cc.ZZTNAME_ZH,
                    ZZTSHNAME_CHS = cc.ZZTSHNAME_CHS,
                    ZZTSHNAME_EN = cc.ZZTSHNAME_EN,
                    ZZTSHNAME_LOC = cc.ZZTSHNAME_LOC,
                    VIEW_FLAG = cc.VIEW_FLAG,
                    ZACCOUNT_DATE = cc.ZACCOUNT_DATE,
                    ZACDISABLEYEAR = cc.ZACDISABLEYEAR,
                    ZACID = cc.ZACID,
                    ZACISDETAIL = cc.ZACISDETAIL,
                    ZACJTHBFWN = cc.ZACJTHBFWN,
                    ZACLAYER = cc.ZACLAYER,
                    ZACORGNO = cc.ZACORGNO,
                    ZACPARENTCODE = cc.ZACPARENTCODE,
                    ZACPARENTID = cc.ZACPARENTID,
                    ZACPATH = cc.ZACPATH,
                    ZACSORTORDER = cc.ZACSORTORDER,
                    ZAORGSTATE = cc.ZAORGSTATE,
                    ZAPPROVAL_ORG = cc.ZAPPROVAL_ORG,
                    ZBBID = cc.ZBBID,
                    ZBTID = cc.ZBTID,
                    ZBUSINESS_RECOCATION = cc.ZBUSINESS_RECOCATION,
                    ZTREEID = cc.ZTREEID,
                    ZBUSINESS_UNIT = cc.ZBUSINESS_UNIT,
                    ZCWYGL = cc.ZCWYGL,
                    ZCWYGL_REA = cc.ZCWYGL_REA,
                    ZDCID = cc.ZDCID,
                    ZDEL_MAP = cc.ZDEL_MAP,
                    ZDEL_REA = cc.ZDEL_REA,
                    ZIVFLGID = cc.ZIVFLGID,
                    ZNBFYL = cc.ZNBFYL,
                    ZORGATTR = cc.ZORGATTR,
                    ZORGCHILDATTR = cc.ZORGCHILDATTR,
                    ZORGLOC = cc.ZORGLOC,
                    ZQYBBDAT = cc.ZQYBBDAT,
                    ZREGIONAL = cc.ZREGIONAL,
                    ZREPORT_FLAG = cc.ZREPORT_FLAG,
                    ZREPORT_NODE = cc.ZREPORT_NODE,
                    ZREPORT_TIME = cc.ZREPORT_TIME,
                    ZRULE = cc.ZRULE,
                    ZSCENTER = cc.ZSCENTER,
                    ZSNO = cc.ZSNO,
                    ZTAXMETHOD = cc.ZTAXMETHOD,
                    ZTAXPAYER_CATEGORY = cc.ZTAXPAYER_CATEGORY,
                    ZTAX_ORGANIZATION = cc.ZTAX_ORGANIZATION,
                    ZTORG = cc.ZTORG,
                    ZTREEVER = cc.ZTREEVER,
                    ZTRNO = cc.ZTRNO,
                    ZUNAME = cc.ZUNAME,
                    ZZCURRENCY = cc.ZZCURRENCY
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取币种列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<CurrencyDetailsDto>>> GetCurrencySearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<CurrencyDetailsDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<Currency>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new CurrencyDetailsDto
                {
                    Id = cc.Id.ToString(),
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    Name = cc.ZCURRENCYNAME,
                    Code = cc.ZCURRENCYCODE,
                    LetterCode = cc.ZCURRENCYALPHABET,
                    StandardName = cc.STANDARDNAMEE,
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    Remark = cc.ZREMARKS,
                    Version = cc.ZVERSION
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取币种详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<CurrencyDetailsDto>> GetCurrencyDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<CurrencyDetailsDto>();

            var result = await _dbContext.Queryable<Currency>()
                .Where((cc) => cc.IsDelete == 1 && cc.Id.ToString() == id)
                .Select((cc) => new CurrencyDetailsDto
                {
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    Name = cc.ZCURRENCYNAME,
                    Code = cc.ZCURRENCYCODE,
                    LetterCode = cc.ZCURRENCYALPHABET,
                    StandardName = cc.STANDARDNAMEE,
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    Remark = cc.ZREMARKS,
                    Version = cc.ZVERSION
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取值域列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ValueDomainReceiveResponseDto>>> GetValueDomainReceiveAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ValueDomainReceiveResponseDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<ValueDomain>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new ValueDomainReceiveResponseDto
                {
                    Id = cc.Id.ToString(),
                    ZDOM_CODE = cc.ZDOM_CODE,
                    ZDOM_DESC = cc.ZDOM_DESC,
                    ZDOM_NAME = cc.ZDOM_NAME,
                    ZDOM_SUP = cc.ZDOM_SUP,
                    ZDOM_VALUE = cc.ZDOM_VALUE,
                    ZREMARKS = cc.ZREMARKS,
                    ZVERSION = cc.ZVERSION
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
    }
}
