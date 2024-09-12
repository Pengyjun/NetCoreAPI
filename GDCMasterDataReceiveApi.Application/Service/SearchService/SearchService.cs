using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts;
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
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
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
        public async Task<ResponseAjaxResult<List<UserSearchResponseDto>>> GetUserSearchAsync(UserSearchRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<UserSearchResponseDto>>();
            RefAsync<int> total = 0;

            //获取人员信息
            var userInfos = await _dbContext.Queryable<User>()
                .LeftJoin<UserStatus>((u, us) => u.EMP_STATUS == us.OneCode)
                .LeftJoin<Institution>((u, us, ins) => u.OFFICE_DEPID == ins.OID)
                .Where((u, us, ins) => !string.IsNullOrWhiteSpace(u.EMP_STATUS) && u.IsDelete == 1)
                .Select((u, us, ins) => new UserSearchResponseDto
                {
                    Id = u.Id.ToString(),
                    Name = u.NAME,
                    CertNo = u.CERT_NO,
                    OfficeDepId = u.OFFICE_DEPID,
                    Email = u.EMAIL,
                    EmpCode = u.EMP_CODE,
                    Enable = u.Enable == 1 ? "启用" : "禁用",
                    UserInfoStatus = us.OneName,
                    Phone = u.PHONE,
                    OfficeDepIdName = ins.NAME
                }).ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            //获取机构信息
            var institutions = await _dbContext.Queryable<Institution>()
                .Where(t => t.IsDelete == 1)
                .Select(t => new UInstutionDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
                .ToListAsync();

            foreach (var institution in userInfos)
            {
                institution.CompanyName = GetUserCompany(institution.OfficeDepId, institutions);
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

            //获取机构信息
            var institutions = await _dbContext.Queryable<Institution>()
                .Where(t => t.IsDelete == 1)
                .Select(t => new UInstutionDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
                .ToListAsync();
            uDetails.CompanyName = GetUserCompany(uDetails.OfficeDepId, institutions);

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
        public async Task<ResponseAjaxResult<List<InstitutionDto>>> GetInstitutionAsync(InstitutionRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<InstitutionDto>>();
            var result = new List<InstitutionDto>();

            //机构树初始化
            var institutions = await _dbContext.Queryable<Institution>()
                .Where(t => t.IsDelete == 1)
                .ToListAsync();

            //其他数据
            var otherNodes = institutions.Where(x => x.OID != "101162350")
                .Select(x => new InstitutionDto
                {
                    Code = x.OCODE,
                    EnglishName = x.ENGLISHNAME,
                    EnglishShortName = x.ENGLISHSHORTNAME,
                    EntClass = x.ENTCLASS,
                    Id = x.Id.ToString(),
                    Name = x.NAME,
                    Oid = x.OID,
                    Orule = x.ORULE,
                    POid = x.POID,
                    ShortName = x.SHORTNAME,
                    Status = x.STATUS
                })
                .ToList();

            //根节点
            var rootNode = institutions.Where(x => x.OID == "101162350")
                .Select(x => new InstitutionDto
                {
                    Code = x.OCODE,
                    EnglishName = x.ENGLISHNAME,
                    EnglishShortName = x.ENGLISHSHORTNAME,
                    EntClass = x.ENTCLASS,
                    Id = x.Id.ToString(),
                    Name = x.NAME,
                    Oid = x.OID,
                    Orule = x.ORULE,
                    POid = x.POID,
                    ShortName = x.SHORTNAME,
                    Status = x.STATUS,
                    Children = GetChildren(x.OID, otherNodes)
                }).FirstOrDefault();
            result.Add(rootNode);

            responseAjaxResult.SuccessResult(result);
            responseAjaxResult.Count = result.Count;
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="pOid"></param>
        /// <param name="children"></param>
        /// <returns></returns>
        public List<InstitutionDto> GetChildren(string pOid, List<InstitutionDto> children)
        {
            var childs = children.Where(x => x.POid == pOid).ToList();
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

            responseAjaxResult.SuccessResult(insDetails);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<ProjectSearchDto>>> GetProjectSearchAsync(ProjectRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectSearchDto>>();
            RefAsync<int> total = 0;

            var proList = await _dbContext.Queryable<Project>()
                .LeftJoin<Institution>((pro, ins) => pro.ZPRO_ORG == ins.OID)
                .Where((pro, ins) => pro.IsDelete == 1)
                .Select((pro, ins) => new ProjectSearchDto
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
                    Year = pro.ZPROJYEAR
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
        public async Task<ResponseAjaxResult<List<CorresUnitSearchDto>>> GetCorresUnitSearchAsync(CorresUnitRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<CorresUnitSearchDto>>();
            RefAsync<int> total = 0;

            var corresUnitList = await _dbContext.Queryable<CorresUnit>()
                .Where((cu) => cu.IsDelete == 1)
                .Select((cu) => new CorresUnitSearchDto
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
                    StatusOfUnit = cu.ZBPSTATE == "01" ? "启用" : "停用"
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

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
        public async Task<ResponseAjaxResult<List<CountryRegionSearchDto>>> GetCountryRegionSearchAsync(CountryRegionRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<CountryRegionSearchDto>>();
            RefAsync<int> total = 0;

            var corresUnitList = await _dbContext.Queryable<CountryRegion>()
                .Where((cr) => cr.IsDelete == 1)
                .Select((cr) => new CountryRegionSearchDto
                {
                    Id = cr.Id.ToString(),
                    Country = cr.ZCOUNTRYCODE,
                    DigitCode = cr.ZGBNUM,
                    Name = cr.ZCOUNTRYNAME,
                    NameEnglish = cr.ZCOUNTRYENAME,
                    NationalCode = cr.ZGBCHAR,
                    State = cr.ZSTATE
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
        public async Task<ResponseAjaxResult<List<CountryContinentSearchDto>>> GetCountryContinentSearchAsync(CountryContinentRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<CountryContinentSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<CountryContinent>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new CountryContinentSearchDto
                {
                    Id = cc.Id.ToString(),
                    ContinentCode = cc.ZCONTINENTCODE,
                    Name = cc.ZCONTINENTNAME,
                    RegionalDescr = cc.ZAREANAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效"
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
        public async Task<ResponseAjaxResult<List<FinancialInstitutionSearchDto>>> GetFinancialInstitutionSearchAsync(FinancialInstitutionRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<FinancialInstitutionSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<FinancialInstitution>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new FinancialInstitutionSearchDto
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
                    State = cc.ZDATSTATE == "1" ? "有效" : "无效"
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
        public async Task<ResponseAjaxResult<List<DeviceClassCodeSearchDto>>> GetDeviceClassCodeSearchAsync(DeviceClassCodeRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DeviceClassCodeSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<DeviceClassCode>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new DeviceClassCodeSearchDto
                {
                    Id = cc.Id.ToString(),
                    AliasName = cc.ZCALIAS,
                    Code = cc.ZCLASS,
                    Description = cc.ZCDESC,
                    Name = cc.ZCNAME,
                    UnitOfMeasurement = cc.ZMSEHI,
                    State = cc.ZUSSTATE == "1" ? "有效" : "无效"
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
        public async Task<ResponseAjaxResult<List<InvoiceTypeSearchDto>>> GetInvoiceTypeSearchAsync(InvoiceTypeRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<InvoiceTypeSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<InvoiceType>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new InvoiceTypeSearchDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZINVTCODE,
                    Name = cc.ZINVTNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效"
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
        public async Task<ResponseAjaxResult<List<ScientifiCNoProjectSearchDto>>> GetScientifiCNoProjectSearchAsync(ScientifiCNoProjectRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ScientifiCNoProjectSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<ScientifiCNoProject>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new ScientifiCNoProjectSearchDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZSRPN,
                    CurrencyOfCost = cc.ZPROJCOSTCUR,
                    ForeignName = cc.ZSRPN_FN,
                    IsHighTech = cc.ZHITECH == "1" ? "是" : "否",
                    MDCode = cc.ZSRP,
                    PjectState = cc.ZKPSTATE,
                    TotalCost = cc.ZPROJCOST,
                    Year = cc.ZPROJYEA
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
        public async Task<ResponseAjaxResult<List<RoomNumberSearchDto>>> GetRoomNumberSearchAsync(RoomNumberRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<RoomNumberSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<RoomNumber>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new RoomNumberSearchDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZROOM_NAME,
                    BFormat = cc.ZFORMAT,
                    BuildCode = cc.ZBLDG,
                    Code = cc.ZROOM,
                    State = cc.ZSTATE == "1" ? "有效" : "无效"
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
        public async Task<ResponseAjaxResult<List<LanguageSearchDto>>> GetLanguageSearchAsync(LanguageRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<LanguageSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<Language>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new LanguageSearchDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZLANG_ZH,
                    DirCode = cc.ZLANG_BIB,
                    EnglishName = cc.ZLANG_EN,
                    TermCode = cc.ZLANG_TER,
                    State = cc.ZSTATE == "1" ? "有效" : "无效"
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
        public async Task<ResponseAjaxResult<List<BankCardSearchDto>>> GetBankCardSearchAsync(BankCardRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<BankCardSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<BankCard>()
                .Select((cc) => new BankCardSearchDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZKOINH,
                    AccountCurrency = cc.ZCURR,
                    AccountStatus = cc.ZBANKSTA,
                    BankAccount = cc.ZBANKN
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
                .Where((cc) =>  cc.Id.ToString() == id)
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
        public async Task<ResponseAjaxResult<List<DeviceDetailCodeSearchDto>>> GetDeviceDetailCodeSearchAsync(DeviceDetailCodeRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DeviceDetailCodeSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<DeviceDetailCode>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new DeviceDetailCodeSearchDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZMNAME,
                    Descption = cc.ZMNAMES,
                    MDCode = cc.ZMATERIAL,
                    ProductNameCode = cc.ZCLASS
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
        public async Task<ResponseAjaxResult<List<AccountingDepartmentSearchDto>>> GetAccountingDepartmentSearchAsync(AccountingDepartmentRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<AccountingDepartmentSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<AccountingDepartment>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new AccountingDepartmentSearchDto
                {
                    Id = cc.Id.ToString(),
                    Name = cc.ZDNAME_CHS,
                    AccDepCode = cc.ZDCODE,
                    AccDepELName = cc.ZDNAME_EN,
                    AccDepTCCName = cc.ZDNAME_CHT,
                    State = cc.ZDATSTATE == "1" ? "停用" : "未停用"
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
        public async Task<ResponseAjaxResult<List<RelationalContractsSearchDto>>> GetRelationalContractsSearchAsync(RelationalContractsRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<RelationalContractsSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<RelationalContracts>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new RelationalContractsSearchDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZDELEGATE_ORG,
                    DetailedLine = cc.ZNUMC4,
                    MDCode = cc.MDM_CODE,
                    Status = cc.ZDELEGATE_STATE
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
        public async Task<ResponseAjaxResult<List<RegionalSearchDto>>> GetRegionalSearchAsync(RegionalRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<RegionalSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<Regional>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new RegionalSearchDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZCRHCODE,
                    AreaRange = cc.ZCRHSCOPE,
                    Description = cc.ZCRHNAME,
                    Name = cc.ZCRHABBR,
                    State = cc.ZSTATE == "1" ? "有效" : "无效"
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
        public async Task<ResponseAjaxResult<List<UnitMeasurementSearchDto>>> GetUnitMeasurementSearchAsync(UnitMeasurementRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<UnitMeasurementSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<UnitMeasurement>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new UnitMeasurementSearchDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZUNITCODE,
                    DataIdentifier = cc.ZDELETE,
                    Name = cc.ZUNITNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效"
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
        public async Task<ResponseAjaxResult<List<ProjectClassificationSearchDto>>> GetProjectClassificationSearchAsync(ProjectClassificationRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<ProjectClassificationSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<ProjectClassification>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new ProjectClassificationSearchDto
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
                    ChanYeThirdName = cc.ZICSTD3NAME
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
        public async Task<ResponseAjaxResult<List<RegionalCenterSearchDto>>> GetRegionalCenterSearchAsync(RegionalCenterRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<RegionalCenterSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<RegionalCenter>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new RegionalCenterSearchDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZCRCCODE,
                    Description = cc.ZCRCNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效"
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
        public async Task<ResponseAjaxResult<List<NationalEconomySearchDto>>> GetNationalEconomySearchAsync(NationalEconomyRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<NationalEconomySearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<NationalEconomy>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new NationalEconomySearchDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZNEQCODE,
                    Descption = cc.ZNEQDESC,
                    Name = cc.ZNEQNAME
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
        public async Task<ResponseAjaxResult<List<AdministrativeAccountingMapperSearchDto>>> GetAdministrativeAccountingMapperSearchAsync(AdministrativeAccountingMapperRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<AdministrativeAccountingMapperSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<AdministrativeAccountingMapper>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new AdministrativeAccountingMapperSearchDto
                {
                    Id = cc.Id.ToString(),
                    AccOrgCode = cc.ZAORGNO,
                    AccOrgId = cc.ZAID,
                    AdministrativeOrgCode = cc.ZORGCODE
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
        public async Task<ResponseAjaxResult<List<EscrowOrganizationSearchDto>>> GetEscrowOrganizationSearchAsync(EscrowOrganizationRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<EscrowOrganizationSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<EscrowOrganization>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new EscrowOrganizationSearchDto
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
                    TelAddress = cc.ZADDRESS
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
        /// 商机项目(不含境外商机项目) 列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<BusinessNoCpportunitySearchDto>>> GetBusinessNoCpportunitySearchAsync(BusinessNoCpportunityRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<BusinessNoCpportunitySearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<BusinessNoCpportunity>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new BusinessNoCpportunitySearchDto
                {
                    Id = cc.Id.ToString(),
                    Country = cc.ZZCOUNTRY,
                    BPjectForeignName = cc.ZBOPN_EN,
                    BPjectMDCode = cc.ZBOP,
                    Name = cc.ZBOPN,
                    PjectType = cc.ZPROJTYPE,
                    QualificationUnit = cc.ZORG_QUAL,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    TaxationMethod = cc.ZTAXMETHOD
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 商机项目(不含境外商机项目) 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<BusinessNoCpportunityDetailsDto>> GetBusinessNoCpportunityDetailsAsync(string id)
        {
            var responseAjaxResult = new ResponseAjaxResult<BusinessNoCpportunityDetailsDto>();

            var result = await _dbContext.Queryable<BusinessNoCpportunity>()
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
        public async Task<ResponseAjaxResult<List<AdministrativeDivisionSearchDto>>> GetAdministrativeDivisionSearchAsync(AdministrativeDivisionRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<AdministrativeDivisionSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<AdministrativeDivision>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new AdministrativeDivisionSearchDto
                {
                    Id = cc.Id.ToString(),
                    CodeOfCCCCRegional = cc.ZCRHCODE,
                    RegionalismCode = cc.ZADDVSCODE,
                    RegionalismLevel = cc.ZADDVSLEVEL,
                    Name = cc.ZADDVSNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效"
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
        public async Task<ResponseAjaxResult<List<AccountingOrganizationSearchDto>>> GetAccountingOrganizationSearchAsync(AccountingOrganizationRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<AccountingOrganizationSearchDto>>();
            RefAsync<int> total = 0;

            var ccList = await _dbContext.Queryable<AccountingOrganization>()
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new AccountingOrganizationSearchDto
                {
                    Id = cc.Id.ToString(),
                    CountryName = cc.ZCYNAME,
                    Name = cc.ZZTNAME_ZH,
                    State = cc.ZOSTATE == "1" ? "有效" : "无效",
                    OrgELName = cc.ZZTNAME_EN,
                    OrgGruleCode = cc.ZORULE,
                    OrgLLanguage = cc.ZZTNAME_LOC,
                    OrgLocation = cc.ZORGLOC,
                    OrgShortName = cc.ZZTSHNAME_CHS,
                    RegionalAttr = cc.ZREGIONAL,
                    TelLocation = cc.ZADDRESS,
                    UnitSecCode = cc.ZGPOID
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
                    CountryName = cc.ZCYNAME,
                    EnterpriseCode = cc.ZENTC,
                    IsIndependenceAcc = cc.ZCHECKIND,
                    Name = cc.ZZTNAME_ZH,
                    LevelSequence = cc.ZSNO,
                    MeanOrgCode = cc.ZORGNO,
                    State = cc.ZOSTATE == "1" ? "有效" : "无效",
                    OldOrgCode = cc.OID,
                    OldSupOrgCode = cc.POID,
                    OrgAttr = cc.ZOATTR,
                    OrgChildAttr = cc.ZOCATTR,
                    OrgCode = cc.MDM_CODE,
                    OrgELName = cc.ZZTNAME_EN,
                    OrgELShortName = cc.ZZTSHNAME_EN,
                    OrgGruleCode = cc.ZORULE,
                    OrgLLanguage = cc.ZZTNAME_LOC,
                    OrgLocation = cc.ZORGLOC,
                    OrgShortName = cc.ZZTSHNAME_CHS,
                    OrgShortNameLLanguage = cc.ZZTSHNAME_LOC,
                    OrgTreeCode = cc.ZTREEID1,
                    OrgTreeVersion = cc.ZTREEVER,
                    RegionalAttr = cc.ZREGIONAL,
                    RegistrationNo = cc.ZCUSCC,
                    Shareholding = cc.ZHOLDING,
                    SupOrgCode = cc.ZORGUP,
                    TelLocation = cc.ZADDRESS,
                    TreeLevel = cc.ZO_LEVEL,
                    UnitSecCode = cc.ZGPOID,
                    ViewIdentifier = cc.VIEW_FLAG
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
    }
}
