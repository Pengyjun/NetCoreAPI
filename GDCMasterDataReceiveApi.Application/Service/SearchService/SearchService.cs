﻿using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
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
        public async Task<ResponseAjaxResult<List<LouDongDto>>> GetSearchLouDongAsync(LouDongRequestDto requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<LouDongDto>>();

            ////读取数据
            //var result = await _baseService.GetSearchListAsync<LouDongDto>("t_loudong", requestDto.Sql, requestDto.FilterParams, true, requestDto.PageIndex, requestDto.PageSize);

            //// 设置响应结果的总数
            //responseAjaxResult.Count = result.Count;
            //responseAjaxResult.SuccessResult(result);
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
    }
}
