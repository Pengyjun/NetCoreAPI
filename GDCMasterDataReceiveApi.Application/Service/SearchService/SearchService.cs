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
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.EscrowOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.NationalEconomy;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RelationalContracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ScientifiCNoProject;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System.Diagnostics.Metrics;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        /// 用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<UserSearchDetailsDto>>> GetUserSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<UserSearchDetailsDto>>();
            RefAsync<int> total = 0;

            //过滤条件
            UserSearchDetailsDto filterCondition = new();
            List<string?> oids = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<UserSearchDetailsDto>(requestDto.FilterConditionJson);
                if (filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CompanyName))
                {
                    oids = await _dbContext.Queryable<Institution>()
                    .Where(t => t.IsDelete == 1 && t.NAME.Contains(filterCondition.CompanyName))
                    .Select(t => t.OID)
                    .ToListAsync();

                }
            }
            #region 基本查询
            //获取人员信息
            var userInfos = await _dbContext.Queryable<User>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), u => u.NAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Phone), u => u.PHONE.Contains(filterCondition.Phone))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EmpCode), u => u.EMP_CODE.Contains(filterCondition.EmpCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Nationality), u => u.NATIONALITY.Contains(filterCondition.Nationality))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Email), u => u.EMAIL.Contains(filterCondition.Email))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CertNo), u => u.CERT_NO.Contains(filterCondition.CertNo))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OfficeDepId), u => u.OFFICE_DEPID.Contains(filterCondition.OfficeDepId))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UserInfoStatus), u => u.EMP_STATUS.Contains(filterCondition.UserInfoStatus))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Enable), u => u.Enable == Convert.ToInt32(filterCondition.Enable))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CompanyName), u => oids.Contains(filterCondition.OfficeDepId))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.NameSpell), u => u.NAME_SPELL.Contains(filterCondition.NameSpell))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EnName), u => u.EN_NAME.Contains(filterCondition.EnName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EmpCode), u => u.EMP_CODE.Contains(filterCondition.EmpCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CertType), u => u.CERT_TYPE.Contains(filterCondition.CertType))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Sex), u => u.SEX.Contains(filterCondition.Sex))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Nation), u => u.NATION.Contains(filterCondition.Nation))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PositionName), u => u.POSITION_NAME.Contains(filterCondition.PositionName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.JobName), u => u.JOB_NAME.Contains(filterCondition.JobName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EmpSort), u => u.EMP_SORT.Contains(filterCondition.EmpSort))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UserLogin), u => u.USER_LOGIN.Contains(filterCondition.UserLogin))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.HrEmpCode), u => u.HR_EMP_CODE.Contains(filterCondition.HrEmpCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EntryTime), u => u.ENTRY_TIME.Contains(filterCondition.EntryTime))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Tel), u => u.TEL.Contains(filterCondition.Tel))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fax), u => u.FAX.Contains(filterCondition.Fax))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OfficeNum), u => u.OFFICE_NUM.Contains(filterCondition.OfficeNum))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Attribute1), u => u.ATTRIBUTE1.Contains(filterCondition.Attribute1))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Attribute2), u => u.ATTRIBUTE2.Contains(filterCondition.Attribute2))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Attribute3), u => u.ATTRIBUTE3.Contains(filterCondition.Attribute3))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PositionGradeNorm), u => u.POSITIONGRADENORM.Contains(filterCondition.PositionGradeNorm))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.HighEstGrade), u => u.HIGHESTGRADE.Contains(filterCondition.HighEstGrade))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SameHighEstGrade), u => u.SAMEHIGHESTGRADE.Contains(filterCondition.SameHighEstGrade))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PoliticsFace), u => u.POLITICSFACE.Contains(filterCondition.PoliticsFace))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.DispatchunitName), u => u.DISPATCHUNITNAME.Contains(filterCondition.DispatchunitName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.DispatchunitShortName), u => u.DISPATCHUNITSHORTNAME.Contains(filterCondition.DispatchunitShortName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Externaluser), u => u.EXTERNALUSER.Contains(filterCondition.Externaluser))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Attribute4), u => u.ATTRIBUTE4.Contains(filterCondition.Attribute4))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Attribute5), u => u.ATTRIBUTE5.Contains(filterCondition.Attribute5))
                .WhereIF(filterCondition != null && filterCondition.Ids != null && filterCondition.Ids.Any(), u => filterCondition.Ids.Contains(u.Id.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                .Where(u => u.IsDelete == 1)
                .Select(u => new UserSearchDetailsDto
                {
                    Id = u.Id.ToString(),
                    Name = u.NAME,
                    CertNo = u.CERT_NO,
                    OfficeDepId = u.OFFICE_DEPID,
                    Email = u.EMAIL,
                    EmpCode = u.EMP_CODE,
                    Enable = u.Enable == 1 ? "有效" : "禁用",
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
                    CountryRegion = u.NATIONALITY,
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
                    UserInfoStatus = u.EMP_STATUS,
                    SubDepts = u.SUB_DEPTS,
                    Tel = u.TEL,
                    UserLogin = u.USER_LOGIN,
                    CreateTime = u.CreateTime,
                    UpdateTime = u.UpdateTime
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            #endregion

            if (userInfos != null && userInfos.Any())
            {
                //机构信息
                var institutions = await _dbContext.Queryable<Institution>()
                    .Where(t => t.IsDelete == 1)
                    .Select(t => new InstutionRespDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
                    .ToListAsync();

                //值域信息
                var valDomain = await _dbContext.Queryable<ValueDomain>()
                    .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                    .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
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
                var uStatus = valDomain.Where(x => us.Contains(x.ZDOM_VALUE) && x.ZDOM_CODE == "ZEMPSTATE").ToList();

                //民族
                var nationKeys = userInfos.Select(x => x.Nation).ToList();
                var nation = valDomain.Where(x => nationKeys.Contains(x.ZDOM_VALUE) && x.ZDOM_CODE == "ZNATION").ToList();

                //有效证件类型
                var certTypeKeys = userInfos.Select(x => x.CertType).ToList();
                var certType = valDomain.Where(x => certTypeKeys.Contains(x.ZDOM_VALUE) && x.ZDOM_CODE == "ZIDTYPE").ToList();

                //用工类型
                var empSortKeys = userInfos.Select(x => x.EmpSort).ToList();
                var empSort = valDomain.Where(x => empSortKeys.Contains(x.ZDOM_VALUE) && x.ZDOM_CODE == "ZEMPTYPE").ToList();

                //主职岗位类别
                var jobTypeKeys = userInfos.Select(x => x.JobType).ToList();
                var jobType = valDomain.Where(x => jobTypeKeys.Contains(x.ZDOM_VALUE) && x.ZDOM_CODE == "ZJOBTYPE").ToList();

                //最高职级
                var positionGradeKeys = userInfos.Select(x => x.PositionGrade).ToList();
                var positionGrade = valDomain.Where(x => positionGradeKeys.Contains(x.ZDOM_VALUE) && x.ZDOM_CODE == "ZEGRADE").ToList();

                //职级（新版）
                var positionGradeNormKeys = userInfos.Select(x => x.PositionGradeNorm).ToList();
                var positionGradeNorm = valDomain.Where(x => positionGradeNormKeys.Contains(x.ZDOM_VALUE) && x.ZDOM_CODE == "ZEGRADE").ToList();

                //新版最高职级
                var highEstGradeKeys = userInfos.Select(x => x.HighEstGrade).ToList();
                var highEstGrade = valDomain.Where(x => highEstGradeKeys.Contains(x.ZDOM_VALUE) && x.ZDOM_CODE == "ZEGRADE").ToList();

                //统一的最高职级
                var sameHighEstGradeKeys = userInfos.Select(x => x.SameHighEstGrade).ToList();
                var sameHighEstGrade = valDomain.Where(x => sameHighEstGradeKeys.Contains(x.ZDOM_VALUE) && x.ZDOM_CODE == "ZEGRADE").ToList();

                #endregion

                foreach (var uInfo in userInfos)
                {
                    uInfo.CompanyName = GetUserCompany(uInfo.OfficeDepId, institutions);
                    uInfo.OfficeDepIdName = institutions.FirstOrDefault(x => x.Oid == uInfo.OfficeDepId)?.Name;
                    uInfo.UserInfoStatus = uStatus.FirstOrDefault(x => x.ZDOM_VALUE == uInfo.UserInfoStatus)?.ZDOM_NAME;
                    uInfo.Nationality = contryRegion.FirstOrDefault(x => x.ZCOUNTRYCODE == uInfo.CountryRegion)?.ZCOUNTRYNAME;
                    uInfo.CountryRegion = contryRegion.FirstOrDefault(x => x.ZCOUNTRYCODE == uInfo.CountryRegion)?.ZCOUNTRYNAME;
                    uInfo.Nation = nation.FirstOrDefault(x => x.ZDOM_VALUE == uInfo.Nation)?.ZDOM_NAME;
                    uInfo.EmpSort = empSort.FirstOrDefault(x => x.ZDOM_VALUE == uInfo.EmpSort)?.ZDOM_NAME;
                    uInfo.PositionGrade = jobType.FirstOrDefault(x => x.ZDOM_VALUE == uInfo.PositionGrade)?.ZDOM_NAME;
                    uInfo.PositionGradeNorm = positionGradeNorm.FirstOrDefault(x => x.ZDOM_VALUE == uInfo.PositionGradeNorm)?.ZDOM_NAME;
                    uInfo.HighEstGrade = positionGradeNorm.FirstOrDefault(x => x.ZDOM_VALUE == uInfo.HighEstGrade)?.ZDOM_NAME;
                    uInfo.SameHighEstGrade = positionGradeNorm.FirstOrDefault(x => x.ZDOM_VALUE == uInfo.SameHighEstGrade)?.ZDOM_NAME;
                    uInfo.CertType = certType.FirstOrDefault(x => x.ZDOM_VALUE == uInfo.CertType)?.ZDOM_NAME;
                    uInfo.SubDepts = GetSubDepts(uInfo.SubDepts, institutions, valDomain, uInfo.JobName);
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
                .Where(u => !string.IsNullOrWhiteSpace(u.EMP_STATUS) && u.IsDelete == 1 && u.Id.ToString() == uId)
                .Select(u => new UserSearchDetailsDto
                {
                    Name = u.NAME,
                    CertNo = u.CERT_NO,
                    OfficeDepId = u.OFFICE_DEPID,
                    Email = u.EMAIL,
                    EmpCode = u.EMP_CODE,
                    Enable = u.Enable == 1 ? "有效" : "禁用",
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
                    UserInfoStatus = u.EMP_STATUS,
                    SubDepts = u.SUB_DEPTS,
                    Tel = u.TEL,
                    UserLogin = u.USER_LOGIN,
                    CreateTime = u.CreateTime,
                    UpdateTime = u.UpdateTime
                })
                .FirstAsync();

            #region 其他基本信息
            //获取机构信息
            var institutions = await _dbContext.Queryable<Institution>()
                .Where(t => t.IsDelete == 1)
                .Select(t => new InstutionRespDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
                .ToListAsync();

            //值域信息
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();

            //公司
            uDetails.CompanyName = GetUserCompany(uDetails.OfficeDepId, institutions);

            //国籍
            var name = await _dbContext.Queryable<CountryRegion>().FirstAsync(t => t.IsDelete == 1 && uDetails.Nationality == t.ZCOUNTRYCODE);
            uDetails.CountryRegion = name == null ? null : name.ZCOUNTRYNAME;
            uDetails.Nationality = name == null ? null : name.ZCOUNTRYNAME;

            //用户状态
            uDetails.UserInfoStatus = valDomain.FirstOrDefault(x => uDetails.UserInfoStatus == x.ZDOM_VALUE && x.ZDOM_CODE == "ZEMPSTATE")?.ZDOM_NAME;

            //民族
            uDetails.Nation = valDomain.FirstOrDefault(x => uDetails.Nation == x.ZDOM_VALUE && x.ZDOM_CODE == "ZNATION")?.ZDOM_NAME;

            //用工类型
            uDetails.EmpSort = valDomain.FirstOrDefault(x => uDetails.EmpSort == x.ZDOM_VALUE && x.ZDOM_CODE == "ZEMPTYPE")?.ZDOM_NAME;

            //有效证件类型
            uDetails.CertType = valDomain.FirstOrDefault(x => uDetails.CertType == x.ZDOM_VALUE && x.ZDOM_CODE == "ZIDTYPE")?.ZDOM_NAME;

            //主职岗位类别
            uDetails.JobType = valDomain.FirstOrDefault(x => uDetails.JobType == x.ZDOM_VALUE && x.ZDOM_CODE == "ZJOBTYPE")?.ZDOM_NAME;

            //最高职级
            uDetails.PositionGrade = valDomain.FirstOrDefault(x => uDetails.PositionGrade == x.ZDOM_VALUE && x.ZDOM_CODE == "ZEGRADE")?.ZDOM_NAME;

            //职级（新版）
            uDetails.PositionGradeNorm = valDomain.FirstOrDefault(x => uDetails.PositionGradeNorm == x.ZDOM_VALUE && x.ZDOM_CODE == "ZEGRADE")?.ZDOM_NAME;

            //新版最高职级
            uDetails.HighEstGrade = valDomain.FirstOrDefault(x => uDetails.HighEstGrade == x.ZDOM_VALUE && x.ZDOM_CODE == "ZEGRADE")?.ZDOM_NAME;

            //统一的最高职级
            uDetails.SameHighEstGrade = valDomain.FirstOrDefault(x => uDetails.SameHighEstGrade == x.ZDOM_VALUE && x.ZDOM_CODE == "ZEGRADE")?.ZDOM_NAME;

            //所属部门
            uDetails.OfficeDepIdName = institutions.FirstOrDefault(x => x.Oid == uDetails.OfficeDepId)?.Name;

            //兼职所在部门、岗位类别、职级、岗位名称及排序
            uDetails.SubDepts = GetSubDepts(uDetails.SubDepts, institutions, valDomain, uDetails.JobName);

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
        private string GetUserCompany(string? oid, List<InstutionRespDto> uInstutionDtos)
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
        /// 获取兼职所在部门、岗位类别、职级、岗位名称及排序
        /// </summary>
        /// <returns></returns>
        private string GetSubDepts(string? subDepts, List<InstutionRespDto> instutionDtos, List<VDomainRespDto> vDomainList, string? jobName)
        {
            string rd = string.Empty;
            if (!string.IsNullOrWhiteSpace(subDepts))
            {
                //先根据,拆
                var dSubDepts = subDepts.Split(',').ToList();
                if (dSubDepts != null && dSubDepts.Any())
                {
                    string val = string.Empty;
                    foreach (var ot in dSubDepts)
                    {
                        var values = ot.Split('|').ToList();
                        //根据|拆
                        for (int i = 0; i < values.Count; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    //所在部门
                                    if (values[i].Length > 0) val += instutionDtos.FirstOrDefault(x => x.Oid == values[i])?.Name;
                                    break;
                                case 1:
                                    //岗位类别
                                    if (values[i].Length > 0) val += vDomainList.FirstOrDefault(x => x.ZDOM_CODE == "ZJOBTYPE" && x.ZDOM_VALUE == values[i])?.ZDOM_NAME;
                                    break;
                                case 2:
                                    //职级
                                    if (values[i].Length > 0) val += vDomainList.FirstOrDefault(x => x.ZDOM_CODE == "ZEGRADE" && x.ZDOM_VALUE == values[i])?.ZDOM_NAME;
                                    break;
                                case 3:
                                    //岗位名称
                                    if (values[i].Length > 0) val += jobName;
                                    break;
                                case 4:
                                    //排序
                                    if (values[i].Length > 0) val += values[i];
                                    break;
                            }
                            val += "|";
                        }
                        val = !string.IsNullOrWhiteSpace(val) ? val.Substring(0, val.Length - 1) : val;
                        val += ",";
                    }
                    rd = !string.IsNullOrWhiteSpace(val) ? val.Substring(0, val.Length - 1) : val;
                }
            }

            return rd;
        }
        /// <summary>
        /// 获取机构名称
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="instutionDtos"></param>
        /// <returns></returns>
        private string? GetInstitutionName(string? oid, List<InstutionRespDto> instutionDtos)
        {
            var instutionName = instutionDtos.FirstOrDefault(x => x.Oid == oid)?.Name;
            return instutionName;
        }
        /// <summary>
        /// 根据值域获取名称
        /// </summary>
        /// <param name="zDomValue"></param>
        /// <param name="vDomainList"></param>
        /// <param name="zDomCode"></param>
        /// <returns></returns>
        private string? GetValueDomain(string? zDomValue, List<VDomainRespDto> vDomainList, string? zDomCode)
        {
            var vDomainName = vDomainList.FirstOrDefault(x => x.ZDOM_VALUE == zDomValue && x.ZDOM_CODE == zDomCode)?.ZDOM_NAME;
            return vDomainName;
        }
        /// <summary>
        /// 获取国家地区名称
        /// </summary>
        /// <param name="country"></param>
        /// <param name="countryRegionOrAdminDivisionDtos"></param>
        /// <returns></returns>
        private string? GetCountryRegion(string? country, List<CountryRegionOrAdminDivisionDto> countryRegionOrAdminDivisionDtos)
        {
            var countryName = countryRegionOrAdminDivisionDtos.FirstOrDefault(x => x.Code == country)?.Name;
            return countryName;
        }
        /// <summary>
        /// 获取省、市、县（行政区划）
        /// </summary>
        /// <param name="regionalismCode"></param>
        /// <param name="countryRegionOrAdminDivisionDtos"></param>
        /// <returns></returns>
        private string? GetAdministrativeDivision(string? regionalismCode, List<CountryRegionOrAdminDivisionDto> countryRegionOrAdminDivisionDtos)
        {
            var name = countryRegionOrAdminDivisionDtos.FirstOrDefault(x => x.Code == regionalismCode)?.Name;
            return name;
        }
        /// <summary>
        /// 往来单位  中标主体
        /// </summary>
        /// <param name="zbp"></param>
        /// <param name="corresUnits"></param>
        /// <returns></returns>
        private string? GetCorresunit(string? zbp, List<CorresUnit> corresUnits)
        {
            var name = corresUnits.FirstOrDefault(x => x.ZBP == zbp)?.ZBPNAME_ZH;
            return name;
        }
        /// <summary>
        /// 责任主体 行政组织
        /// </summary>
        /// <param name="mdmCode"></param>
        /// <param name="administrativeOrganizations"></param>
        /// <returns></returns>
        private string? GetXZZZ(string? mdmCode, List<AdministrativeOrganization> administrativeOrganizations)
        {
            var name = administrativeOrganizations.FirstOrDefault(x => x.MDM_CODE == mdmCode)?.ZZTNAME_ZH;
            return name;
        }
        /// <summary>
        /// 处理日期 yyyymmdd --  yyyy年mm月dd日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string? GetDate(string? date)
        {
            if (!string.IsNullOrWhiteSpace(date))
            {
                Utils.TryConvertDateTimeFromDateDay(Convert.ToInt32(date), out DateTime time);
                return time.ToString("yyyy年MM月dd日");
            }
            return null;
        }
        /// <summary>
        /// 处理日期 yyyymmddhhmmss -- yyyy年mm月dd日hh时mm分ss秒
        /// </summary>
        /// <param name="dayTime"></param>
        /// <returns></returns>
        private string? GetDateDay(string? dayTime)
        {
            if (!string.IsNullOrWhiteSpace(dayTime) && dayTime.Length == 14)
            {
                var year = dayTime.Substring(0, 4);
                var month = dayTime.Substring(4, 2);
                var day = dayTime.Substring(6, 2);
                var hh = dayTime.Substring(8, 2);
                var mm = dayTime.Substring(10, 2);
                var ss = dayTime.Substring(12, 2);
                DateTime.TryParse($"{year}-{month}-{day} {hh}:{mm}:{ss}", out DateTime time);
                return time.ToString("yyyy年MM月dd日 HH:mm:ss");
            }
            return null;
        }
        /// <summary>
        /// 机构树列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<InstitutionDetatilsDto>>> GetInstitutionAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<InstitutionDetatilsDto>>();
            var result = new List<InstitutionDetatilsDto>();
            List<string> oids = new();

            //过滤条件
            InstitutionDetatilsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<InstitutionDetatilsDto>(requestDto.FilterConditionJson);
            }

            #region 初始查询
            var tableList = await _dbContext.Queryable<Institution>().ToListAsync();
            //机构树初始化
            var institutions =  tableList
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), t => t.NAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Oid), t => t.OID.Contains(filterCondition.Oid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EntClass), t => t.ENTCLASS.Contains(filterCondition.EntClass))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.POid), t => t.POID.Contains(filterCondition.POid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Status), t => t.STATUS.Contains(filterCondition.Status))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Code), t => t.OCODE.Contains(filterCondition.Code))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Orule), t => t.ORULE.Contains(filterCondition.Orule))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ShortName), t => t.SHORTNAME.Contains(filterCondition.ShortName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EnglishName), t => t.ENGLISHNAME.Contains(filterCondition.EnglishName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EnglishShortName), t => t.ENGLISHSHORTNAME.Contains(filterCondition.EnglishShortName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BizType), t => t.BIZTYPE.Contains(filterCondition.BizType))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Gpoid), t => t.GPOID.Contains(filterCondition.Gpoid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Type), t => t.TYPE.Contains(filterCondition.Type))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TypeExt), t => t.TYPEEXT.Contains(filterCondition.TypeExt))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Mrut), t => t.MRUT.Contains(filterCondition.Mrut))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Sno), t => t.SNO.Contains(filterCondition.Sno))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Coid), t => t.COID.Contains(filterCondition.Coid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Crossorgan), t => t.CROSSORGAN.Contains(filterCondition.Crossorgan))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Goid), t => t.GOID.Contains(filterCondition.Goid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Grade), t => t.GRADE.Contains(filterCondition.Grade))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Oper), t => t.OPER.Contains(filterCondition.Oper))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Note), t => t.NOTE.Contains(filterCondition.Note))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TemorganName), t => t.TEMORGANNAME.Contains(filterCondition.TemorganName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgProvince), t => t.ORGPROVINCE.Contains(filterCondition.OrgProvince))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Carea), t => t.CAREA.Contains(filterCondition.Carea))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TerritoryPro), t => t.TERRITORYPRO.Contains(filterCondition.TerritoryPro))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BizDomain), t => t.BIZDOMAIN.Contains(filterCondition.BizDomain))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgGrade), t => t.ORGGRADE.Contains(filterCondition.OrgGrade))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ProjectScale), t => t.PROJECTSCALE.Contains(filterCondition.ProjectScale))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ProjectManType), t => t.PROJECTMANTYPE.Contains(filterCondition.ProjectManType))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ProjectType), t => t.PROJECTTYPE.Contains(filterCondition.ProjectType))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.StartDate), t => t.STARTDATE.Contains(filterCondition.StartDate))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Organemp), t => t.ORGANEMP.Contains(filterCondition.Organemp))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrganGrade), t => t.ORGANGRADE.Contains(filterCondition.OrganGrade))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Rown), t => t.ROWN.Contains(filterCondition.Rown))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.RoId), t => t.ROID.Contains(filterCondition.RoId))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.GlobalSno), t => t.GLOBAL_SNO.Contains(filterCondition.GlobalSno))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.QyGrade), t => t.QYGRADE.Contains(filterCondition.QyGrade))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.RegisterCode), t => t.REGISTERCODE.Contains(filterCondition.RegisterCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ShareHoldings), t => t.SHAREHOLDINGS.Contains(filterCondition.ShareHoldings))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.MdmCode), t => t.MDM_CODE.Contains(filterCondition.MdmCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                .Where(t => t.IsDelete == 1)
                .OrderBy(t => Convert.ToInt32(t.SNO))
                .ToList();
            #endregion

            //再次读取
            var roid = institutions.Select(x => x.ROID).ToList();
            var coid = institutions.Select(x => x.COID).ToList();
            var againInstutions = tableList
                .Where(t => t.IsDelete == 1 && (roid.Contains(t.ROID) || coid.Contains(t.COID)))
                .Select(t => new { t.OID, t.NAME })
                .ToList();

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
                else
                {
                    //得到所有符合条件的机构rules
                    var rules = institutions.Select(x => x.GRULE).ToList();

                    //拆分rules得到所有符合条件的oids
                    foreach (var r in rules)
                    {
                        if (!string.IsNullOrWhiteSpace(r)) oids.AddRange(r.Split('-'));
                    }
                }
            }

            #region 数据详细查询
            //值域信息
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();

            //行政区划（省、市、县）
            var advisionsKey = institutions.Select(x => x.ORGPROVINCE).ToList();
            var advisions = await _dbContext.Queryable<AdministrativeDivision>()
                .Where(t => t.IsDelete == 1 && advisionsKey.Contains(t.ZADDVSCODE))
                .Select(t => new { t.ZADDVSCODE, t.ZADDVSNAME })
                .ToListAsync();

            //国家
            var careasKey = institutions.Select(x => x.CAREA).ToList();
            var carea = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1 && careasKey.Contains(t.ZCOUNTRYCODE))
                .Select(t => new { t.ZCOUNTRYCODE, t.ZCOUNTRYNAME })
                .ToListAsync();

            //机构业务类型
            var institutionBusType = await _dbContext.Queryable<InstitutionBusinessType>()
                .Where(t => t.IsDelete == 1)
                .Select(t => new FilterChildData { Key = t.Code, Val = t.Name })
                .ToListAsync();

            #region 转换赋值
            // 将 valDomain 和其他相关集合转换为字典，避免重复键
            var entClassDict = valDomain
                .Where(x => x.ZDOM_CODE == "ZENTC")
                .GroupBy(x => x.ZDOM_VALUE)
                .Select(g => g.First()) // 只保留第一个
                .ToDictionary(x => x.ZDOM_VALUE, x => x.ZDOM_NAME);

            var statusDict = valDomain
                .Where(x => x.ZDOM_CODE == "ZORGSTATE")
                .GroupBy(x => x.ZDOM_VALUE)
                .Select(g => g.First()) // 只保留第一个
                .ToDictionary(x => x.ZDOM_VALUE, x => x.ZDOM_NAME);

            var typeDict = valDomain
                .Where(x => x.ZDOM_CODE == "ZORGATTR")
                .GroupBy(x => x.ZDOM_VALUE)
                .Select(g => g.First()) // 只保留第一个
                .ToDictionary(x => x.ZDOM_VALUE, x => x.ZDOM_NAME);

            var typeExtDict = valDomain
                .Where(x => x.ZDOM_CODE == "ZORGCHILDATTR")
                .GroupBy(x => x.ZDOM_VALUE)
                .Select(g => g.First()) // 只保留第一个
                .ToDictionary(x => x.ZDOM_VALUE, x => x.ZDOM_NAME);

            var isIndependentDict = valDomain
                .Where(x => x.ZDOM_CODE == "ZCHECKIND")
                .GroupBy(x => x.ZDOM_VALUE)
                .Select(g => g.First()) // 只保留第一个
                .ToDictionary(x => x.ZDOM_VALUE, x => x.ZDOM_NAME);

            var shareholdingsDict = valDomain
                .Where(x => x.ZDOM_CODE == "ZHOLDING")
                .GroupBy(x => x.ZDOM_VALUE)
                .Select(g => g.First()) // 只保留第一个
                .ToDictionary(x => x.ZDOM_VALUE, x => x.ZDOM_NAME);

            var projectTypeDict = valDomain
                .Where(x => x.ZDOM_CODE == "ZPROJTYPE")
                .GroupBy(x => x.ZDOM_VALUE)
                .Select(g => g.First()) // 只保留第一个
                .ToDictionary(x => x.ZDOM_VALUE, x => x.ZDOM_NAME);

            var bizTypeDict = institutionBusType
                .GroupBy(x => x.Key)
                .Select(g => g.First()) // 只保留第一个
                .ToDictionary(x => x.Key, x => x.Val);

            var provinceDict = advisions
                .GroupBy(x => x.ZADDVSCODE)
                .Select(g => g.First()) // 只保留第一个
                .ToDictionary(x => x.ZADDVSCODE, x => x.ZADDVSNAME);

            var countryDict = carea
                .GroupBy(x => x.ZCOUNTRYCODE)
                .Select(g => g.First()) // 只保留第一个
                .ToDictionary(x => x.ZCOUNTRYCODE, x => x.ZCOUNTRYNAME);

            var againInstitutionDict = againInstutions
                .GroupBy(x => x.OID)
                .Select(g => g.First()) // 只保留第一个
                .ToDictionary(x => x.OID, x => x.NAME);
            #endregion

            foreach (var ins in institutions)
            {
                // 企业分类
                ins.ENTCLASS = entClassDict.TryGetValue(ins.ENTCLASS, out var entClassName) ? entClassName : null;
                // 机构状态
                ins.STATUS = statusDict.TryGetValue(ins.STATUS, out var statusName) ? statusName : null;

                // 机构属性
                ins.TYPE = typeDict.TryGetValue(ins.TYPE, out var typeName) ? typeName : null;

                // 机构子属性
                ins.TYPEEXT = typeExtDict.TryGetValue(ins.TYPEEXT, out var typeExtName) ? typeExtName : null;

                // 拥有兼管职能
                ins.CROSSORGAN = ins.CROSSORGAN == "0" ? "否" : "是";

                // 机构所在地（省）
                ins.ORGPROVINCE = provinceDict.TryGetValue(ins.ORGPROVINCE, out var provinceName) ? provinceName : null;

                // 国家
                ins.CAREA = countryDict.TryGetValue(ins.CAREA, out var countryName) ? countryName : null;

                // 独立授权
                ins.ROWN = ins.ROWN == "1" ? "是" : "否";

                // 授权机构
                ins.ROID = againInstitutionDict.TryGetValue(ins.ROID, out var roidName) ? roidName : null;

                // 隶属单位
                ins.COID = againInstitutionDict.TryGetValue(ins.COID, out var coidName) ? coidName : null;

                // 是否独立核算
                ins.IS_INDEPENDENT = isIndependentDict.TryGetValue(ins.IS_INDEPENDENT, out var independentName) ? independentName : null;

                // 持股情况
                ins.SHAREHOLDINGS = shareholdingsDict.TryGetValue(ins.SHAREHOLDINGS, out var holdingsName) ? holdingsName : null;

                // 项目类型
                ins.PROJECTTYPE = projectTypeDict.TryGetValue(ins.PROJECTTYPE, out var projectTypeName) ? projectTypeName : null;

                // 机构业务类型
                ins.BIZTYPE = bizTypeDict.TryGetValue(ins.BIZTYPE, out var bizTypeName) ? bizTypeName : null;
                #region MyRegion

                ////企业分类
                //ins.ENTCLASS = valDomain.FirstOrDefault(x => ins.ENTCLASS == x.ZDOM_VALUE && x.ZDOM_CODE == "ZENTC")?.ZDOM_NAME;

                ////机构状态
                //ins.STATUS = valDomain.FirstOrDefault(x => ins.STATUS == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGSTATE")?.ZDOM_NAME;

                ////机构属性
                //ins.TYPE = valDomain.FirstOrDefault(x => ins.TYPE == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGATTR")?.ZDOM_NAME;

                ////机构子属性
                //ins.TYPEEXT = valDomain.FirstOrDefault(x => ins.TYPEEXT == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGCHILDATTR")?.ZDOM_NAME;

                ////拥有兼管职能
                //ins.CROSSORGAN = ins.CROSSORGAN == "0" ? "否" : "是";

                ////机构所在地（省）
                //ins.ORGPROVINCE = advisions.FirstOrDefault(x => x.ZADDVSCODE == ins.ORGPROVINCE)?.ZADDVSNAME;

                ////国家
                //ins.CAREA = carea.FirstOrDefault(x => x.ZCOUNTRYCODE == ins.CAREA)?.ZCOUNTRYNAME;

                ////独立授权
                //ins.ROWN = ins.ROWN == "1" ? "是" : "否";

                ////授权机构
                //ins.ROID = againInstutions.FirstOrDefault(x => x.OID == ins.ROID)?.NAME;

                ////隶属单位
                //ins.COID = againInstutions.FirstOrDefault(x => x.OID == ins.COID)?.NAME;

                ////是否独立核算
                //ins.IS_INDEPENDENT = valDomain.FirstOrDefault(x => ins.IS_INDEPENDENT == x.ZDOM_VALUE && x.ZDOM_CODE == "ZCHECKIND")?.ZDOM_NAME;

                ////持股情况
                //ins.SHAREHOLDINGS = valDomain.FirstOrDefault(x => ins.SHAREHOLDINGS == x.ZDOM_VALUE && x.ZDOM_CODE == "ZHOLDING")?.ZDOM_NAME;

                ////项目类型
                //ins.PROJECTTYPE = valDomain.FirstOrDefault(x => ins.PROJECTTYPE == x.ZDOM_VALUE && x.ZDOM_CODE == "ZPROJTYPE")?.ZDOM_NAME;

                ////机构业务类型
                //ins.BIZTYPE = institutionBusType.FirstOrDefault(x => ins.BIZTYPE == x.Key)?.Val;

                #endregion
            }

            #endregion

            //其他数据
            var otherNodes = tableList
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
                    Version = ins.VERSION,
                    CreateTime = ins.CreateTime,
                    UpdateTime = ins.UpdateTime
                })
                .ToList();

            //根节点
            var rootNode = tableList
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
                    CreateTime = ins.CreateTime,
                    UpdateTime = ins.UpdateTime,
                    Children = GetChildren(ins.OID, otherNodes)
                })
                .FirstOrDefault();
            if (rootNode != null) result.Add(rootNode);

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
                    Version = ins.VERSION,
                    CreateTime = ins.CreateTime,
                    UpdateTime = ins.UpdateTime
                })
                .FirstAsync();

            var againInstutions = await _dbContext.Queryable<Institution>()
              .Where(t => t.IsDelete == 1 && (t.COID == insDetails.Coid || t.ROID == insDetails.RoId || t.GPOID == insDetails.Gpoid))
              .Select(t => new { t.OID, t.NAME })
              .ToListAsync();

            #region 处理其他基本信息数据
            //值域信息
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();

            //机构业务类型
            var bizType = await _dbContext.Queryable<InstitutionBusinessType>()
                .Where(t => t.IsDelete == 1 && insDetails.BizType == t.Code)
                .FirstAsync();
            insDetails.BizType = bizType == null ? null : bizType.Name;

            //行政区划（省、市、县）
            var advisions = await _dbContext.Queryable<AdministrativeDivision>()
                .Where(t => t.IsDelete == 1 && insDetails.OrgProvince == t.ZADDVSCODE)
                .FirstAsync();
            insDetails.OrgProvince = advisions == null ? null : advisions.ZADDVSNAME;

            //国家
            var carea = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1 && insDetails.Carea == t.ZCOUNTRYCODE)
                .FirstAsync();
            insDetails.Carea = carea == null ? null : carea.ZCOUNTRYNAME;

            //企业分类
            insDetails.EntClass = valDomain.FirstOrDefault(x => insDetails.EntClass == x.ZDOM_VALUE && x.ZDOM_CODE == "ZENTC")?.ZDOM_NAME;

            //机构状态
            insDetails.Status = valDomain.FirstOrDefault(x => insDetails.Status == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGSTATE")?.ZDOM_NAME;

            //机构属性
            insDetails.Type = valDomain.FirstOrDefault(x => insDetails.Type == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGATTR")?.ZDOM_NAME;

            //机构子属性
            insDetails.TypeExt = valDomain.FirstOrDefault(x => insDetails.TypeExt == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGCHILDATTR")?.ZDOM_NAME;

            //拥有兼管职能
            insDetails.Crossorgan = insDetails.Crossorgan == "0" ? "否" : "是";

            //独立授权
            insDetails.Rown = insDetails.Rown == "1" ? "是" : "否";

            //隶属单位
            insDetails.Coid = againInstutions.FirstOrDefault(x => x.OID == insDetails.Coid)?.NAME;

            //分组机构id
            insDetails.Gpoid = againInstutions.FirstOrDefault(x => x.OID == insDetails.Gpoid)?.NAME;

            //授权机构
            insDetails.RoId = againInstutions.FirstOrDefault(x => x.OID == insDetails.RoId)?.NAME;

            //是否独立核算
            insDetails.IsIndependent = valDomain.FirstOrDefault(x => insDetails.IsIndependent == x.ZDOM_VALUE && x.ZDOM_CODE == "ZSingleIND")?.ZDOM_NAME;

            //持股情况
            insDetails.ShareHoldings = valDomain.FirstOrDefault(x => insDetails.ShareHoldings == x.ZDOM_VALUE && x.ZDOM_CODE == "ZHOLDING")?.ZDOM_NAME;

            //项目类型
            insDetails.ProjectType = valDomain.FirstOrDefault(x => insDetails.ProjectType == x.ZDOM_VALUE && x.ZDOM_CODE == "ZPROJTYPE")?.ZDOM_NAME;



            #endregion


            responseAjaxResult.SuccessResult(insDetails);
            return responseAjaxResult;
        }
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DHProjects>>> GetProjectSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DHProjects>>();
            RefAsync<int> total = 0;

            //过滤条件
            DHProjects filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<DHProjects>(requestDto.FilterConditionJson);
            }

            #region 项目初始查询
            var proList = await _dbContext.Queryable<DHProjects>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPROJECT), (pro) => pro.ZPROJECT.Contains(filterCondition.ZPROJECT))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPROJNAME), (pro) => pro.ZPROJNAME.Contains(filterCondition.ZPROJNAME))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPROJENAME), (pro) => pro.ZPROJENAME.Contains(filterCondition.ZPROJENAME))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPROJECTID), (pro) => pro.ZPROJECTID.Contains(filterCondition.ZPROJECTID))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPROJTYPE), (pro) => pro.ZPROJTYPE.Contains(filterCondition.ZPROJTYPE))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPROJLOC), (pro) => pro.ZPROJLOC.Contains(filterCondition.ZPROJLOC))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZCPBC), (pro) => pro.ZCPBC.Contains(filterCondition.ZCPBC))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZINVERSTOR), (pro) => pro.ZINVERSTOR.Contains(filterCondition.ZINVERSTOR))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZAPPROVAL), (pro) => pro.ZAPPROVAL.Contains(filterCondition.ZAPPROVAL))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZAPVLDATE.ToString()), (pro) => pro.ZAPVLDATE.ToString().Contains(filterCondition.ZAPVLDATE.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZSI), (pro) => pro.ZSI.Contains(filterCondition.ZSI))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPRO_ORG), (pro) => pro.ZPRO_ORG.Contains(filterCondition.ZPRO_ORG))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZHEREINAFTER), (pro) => pro.ZHEREINAFTER.Contains(filterCondition.ZHEREINAFTER))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPROJECTUP), (pro) => pro.ZPROJECTUP.Contains(filterCondition.ZPROJECTUP))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPROJYEAR), (pro) => pro.ZPROJYEAR.Contains(filterCondition.ZPROJYEAR))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZSTARTDATE.ToString()), (pro) => pro.ZSTARTDATE.ToString().Contains(filterCondition.ZSTARTDATE.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZFINDATE.ToString()), (pro) => pro.ZFINDATE.ToString().Contains(filterCondition.ZFINDATE.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZENG), (pro) => pro.ZENG.Contains(filterCondition.ZENG))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZBLOCK), (pro) => pro.ZBLOCK.Contains(filterCondition.ZBLOCK))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZRESP), (pro) => pro.ZRESP.Contains(filterCondition.ZRESP))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZLDLOC), (pro) => pro.ZLDLOC.Contains(filterCondition.ZLDLOC))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZLDLOCGT.ToString()), (pro) => pro.ZLDLOCGT.ToString().Contains(filterCondition.ZLDLOCGT.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZCBR.ToString()), (pro) => pro.ZCBR.ToString().Contains(filterCondition.ZCBR.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZTRADER), (pro) => pro.ZTRADER.Contains(filterCondition.ZTRADER))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZINSURANCE), (pro) => pro.ZINSURANCE.Contains(filterCondition.ZINSURANCE))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPOLICYNO), (pro) => pro.ZPOLICYNO.Contains(filterCondition.ZPOLICYNO))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZINSURED), (pro) => pro.ZINSURED.Contains(filterCondition.ZINSURED))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZISTARTDATE.ToString()), (pro) => pro.ZISTARTDATE.ToString().Contains(filterCondition.ZISTARTDATE.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZIFINDATE.ToString()), (pro) => pro.ZIFINDATE.ToString().Contains(filterCondition.ZIFINDATE.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZFUND), (pro) => pro.ZFUND.Contains(filterCondition.ZFUND))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZFUNDNAME), (pro) => pro.ZFUNDNAME.Contains(filterCondition.ZFUNDNAME))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZFUNDNO), (pro) => pro.ZFUNDNO.Contains(filterCondition.ZFUNDNO))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZZCURRENCY), (pro) => pro.ZZCURRENCY.Contains(filterCondition.ZZCURRENCY))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZFUNDORGFORM), (pro) => pro.ZFUNDORGFORM.Contains(filterCondition.ZFUNDORGFORM))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPRO_BP), (pro) => pro.ZPRO_BP.Contains(filterCondition.ZPRO_BP))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZFUNDMTYPE), (pro) => pro.ZFUNDMTYPE.Contains(filterCondition.ZFUNDMTYPE))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZFSTARTDATE.ToString()), (pro) => pro.ZFSTARTDATE.ToString().Contains(filterCondition.ZFSTARTDATE.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZFFINDATE.ToString()), (pro) => pro.ZFFINDATE.ToString().Contains(filterCondition.ZFFINDATE.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZCUSTODIAN), (pro) => pro.ZCUSTODIAN.Contains(filterCondition.ZCUSTODIAN))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZLESSEE), (pro) => pro.ZLESSEE.Contains(filterCondition.ZLESSEE))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZLESSEETYPE), (pro) => pro.ZLESSEETYPE.Contains(filterCondition.ZLESSEETYPE))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZLEASESNAME), (pro) => pro.ZLEASESNAME.Contains(filterCondition.ZLEASESNAME))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZLSTARTDATE.ToString()), (pro) => pro.ZLSTARTDATE.ToString().Contains(filterCondition.ZLSTARTDATE.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZLFINDATE.ToString()), (pro) => pro.ZLFINDATE.ToString().Contains(filterCondition.ZLFINDATE.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZOLDNAME), (pro) => pro.ZOLDNAME.Contains(filterCondition.ZOLDNAME))
                //.WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Z2NDORG), (pro) => pro.Z2NDORG.Contains(filterCondition.Z2NDORG))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZSTOPREASON), (pro) => pro.ZSTOPREASON.Contains(filterCondition.ZSTOPREASON))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZTAXMETHOD), (pro) => pro.ZTAXMETHOD.Contains(filterCondition.ZTAXMETHOD))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPOS), (pro) => pro.ZPOS.Contains(filterCondition.ZPOS))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZAWARDMAI), (pro) => pro.ZAWARDMAI.Contains(filterCondition.ZAWARDMAI))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZCS), (pro) => pro.ZCS.Contains(filterCondition.ZCS))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZBIZDEPT), (pro) => pro.ZBIZDEPT.Contains(filterCondition.ZBIZDEPT))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZSTATE), (pro) => pro.ZSTATE.Contains(filterCondition.ZSTATE))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zdelete), (pro) => pro.Zdelete.Contains(filterCondition.IsDelete.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FZmanagemode), (pro) => pro.FZmanagemode.Contains(filterCondition.FZmanagemode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FZcy2ndorg), (pro) => pro.FZcy2ndorg.Contains(filterCondition.FZcy2ndorg))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FZwinningc), (pro) => pro.FZwinningc.Contains(filterCondition.FZwinningc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FZawardp), (pro) => pro.FZawardp.Contains(filterCondition.FZawardp))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreatedAt.ToString()), (pro) => Convert.ToDateTime(pro.CreatedAt).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreatedAt).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdatedAt.ToString()), (pro) => Convert.ToDateTime(pro.UpdatedAt).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdatedAt).ToString("yyyy-MM-dd"))
                .Where((pro) => pro.IsDelete == 1)
                .Select((pro) => new DHProjects
                {
                    IsDelete = pro.IsDelete,
                    Zdelete = pro.Zdelete,
                    CreatedAt = pro.CreatedAt,
                    FZawardp = pro.FZawardp,
                    CreateId = pro.CreateId,
                    DeleteId = pro.DeleteId,
                    DeleteTime = pro.DeleteTime,
                    FZcy2ndorg = pro.FZcy2ndorg,
                    FZmanagemode = pro.FZmanagemode,
                    FZwinningc = pro.FZwinningc,
                    UpdatedAt = pro.UpdatedAt,
                    //Z2NDORG = pro.Z2NDORG,
                    Id = pro.Id,
                    ZAPPROVAL = pro.ZAPPROVAL,
                    ZAPVLDATE = pro.ZAPVLDATE,
                    ZAWARDMAI = pro.ZAWARDMAI,
                    ZBIZDEPT = pro.ZBIZDEPT,
                    ZBLOCK = pro.ZBLOCK,
                    ZSI = pro.ZSI,
                    ZCBR = pro.ZCBR,
                    ZCPBC = pro.ZCPBC,
                    ZCS = pro.ZCS,
                    ZCUSTODIAN = pro.ZCUSTODIAN,
                    ZENG = pro.ZENG,
                    ZFFINDATE = pro.ZFFINDATE,
                    ZFINDATE = pro.ZFINDATE,
                    ZFSTARTDATE = pro.ZFSTARTDATE,
                    ZFUND = pro.ZFUND,
                    ZFUNDMTYPE = pro.ZFUNDMTYPE,
                    ZFUNDNAME = pro.ZFUNDNAME,
                    ZFUNDNO = pro.ZFUNDNO,
                    ZFUNDORGFORM = pro.ZFUNDORGFORM,
                    ZHEREINAFTER = pro.ZHEREINAFTER,
                    ZIFINDATE = pro.ZIFINDATE,
                    ZINSURANCE = pro.ZINSURANCE,
                    ZINSURED = pro.ZINSURED,
                    ZINVERSTOR = pro.ZINVERSTOR,
                    ZISTARTDATE = pro.ZISTARTDATE,
                    ZLDLOC = pro.ZLDLOC,
                    ZLDLOCGT = pro.ZLDLOCGT,
                    ZLEASESNAME = pro.ZLEASESNAME,
                    ZSTATE = pro.ZSTATE,
                    ZLESSEE = pro.ZLESSEE,
                    ZLESSEETYPE = pro.ZLESSEETYPE,
                    ZLFINDATE = pro.ZLFINDATE,
                    ZLSTARTDATE = pro.ZLSTARTDATE,
                    ZOLDNAME = pro.ZOLDNAME,
                    ZPOLICYNO = pro.ZPOLICYNO,
                    ZPOS = pro.ZPOS,
                    ZPROJECT = pro.ZPROJECT,
                    ZPROJECTID = pro.ZPROJECTID,
                    ZPROJECTUP = pro.ZPROJECTUP,
                    ZPROJENAME = pro.ZPROJENAME,
                    ZPROJLOC = pro.ZPROJLOC,
                    ZPROJNAME = pro.ZPROJNAME,
                    ZPROJTYPE = pro.ZPROJTYPE,
                    ZPROJYEAR = pro.ZPROJYEAR,
                    ZPRO_BP = pro.ZPRO_BP,
                    ZPRO_ORG = pro.ZPRO_ORG,
                    ZRESP = pro.ZRESP,
                    ZSTARTDATE = pro.ZSTARTDATE,
                    ZSTOPREASON = pro.ZSTOPREASON,
                    ZTAXMETHOD = pro.ZTAXMETHOD,
                    ZTRADER = pro.ZTRADER,
                    Zversion = pro.Zversion,
                    ZZCOUNTRY = pro.ZZCOUNTRY,
                    ZZCURRENCY = pro.ZZCURRENCY,
                    UpdateId = pro.UpdateId,
                    CreateTime = pro.CreateTime,
                    UpdateTime = pro.UpdateTime
                })
                .OrderByDescending(x => x.UpdatedAt)
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
            #endregion

            #region 详细查询
            //值域信息
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();

            #region 原来的

            ////国家地区
            //var countrysKey = proList.Select(x => x.Country).ToList();
            //var country = await _dbContext.Queryable<CountryRegion>()
            //    .Where(t => t.IsDelete == 1 && countrysKey.Contains(t.ZCOUNTRYCODE))
            //    .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZCOUNTRYCODE, Name = t.ZCOUNTRYNAME })
            //    .ToListAsync();

            ////行政区划
            //var locationKey = proList.Select(x => x.Location).ToList();
            //var location = await _dbContext.Queryable<AdministrativeDivision>()
            //    .Where(t => t.IsDelete == 1 && locationKey.Contains(t.ZADDVSCODE))
            //    .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZADDVSCODE, Name = t.ZADDVSNAME })
            //    .ToListAsync();

            ////项目机构
            //var pjectOrgKey = proList.Select(x => x.PjectOrg).ToList();
            //var pjectOrg2Key = proList.Select(x => x.PjectOrgBP).ToList();
            //var pjectOrg = await _dbContext.Queryable<Institution>()
            //    .Where(t => t.IsDelete == 1 && (pjectOrgKey.Contains(t.OID) || pjectOrg2Key.Contains(t.OID)))
            //    .Select(t => new InstutionRespDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
            //    .ToListAsync();

            //foreach (var item in proList)
            //{
            //    //项目类型
            //    item.Type = GetValueDomain(item.Type, valDomain, "ZPROJTYPE");

            //    //国家地区
            //    item.Country = GetCountryRegion(item.Country, country);

            //    //项目所在地
            //    item.Location = GetAdministrativeDivision(item.Location, location);

            //    //项目机构
            //    item.PjectOrg = GetInstitutionName(item.PjectOrg, pjectOrg);
            //    item.PjectOrgBP = GetInstitutionName(item.PjectOrgBP, pjectOrg);

            //    //日期
            //    item.PlanStartDate = GetDate(item.PlanStartDate);
            //    item.PlanCompletionDate = GetDate(item.PlanCompletionDate);
            //    item.AcquisitionTime = GetDate(item.AcquisitionTime);
            //    item.BChangeTime = GetDate(item.BChangeTime);
            //    item.StartDateOfInsure = GetDate(item.StartDateOfInsure);
            //    item.EndDateOfInsure = GetDate(item.EndDateOfInsure);
            //    item.FundEstablishmentDate = GetDate(item.FundEstablishmentDate);
            //    item.FundExpirationDate = GetDate(item.FundExpirationDate);
            //    item.LeaseStartDate = GetDate(item.LeaseStartDate);
            //    item.DueDate = GetDate(item.DueDate);
            //    item.CreateDate = GetDateDay(item.CreateDate);
            //    item.ResolutionTime = GetDate(item.ResolutionTime);

            //    //收入来源
            //    item.SourceOfIncome = GetValueDomain(item.SourceOfIncome, valDomain, "ZSI");

            //    //操盘情况
            //    item.TradingSituation = GetValueDomain(item.TradingSituation, valDomain, "ZTRADER");

            //    //承租人类型
            //    item.TenantType = GetValueDomain(item.TenantType, valDomain, "ZLESSEETYPE");

            //    //参与二级单位
            //    item.ParticipateInUnitSecs = GetValueDomain(item.ParticipateInUnitSecs, valDomain, "ZCY2NDORG");

            //    //计税方式
            //    item.TaxMethod = GetValueDomain(item.TaxMethod, valDomain, "ZTAXMETHOD");

            //    //并表情况
            //    item.ConsolidatedTable = GetValueDomain(item.ConsolidatedTable, valDomain, "ZCS");

            //    //是否联合体项目
            //    item.IsJoint = item.IsJoint == "1" ? "是" : "否";



            //}
            #endregion

            #region DH
            //国家地区
            var countrysKey = proList.Select(x => x.ZZCOUNTRY).ToList();
            var country = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1 && countrysKey.Contains(t.ZCOUNTRYCODE))
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZCOUNTRYCODE, Name = t.ZCOUNTRYNAME })
                .ToListAsync();

            //行政区划
            var locationKey = proList.Select(x => x.ZPROJLOC).ToList();
            var location = await _dbContext.Queryable<AdministrativeDivision>()
                .Where(t => t.IsDelete == 1 && locationKey.Contains(t.ZADDVSCODE))
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZADDVSCODE, Name = t.ZADDVSNAME })
                .ToListAsync();

            //项目机构 //商机项目机构
            var pjectOrgKey = proList.Select(x => x.ZPRO_ORG).ToList();
            var pjectOrg2Key = proList.Select(x => x.ZPRO_BP).ToList();
            //var pjectOrg3Key = proList.Select(x => x.Z2NDORG).ToList();
            var pjectOrg = await _dbContext.Queryable<Institution>()
                .Where(t => t.IsDelete == 1 && (pjectOrgKey.Contains(t.OID) || pjectOrg2Key.Contains(t.OID)))
                .Select(t => new InstutionRespDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
                .ToListAsync();

            //中标主体 往来单位
            var ZAWARDMAIIds = proList.Select(x => x.ZAWARDMAI).ToList();
            var zbzt = await _dbContext.Queryable<CorresUnit>()
                .Where(t => t.IsDelete == 1 && ZAWARDMAIIds.Contains(t.ZBP))
                .ToListAsync();

            foreach (var item in proList)
            {
                //曾用名
                if (!string.IsNullOrWhiteSpace(item.ZOLDNAME))
                {
                    var jsonObject = JObject.Parse(item.ZOLDNAME);
                    if (!string.IsNullOrWhiteSpace(jsonObject["item"].ToString()))
                    {
                        item.FzitOnameList = JsonConvert.DeserializeObject<List<FzitOnames>>(jsonObject["item"].ToString());
                    }
                }

                //项目类型
                item.ZPROJTYPE = GetValueDomain(item.ZPROJTYPE, valDomain, "ZPROJTYPE");

                //国家地区
                item.ZZCOUNTRY = GetCountryRegion(item.ZZCOUNTRY, country);

                //项目所在地
                item.ZPROJLOC = GetAdministrativeDivision(item.ZPROJLOC, location);

                //项目机构 //商机项目机构
                item.ZPRO_ORG = GetInstitutionName(item.ZPRO_ORG, pjectOrg);
                item.ZPRO_BP = GetInstitutionName(item.ZPRO_BP, pjectOrg);

                //收入来源
                item.ZSI = GetValueDomain(item.ZSI, valDomain, "ZSI");

                //操盘情况
                item.ZTRADER = GetValueDomain(item.ZTRADER, valDomain, "ZTRADER");

                //承租人类型
                item.ZLESSEETYPE = GetValueDomain(item.ZLESSEETYPE, valDomain, "ZLESSEETYPE");

                //参与二级单位
                item.FZcy2ndorg = GetValueDomain(item.FZcy2ndorg, valDomain, "ZCY2NDORG");

                //计税方式
                item.ZTAXMETHOD = GetValueDomain(item.ZTAXMETHOD, valDomain, "ZTAXMETHOD");

                //并表情况
                item.ZCS = GetValueDomain(item.ZCS, valDomain, "ZCS");

                //是否联合体项目
                item.FZwinningc = item.FZwinningc == "1" ? "是" : "否";

                //中交项目业务分类
                item.ZCPBC = GetValueDomain(item.ZCPBC, valDomain, "ZCPBC");

                //项目组织形式
                item.ZPOS = GetValueDomain(item.ZPOS, valDomain, "ZPOS");

                //所属事业部
                item.ZBIZDEPT = GetValueDomain(item.ZBIZDEPT, valDomain, "ZBIZDEPT");

                //参与二级单位
                item.FZcy2ndorg = GetValueDomain(item.FZcy2ndorg, valDomain, "ZCY2NDORG");

                //基金组织形式
                item.ZFUNDORGFORM = GetValueDomain(item.ZFUNDORGFORM, valDomain, "ZCY2NDORG");

                ////承租人类型
                item.ZLESSEETYPE = GetValueDomain(item.ZLESSEETYPE, valDomain, "ZLESSEETYPE");

                //项目管理方式
                item.FZmanagemode = GetValueDomain(item.FZmanagemode, valDomain, "ZMANAGE_MODE");

                //中标主体
                item.ZAWARDMAI = GetCorresunit(item.ZAWARDMAI, zbzt);

            }
            #endregion
            #endregion

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(proList);
            return responseAjaxResult;
        }
        //public async Task<ResponseAjaxResult<List<ProjectDetailsDto>>> GetProjectSearchAsync(FilterCondition requestDto)
        //{
        //    var responseAjaxResult = new ResponseAjaxResult<List<ProjectDetailsDto>>();
        //    RefAsync<int> total = 0;

        //    //过滤条件
        //    ProjectDetailsDto filterCondition = new();
        //    if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
        //    {
        //        filterCondition = JsonConvert.DeserializeObject<ProjectDetailsDto>(requestDto.FilterConditionJson);
        //    }

        //    #region 项目初始查询
        //    var proList = await _dbContext.Queryable<Project>()
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZPROJNAME.Contains(filterCondition.Name))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.MDCode), (pro) => pro.ZPROJECT.Contains(filterCondition.MDCode))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ForeignName), (pro) => pro.ZPROJENAME.Contains(filterCondition.ForeignName))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Type), (pro) => pro.ZPROJTYPE.Contains(filterCondition.Type))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Country), (pro) => pro.ZZCOUNTRY.Contains(filterCondition.Country))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Location), (pro) => pro.ZPROJLOC.Contains(filterCondition.Location))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BTypeOfCCCC), (pro) => pro.ZCPBC.Contains(filterCondition.BTypeOfCCCC))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Invest), (pro) => pro.ZINVERSTOR.Contains(filterCondition.Invest))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ResolutionNo), (pro) => pro.ZAPPROVAL.Contains(filterCondition.ResolutionNo))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ResolutionTime), (pro) => pro.ZAPVLDATE.Contains(filterCondition.ResolutionTime))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SourceOfIncome), (pro) => pro.ZSI.Contains(filterCondition.SourceOfIncome))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PjectOrg), (pro) => pro.ZPRO_ORG.Contains(filterCondition.PjectOrg))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Abbreviation), (pro) => pro.ZHEREINAFTER.Contains(filterCondition.Abbreviation))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SupMDCode), (pro) => pro.ZPROJECTUP.Contains(filterCondition.SupMDCode))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Year), (pro) => pro.ZPROJYEAR.Contains(filterCondition.Year))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PlanStartDate), (pro) => pro.ZSTARTDATE.Contains(filterCondition.PlanStartDate))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PlanCompletionDate), (pro) => pro.ZFINDATE.Contains(filterCondition.PlanCompletionDate))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EngineeringName), (pro) => pro.ZENG.Contains(filterCondition.EngineeringName))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ResponsibleParty), (pro) => pro.ZRESP.Contains(filterCondition.ResponsibleParty))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.LandTransactionNo), (pro) => pro.ZLDLOC.Contains(filterCondition.LandTransactionNo))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AcquisitionTime), (pro) => pro.ZLDLOCGT.Contains(filterCondition.AcquisitionTime))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BChangeTime), (pro) => pro.ZCBR.Contains(filterCondition.BChangeTime))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TradingSituation), (pro) => pro.ZTRADER.Contains(filterCondition.TradingSituation))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.NameOfInsureOrg), (pro) => pro.ZINSURANCE.Contains(filterCondition.NameOfInsureOrg))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PolicyNo), (pro) => pro.ZPOLICYNO.Contains(filterCondition.PolicyNo))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Applicant), (pro) => pro.ZINSURED.Contains(filterCondition.Applicant))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.StartDateOfInsure), (pro) => pro.ZISTARTDATE.Contains(filterCondition.StartDateOfInsure))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EndDateOfInsure), (pro) => pro.ZIFINDATE.Contains(filterCondition.EndDateOfInsure))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FundMDCode), (pro) => pro.ZFUND.Contains(filterCondition.FundMDCode))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FundName), (pro) => pro.ZFUNDNAME.Contains(filterCondition.FundName))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FundNo), (pro) => pro.ZFUNDNO.Contains(filterCondition.FundNo))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Currency), (pro) => pro.ZZCURRENCY.Contains(filterCondition.Currency))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FundOrgForm), (pro) => pro.ZFUNDORGFORM.Contains(filterCondition.FundOrgForm))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PjectOrgBP), (pro) => pro.ZPRO_BP.Contains(filterCondition.PjectOrgBP))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FundManager), (pro) => pro.ZFUNDMTYPE.Contains(filterCondition.FundManager))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FundEstablishmentDate), (pro) => pro.ZFSTARTDATE.Contains(filterCondition.FundEstablishmentDate))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FundExpirationDate), (pro) => pro.ZFFINDATE.Contains(filterCondition.FundExpirationDate))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CustodianName), (pro) => pro.ZCUSTODIAN.Contains(filterCondition.CustodianName))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TenantName), (pro) => pro.ZLESSEE.Contains(filterCondition.TenantName))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TenantType), (pro) => pro.ZLESSEETYPE.Contains(filterCondition.TenantType))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.NameOfLeased), (pro) => pro.ZLEASESNAME.Contains(filterCondition.NameOfLeased))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.LeaseStartDate), (pro) => pro.ZLSTARTDATE.Contains(filterCondition.LeaseStartDate))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.DueDate), (pro) => pro.ZLFINDATE.Contains(filterCondition.DueDate))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UnitSec), (pro) => pro.Z2NDORG.Contains(filterCondition.UnitSec))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.State), (pro) => pro.ZSTATE.Contains(filterCondition.State))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ReasonForDeactivate), (pro) => pro.ZSTOPREASON.Contains(filterCondition.ReasonForDeactivate))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TaxMethod), (pro) => pro.ZTAXMETHOD.Contains(filterCondition.TaxMethod))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgMethod), (pro) => pro.ZPOS.Contains(filterCondition.OrgMethod))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.WinningBidder), (pro) => pro.ZAWARDMAI.Contains(filterCondition.WinningBidder))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ConsolidatedTable), (pro) => pro.ZCS.Contains(filterCondition.ConsolidatedTable))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Management), (pro) => pro.ZMANAGE_MODE.Contains(filterCondition.Management))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BDep), (pro) => pro.ZBIZDEPT.Contains(filterCondition.BDep))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.IsJoint), (pro) => pro.ZWINNINGC.Contains(filterCondition.IsJoint))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
        //        .Where((pro) => pro.IsDelete == 1)
        //        .Select((pro) => new ProjectDetailsDto
        //        {
        //            Id = pro.Id.ToString(),
        //            Abbreviation = pro.ZHEREINAFTER,
        //            Country = pro.ZZCOUNTRY,
        //            Currency = pro.ZZCURRENCY,
        //            EngineeringName = pro.ZENG,
        //            ForeignName = pro.ZPROJENAME,
        //            Location = pro.ZPROJLOC,
        //            MDCode = pro.ZPROJECT,
        //            Name = pro.ZPROJNAME,
        //            PjectOrg = pro.ZPRO_ORG,
        //            PjectOrgBP = pro.ZPRO_BP,
        //            PlanCompletionDate = pro.ZFINDATE,
        //            PlanStartDate = pro.ZSTARTDATE,
        //            ResolutionTime = pro.ZAPVLDATE,
        //            ResolutionNo = pro.ZAPPROVAL,
        //            State = pro.ZSTATE == "1" ? "有效" : "无效",
        //            Type = pro.ZPROJTYPE,
        //            UnitSec = pro.Z2NDORG,
        //            Year = pro.ZPROJYEAR,
        //            AcquisitionTime = pro.ZLDLOCGT,
        //            Applicant = pro.ZINSURED,
        //            BChangeTime = pro.ZCBR,
        //            BDep = pro.ZBIZDEPT,
        //            BidDisclosureNo = pro.ZAWARDP,
        //            BTypeOfCCCC = pro.ZCPBC,
        //            ConsolidatedTable = pro.ZCS,
        //            CreateDate = pro.ZCREATE_AT,
        //            CustodianName = pro.ZCUSTODIAN,
        //            DueDate = pro.ZLFINDATE,
        //            EndDateOfInsure = pro.ZIFINDATE,
        //            FundEstablishmentDate = pro.ZFSTARTDATE,
        //            FundExpirationDate = pro.ZFFINDATE,
        //            FundManager = pro.ZFUNDMTYPE,
        //            FundMDCode = pro.ZFUND,
        //            FundName = pro.ZFUNDNAME,
        //            FundNo = pro.ZFUNDNO,
        //            FundOrgForm = pro.ZFUNDORGFORM,
        //            Invest = pro.ZINVERSTOR,
        //            IsJoint = pro.ZWINNINGC,
        //            LandTransactionNo = pro.ZLDLOC,
        //            LeaseStartDate = pro.ZLSTARTDATE,
        //            Management = pro.ZMANAGE_MODE,
        //            NameOfInsureOrg = pro.ZINSURANCE,
        //            NameOfLeased = pro.ZLEASESNAME,
        //            OrgMethod = pro.ZPOS,
        //            ParticipateInUnitSecs = pro.ZCY2NDORG,
        //            PolicyNo = pro.ZPOLICYNO,
        //            ReasonForDeactivate = pro.ZSTOPREASON,
        //            ResponsibleParty = pro.ZRESP,
        //            SourceOfIncome = pro.ZSI,
        //            StartDateOfInsure = pro.ZISTARTDATE,
        //            SupMDCode = pro.ZPROJECTUP,
        //            TaxMethod = pro.ZTAXMETHOD,
        //            TenantName = pro.ZLESSEE,
        //            TenantType = pro.ZLESSEETYPE,
        //            TradingSituation = pro.ZTRADER,
        //            WinningBidder = pro.ZAWARDMAI,
        //            CreateTime = pro.CreateTime,
        //            UpdateTime = pro.UpdateTime
        //        })
        //        .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);
        //    #endregion

        //    #region 详细查询
        //    //值域信息
        //    var valDomain = await _dbContext.Queryable<ValueDomain>()
        //        .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
        //        .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
        //        .ToListAsync();

        //    //国家地区
        //    var countrysKey = proList.Select(x => x.Country).ToList();
        //    var country = await _dbContext.Queryable<CountryRegion>()
        //        .Where(t => t.IsDelete == 1 && countrysKey.Contains(t.ZCOUNTRYCODE))
        //        .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZCOUNTRYCODE, Name = t.ZCOUNTRYNAME })
        //        .ToListAsync();

        //    //行政区划
        //    var locationKey = proList.Select(x => x.Location).ToList();
        //    var location = await _dbContext.Queryable<AdministrativeDivision>()
        //        .Where(t => t.IsDelete == 1 && locationKey.Contains(t.ZADDVSCODE))
        //        .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZADDVSCODE, Name = t.ZADDVSNAME })
        //        .ToListAsync();

        //    //项目机构
        //    var pjectOrgKey = proList.Select(x => x.PjectOrg).ToList();
        //    var pjectOrg2Key = proList.Select(x => x.PjectOrgBP).ToList();
        //    var pjectOrg = await _dbContext.Queryable<Institution>()
        //        .Where(t => t.IsDelete == 1 && (pjectOrgKey.Contains(t.OID) || pjectOrg2Key.Contains(t.OID)))
        //        .Select(t => new InstutionRespDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
        //        .ToListAsync();

        //    foreach (var item in proList)
        //    {
        //        //项目类型
        //        item.Type = GetValueDomain(item.Type, valDomain, "ZPROJTYPE");

        //        //国家地区
        //        item.Country = GetCountryRegion(item.Country, country);

        //        //项目所在地
        //        item.Location = GetAdministrativeDivision(item.Location, location);

        //        //项目机构
        //        item.PjectOrg = GetInstitutionName(item.PjectOrg, pjectOrg);
        //        item.PjectOrgBP = GetInstitutionName(item.PjectOrgBP, pjectOrg);

        //        //日期
        //        item.PlanStartDate = GetDate(item.PlanStartDate);
        //        item.PlanCompletionDate = GetDate(item.PlanCompletionDate);
        //        item.AcquisitionTime = GetDate(item.AcquisitionTime);
        //        item.BChangeTime = GetDate(item.BChangeTime);
        //        item.StartDateOfInsure = GetDate(item.StartDateOfInsure);
        //        item.EndDateOfInsure = GetDate(item.EndDateOfInsure);
        //        item.FundEstablishmentDate = GetDate(item.FundEstablishmentDate);
        //        item.FundExpirationDate = GetDate(item.FundExpirationDate);
        //        item.LeaseStartDate = GetDate(item.LeaseStartDate);
        //        item.DueDate = GetDate(item.DueDate);
        //        item.CreateDate = GetDateDay(item.CreateDate);
        //        item.ResolutionTime = GetDate(item.ResolutionTime);

        //        //收入来源
        //        item.SourceOfIncome = GetValueDomain(item.SourceOfIncome, valDomain, "ZSI");

        //        //操盘情况
        //        item.TradingSituation = GetValueDomain(item.TradingSituation, valDomain, "ZTRADER");

        //        //承租人类型
        //        item.TenantType = GetValueDomain(item.TenantType, valDomain, "ZLESSEETYPE");

        //        //参与二级单位
        //        item.ParticipateInUnitSecs = GetValueDomain(item.ParticipateInUnitSecs, valDomain, "ZCY2NDORG");

        //        //计税方式
        //        item.TaxMethod = GetValueDomain(item.TaxMethod, valDomain, "ZTAXMETHOD");

        //        //并表情况
        //        item.ConsolidatedTable = GetValueDomain(item.ConsolidatedTable, valDomain, "ZCS");

        //        //是否联合体项目
        //        item.IsJoint = item.IsJoint == "1" ? "是" : "否";



        //    }
        //    #endregion

        //    responseAjaxResult.Count = total;
        //    responseAjaxResult.SuccessResult(proList);
        //    return responseAjaxResult;
        //}
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
                    WinningBidder = pro.ZAWARDMAI,
                    CreateTime = pro.CreateTime,
                    UpdateTime = pro.UpdateTime
                })
                .FirstAsync();

            #region 其他详情
            //值域信息
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();

            //国家地区
            var country = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1 && result.Country == t.ZCOUNTRYCODE)
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZCOUNTRYCODE, Name = t.ZCOUNTRYNAME })
                .ToListAsync();
            result.Country = country.Count == 0 ? null : country.FirstOrDefault()?.Name;

            //行政区划
            var location = await _dbContext.Queryable<AdministrativeDivision>()
                .WhereIF(!string.IsNullOrWhiteSpace(result.Location), t => t.ZADDVSCODE == result.Location)
                .Where(t => t.IsDelete == 1)
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZADDVSCODE, Name = t.ZADDVSNAME })
                .ToListAsync();
            result.Location = location.Count == 0 ? null : location.FirstOrDefault()?.Name;

            //项目机构
            var pjectOrg = await _dbContext.Queryable<Institution>()
                .WhereIF(!string.IsNullOrWhiteSpace(result.PjectOrg), t => t.OID == result.PjectOrg)
                .Where(t => t.IsDelete == 1)
                .Select(t => new InstutionRespDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
                .ToListAsync();

            var pjectOrgBp = await _dbContext.Queryable<Institution>()
                .WhereIF(!string.IsNullOrWhiteSpace(result.PjectOrgBP), t => t.OID == result.PjectOrgBP)
             .Where(t => t.IsDelete == 1)
             .Select(t => new InstutionRespDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
             .ToListAsync();
            //项目类型
            result.Type = GetValueDomain(result.Type, valDomain, "ZPROJTYPE");

            //国家地区
            result.Country = GetCountryRegion(result.Country, country);

            //项目所在地
            result.Location = GetAdministrativeDivision(result.Location, location);

            //项目机构
            result.PjectOrg = GetInstitutionName(result.PjectOrg, pjectOrg);
            result.PjectOrgBP = GetInstitutionName(result.PjectOrgBP, pjectOrgBp);

            //日期
            result.PlanStartDate = GetDate(result.PlanStartDate);
            result.PlanCompletionDate = GetDate(result.PlanCompletionDate);
            result.AcquisitionTime = GetDate(result.AcquisitionTime);
            result.BChangeTime = GetDate(result.BChangeTime);
            result.StartDateOfInsure = GetDate(result.StartDateOfInsure);
            result.EndDateOfInsure = GetDate(result.EndDateOfInsure);
            result.FundEstablishmentDate = GetDate(result.FundEstablishmentDate);
            result.FundExpirationDate = GetDate(result.FundExpirationDate);
            result.LeaseStartDate = GetDate(result.LeaseStartDate);
            result.DueDate = GetDate(result.DueDate);
            result.CreateDate = GetDateDay(result.CreateDate);
            result.ResolutionTime = GetDate(result.ResolutionTime);

            //收入来源
            result.SourceOfIncome = GetValueDomain(result.SourceOfIncome, valDomain, "ZSI");

            //操盘情况
            result.TradingSituation = GetValueDomain(result.TradingSituation, valDomain, "ZTRADER");

            //承租人类型
            result.TenantType = GetValueDomain(result.TenantType, valDomain, "ZLESSEETYPE");

            //参与二级单位
            result.ParticipateInUnitSecs = GetValueDomain(result.ParticipateInUnitSecs, valDomain, "ZCY2NDORG");

            //计税方式
            result.TaxMethod = GetValueDomain(result.TaxMethod, valDomain, "ZTAXMETHOD");

            //并表情况
            result.ConsolidatedTable = GetValueDomain(result.ConsolidatedTable, valDomain, "ZCS");

            //是否联合体项目
            result.IsJoint = result.IsJoint == "1" ? "是" : "否";

            #endregion


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

            //过滤条件
            CorresUnitDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<CorresUnitDetailsDto>(requestDto.FilterConditionJson);
            }

            var corresUnitList = await _dbContext.Queryable<CorresUnit>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.DealUnitMDCode), (pro) => pro.ZBP.Contains(filterCondition.DealUnitMDCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.IsGroupUnit), (pro) => pro.ZINCLIENT.Contains(filterCondition.IsGroupUnit))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccUnitCode), (pro) => pro.ZACORGNO.Contains(filterCondition.AccUnitCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgMDCode), (pro) => pro.ZORG.Contains(filterCondition.OrgMDCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZBPNAME_ZH.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.NameEnglish), (pro) => pro.ZBPNAME_EN.Contains(filterCondition.NameEnglish))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.NameInLLanguage), (pro) => pro.ZBPNAME_LOC.Contains(filterCondition.NameInLLanguage))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CategoryUnit), (pro) => pro.ZBPTYPE.Contains(filterCondition.CategoryUnit))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.RegistrationNo), (pro) => pro.ZUSCC.Contains(filterCondition.RegistrationNo))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgCode), (pro) => pro.ZOIBC.Contains(filterCondition.OrgCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BRegistrationNo), (pro) => pro.ZBRNO.Contains(filterCondition.BRegistrationNo))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TaxpayerIdentifyNo), (pro) => pro.ZTRNO.Contains(filterCondition.TaxpayerIdentifyNo))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AbroadRegistrationNo), (pro) => pro.ZOSRNO.Contains(filterCondition.AbroadRegistrationNo))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.IdNo), (pro) => pro.ZIDNO.Contains(filterCondition.IdNo))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AbroadSocialSecurityNo), (pro) => pro.ZSSNO.Contains(filterCondition.AbroadSocialSecurityNo))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Country), (pro) => pro.ZZCOUNTRY.Contains(filterCondition.Country))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Province), (pro) => pro.ZPROVINCE.Contains(filterCondition.Province))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.City), (pro) => pro.ZCITY.Contains(filterCondition.City))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.County), (pro) => pro.ZCOUNTY.Contains(filterCondition.County))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EnterpriseNature), (pro) => pro.ZETPSPROPERTY.Contains(filterCondition.EnterpriseNature))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SupLegalEntity), (pro) => pro.ZCOMPYREL.Contains(filterCondition.SupLegalEntity))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.NatureOfUnit), (pro) => pro.ZBPNATURE.Contains(filterCondition.NatureOfUnit))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TypeOfUnit), (pro) => pro.ZBPKINDS.Contains(filterCondition.TypeOfUnit))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    StatusOfUnit = cu.ZBPSTATE == "01" ? "有效" : "无效",
                    CreateTime = cu.CreateTime,
                    UpdateTime = cu.UpdateTime
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
                    StatusOfUnit = cu.ZBPSTATE == "01" ? "有效" : "无效",
                    CreateTime = cu.CreateTime,
                    UpdateTime = cu.UpdateTime
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

            //过滤条件
            CountryRegionDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<CountryRegionDetailsDto>(requestDto.FilterConditionJson);
            }
            var corresUnitList = await _dbContext.Queryable<CountryRegion>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Country), (pro) => pro.ZCOUNTRYCODE.Contains(filterCondition.Country))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZCOUNTRYNAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.NameEnglish), (pro) => pro.ZCOUNTRYENAME.Contains(filterCondition.NameEnglish))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.NationalCode), (pro) => pro.ZGBCHAR.Contains(filterCondition.NationalCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.DigitCode), (pro) => pro.ZGBNUM.Contains(filterCondition.DigitCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ContinentCode), (pro) => pro.ZCONTINENTCODE.Contains(filterCondition.ContinentCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CCCCCenterCode), (pro) => pro.ZCRCCODE.Contains(filterCondition.CCCCCenterCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    Version = cr.ZVERSION,
                    CreateTime = cr.CreateTime,
                    UpdateTime = cr.UpdateTime
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
                    Version = cr.ZVERSION,
                    CreateTime = cr.CreateTime,
                    UpdateTime = cr.UpdateTime
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

            //过滤条件
            CountryContinentDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<CountryContinentDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<CountryContinent>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ContinentCode), (pro) => pro.ZCONTINENTCODE.Contains(filterCondition.ContinentCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZCONTINENTNAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AreaCode), (pro) => pro.ZAREACODE.Contains(filterCondition.AreaCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.RegionalDescr), (pro) => pro.ZAREANAME.Contains(filterCondition.RegionalDescr))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            FinancialInstitutionDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<FinancialInstitutionDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<FinancialInstitution>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.City), (pro) => pro.ZCITY.Contains(filterCondition.City))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Country), (pro) => pro.ZZCOUNTRY.Contains(filterCondition.Country))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.County), (pro) => pro.ZCOUNTY.Contains(filterCondition.County))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EnglishName), (pro) => pro.ZFINAME_E.Contains(filterCondition.EnglishName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZBANKNAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.NameOfOrg), (pro) => pro.ZFINAME.Contains(filterCondition.NameOfOrg))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Province), (pro) => pro.ZPROVINCE.Contains(filterCondition.Province))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TypesOfAbroadOrg), (pro) => pro.ZOFITYPE.Contains(filterCondition.TypesOfAbroadOrg))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TypesOfOrg), (pro) => pro.ZDFITYPE.Contains(filterCondition.TypesOfOrg))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BankNo), (pro) => pro.ZBANKN.Contains(filterCondition.BankNo))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.MDCode), (pro) => pro.ZFINC.Contains(filterCondition.MDCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.MDCodeofOrg), (pro) => pro.ZORG.Contains(filterCondition.MDCodeofOrg))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.RegistrationNo), (pro) => pro.ZUSCC.Contains(filterCondition.RegistrationNo))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.No), (pro) => pro.ZBANK.Contains(filterCondition.No))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SwiftCode), (pro) => pro.ZSWIFTCOD.Contains(filterCondition.SwiftCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new FinancialInstitutionDetailsDto
                {
                    Id = cc.Id.ToString(),
                    City = cc.ZCITY,
                    Country = cc.ZZCOUNTRY,
                    County = cc.ZCOUNTY,
                    TypesOfAbroadOrg = cc.ZOFITYPE,
                    EnglishName = cc.ZFINAME_E,
                    Name = cc.ZBANKNAME,
                    NameOfOrg = cc.ZFINAME,
                    Province = cc.ZPROVINCE,
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
                    UnitSec = cc.ZFIN2NDORG,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
                })
                .OrderByDescending((cc) => cc.Name)
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            //国家地区
            var countrysKey = ccList.Select(x => x.Country).ToList();
            var country = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1 && countrysKey.Contains(t.ZCOUNTRYCODE))
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZCOUNTRYCODE, Name = t.ZCOUNTRYNAME })
                .ToListAsync();

            //省、市、县
            var p = ccList.Select(x => x.Province).ToList();
            var city = ccList.Select(x => x.City).ToList();
            var c = ccList.Select(x => x.County).ToList();
            var adisision = await _dbContext.Queryable<AdministrativeDivision>()
                .Where(t => t.IsDelete == 1 && (p.Contains(t.ZADDVSCODE) || city.Contains(t.ZADDVSCODE) || c.Contains(t.ZADDVSCODE)))
                .Select(t => new FilterChildData { Key = t.ZADDVSCODE, Val = t.ZADDVSNAME, Code = t.ZADDVSLEVEL })
                .ToListAsync();


            //值域信息
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();

            foreach (var item in ccList)
            {
                //国家地区
                item.Country = GetCountryRegion(item.Country, country);

                //省、市、县
                item.Province = adisision.FirstOrDefault(x => x.Key == item.Province)?.Val;
                item.City = adisision.FirstOrDefault(x => x.Key == item.City)?.Val;
                item.County = adisision.FirstOrDefault(x => x.Key == item.County)?.Val;

                //境内金融机构类型
                item.TypesOfOrg = GetValueDomain(item.TypesOfOrg, valDomain, "ZDFITYPE");

                //境外金融机构类型
                item.TypesOfAbroadOrg = GetValueDomain(item.TypesOfAbroadOrg, valDomain, "ZOFITYPE");
            }

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
                    UnitSec = cc.ZFIN2NDORG,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            DeviceClassCodeDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<DeviceClassCodeDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<DeviceClassCode>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AliasName), (pro) => pro.ZCALIAS.Contains(filterCondition.AliasName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Code), (pro) => pro.ZCLASS.Contains(filterCondition.Code))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Description), (pro) => pro.ZCDESC.Contains(filterCondition.Description))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZCNAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UnitOfMeasurement), (pro) => pro.ZMSEHI.Contains(filterCondition.UnitOfMeasurement))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SortRule), (pro) => pro.ZSORT.Contains(filterCondition.SortRule))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SupCode), (pro) => pro.ZCLASSUP.Contains(filterCondition.SupCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    SupCode = cc.ZCLASSUP,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    SupCode = cc.ZCLASSUP,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            InvoiceTypeDetailshDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<InvoiceTypeDetailshDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<InvoiceType>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Code), (pro) => pro.ZINVTCODE.Contains(filterCondition.Code))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZINVTNAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new InvoiceTypeDetailshDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZINVTCODE,
                    Name = cc.ZINVTNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE,
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
        public async Task<ResponseAjaxResult<List<DHResearchDto>>> GetScientifiCNoProjectSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DHResearchDto>>();
            RefAsync<int> total = 0;

            //过滤条件
            DHResearchDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<DHResearchDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<DHResearch>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzdelete), (pro) => pro.Fzdelete.Contains(filterCondition.Fzdelete.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzversion), (pro) => pro.Fzversion.Contains(filterCondition.Fzversion))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzhitech), (pro) => pro.Fzhitech.Contains(filterCondition.Fzhitech))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzstate), (pro) => pro.Fzstate.Contains(filterCondition.Fzstate))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FzitOname), (pro) => pro.FzitOname.Contains(filterCondition.FzitOname))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzkpstate), (pro) => pro.Fzkpstate.Contains(filterCondition.Fzkpstate))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzmajortype), (pro) => pro.Fzmajortype.Contains(filterCondition.Fzmajortype))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzoutsourcing), (pro) => pro.Fzoutsourcing.Contains(filterCondition.Fzoutsourcing))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzpfindate.ToString()), (pro) => pro.Fzpfindate.ToString().Contains(filterCondition.Fzpfindate.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzprojcost.ToString()), (pro) => pro.Fzprojcost.ToString().Contains(filterCondition.Fzprojcost.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzprojyear.ToString()), (pro) => pro.Fzprojyear.ToString().Contains(filterCondition.Fzprojyear.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzpstartdate.ToString()), (pro) => pro.Fzpstartdate.ToString().Contains(filterCondition.Fzpstartdate.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzsrpclass), (pro) => pro.Fzsrpclass.Contains(filterCondition.Fzsrpclass))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FzsrpCode), (pro) => pro.FzsrpCode.Contains(filterCondition.FzsrpCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzsrpn), (pro) => pro.Fzsrpn.Contains(filterCondition.Fzsrpn))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FzsrpnFn), (pro) => pro.FzsrpnFn.Contains(filterCondition.FzsrpnFn))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FzsrpupCode), (pro) => pro.FzsrpupCode.Contains(filterCondition.FzsrpupCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdatedAt.ToString()), (pro) => pro.UpdatedAt.ToString().Contains(filterCondition.UpdatedAt.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreatedAt.ToString()), (pro) => pro.CreatedAt.ToString().Contains(filterCondition.CreatedAt.ToString()))
                //.WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                //.WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new DHResearchDto
                {
                    CreatedAt = cc.CreatedAt,
                    Fzdelete = cc.Fzdelete,
                    Fzversion = cc.Fzversion,
                    Fzhitech = cc.Fzhitech,
                    Fzstate = cc.Fzstate,
                    FzitAg = cc.FzitAg,
                    FzitAh = cc.FzitAh,
                    FzitAi = cc.FzitAi,
                    FzitAj = cc.FzitAj,
                    FzitAk = cc.FzitAk,
                    FzitDe = cc.FzitDe,
                    FzitOname = cc.FzitOname,
                    Fzkpstate = cc.Fzkpstate,
                    Fzmajortype = cc.Fzmajortype,
                    Fzoutsourcing = cc.Fzoutsourcing,
                    Fzpfindate = cc.Fzpfindate,
                    Fzprojcost = cc.Fzprojcost,
                    Fzprojcostcur = cc.Fzprojcostcur,
                    Fzprojyear = cc.Fzprojyear,
                    Fzpstartdate = cc.Fzpstartdate,
                    Fzsrpclass = cc.Fzsrpclass,
                    FzsrpCode = cc.FzsrpCode,
                    Fzsrpn = cc.Fzsrpn,
                    FzsrpnFn = cc.FzsrpnFn,
                    FzsrpupCode = cc.FzsrpupCode,
                    UpdatedAt = cc.UpdatedAt,
                })
                .OrderByDescending(x => x.UpdatedAt)
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            #region 

            //值域信息
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();

            //币种
            var cIds = ccList.Select(t => t.Fzprojcostcur).ToList();
            var currency = await _dbContext.Queryable<Currency>()
                .Where(t => t.IsDelete == 1 && cIds.Contains(t.ZCURRENCYCODE))
                .Select(t => new { t.ZCURRENCYCODE, t.ZCURRENCYNAME })
                .ToListAsync();

            #endregion

            foreach (var item in ccList)
            {
                #region 子集
                if (!string.IsNullOrWhiteSpace(item.FzitAi))
                {
                    var jsonObject = JObject.Parse(item.FzitAi);
                    if (!string.IsNullOrWhiteSpace(jsonObject["item"].ToString()))
                    {
                        item.FzitAiList = JsonConvert.DeserializeObject<List<FzitAi>>(jsonObject["item"].ToString());
                    }
                }
                if (!string.IsNullOrWhiteSpace(item.FzitOname))
                {
                    var jsonObject = JObject.Parse(item.FzitOname);
                    if (!string.IsNullOrWhiteSpace(jsonObject["item"].ToString()))
                    {
                        item.FzitOnameList = JsonConvert.DeserializeObject<List<FzitOname>>(jsonObject["item"].ToString());
                    }
                }
                if (!string.IsNullOrWhiteSpace(item.FzitAg))
                {
                    var jsonObject = JObject.Parse(item.FzitAg);
                    if (!string.IsNullOrWhiteSpace(jsonObject["item"].ToString()))
                    {
                        item.FzitAgList = JsonConvert.DeserializeObject<List<FzitAg>>(jsonObject["item"].ToString());
                    }
                }
                if (!string.IsNullOrWhiteSpace(item.FzitAk))
                {
                    var jsonObject = JObject.Parse(item.FzitAk);
                    if (!string.IsNullOrWhiteSpace(jsonObject["item"].ToString()))
                    {
                        item.FzitAkList = JsonConvert.DeserializeObject<List<FzitAk>>(jsonObject["item"].ToString());
                    }
                }
                if (!string.IsNullOrWhiteSpace(item.FzitAh))
                {
                    var jsonObject = JObject.Parse(item.FzitAh);
                    if (!string.IsNullOrWhiteSpace(jsonObject["item"].ToString()))
                    {
                        item.FzitAhList = JsonConvert.DeserializeObject<List<FzitAh>>(jsonObject["item"].ToString());
                    }
                }
                if (!string.IsNullOrWhiteSpace(item.FzitDe))
                {
                    var jsonObject = JObject.Parse(item.FzitDe);
                    if (!string.IsNullOrWhiteSpace(jsonObject["item"].ToString()))
                    {
                        item.FzitDeList = JsonConvert.DeserializeObject<List<FzitDe>>(jsonObject["item"].ToString());
                    }
                }
                if (!string.IsNullOrWhiteSpace(item.FzitAj))
                {
                    var jsonObject = JObject.Parse(item.FzitAj);
                    if (!string.IsNullOrWhiteSpace(jsonObject["item"].ToString()))
                    {
                        item.FzitAjList = JsonConvert.DeserializeObject<List<FzitAj>>(jsonObject["item"].ToString());
                    }
                }
                #endregion

                //项目状态
                item.Fzkpstate = valDomain.FirstOrDefault(x => x.ZDOM_CODE == "ZKPSTATE" && x.ZDOM_VALUE == item.Fzkpstate)?.ZDOM_NAME;

                //科研项目分类
                item.Fzsrpclass = valDomain.FirstOrDefault(x => x.ZDOM_CODE == "ZPROJTYPE" && x.ZDOM_VALUE == item.Fzsrpclass)?.ZDOM_NAME;

                //专业类型
                item.Fzmajortype = valDomain.FirstOrDefault(x => x.ZDOM_CODE == "ZMAJORTYPE" && x.ZDOM_VALUE == item.Fzmajortype)?.ZDOM_NAME;

                //币种
                item.Fzprojcostcur = currency.FirstOrDefault(x => x.ZCURRENCYCODE == item.Fzprojcostcur)?.ZCURRENCYNAME;

                ////日期
                //item.Fzpstartdate = item.Fzpstartdate.Value.Date;
                //item.Fzpfindate = item.Fzpfindate.Value.Date;

            }

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        //public async Task<ResponseAjaxResult<List<ScientifiCNoProjectDetailsDto>>> GetScientifiCNoProjectSearchAsync(FilterCondition requestDto)
        //{
        //    var responseAjaxResult = new ResponseAjaxResult<List<ScientifiCNoProjectDetailsDto>>();
        //    RefAsync<int> total = 0;

        //    //过滤条件
        //    ScientifiCNoProjectDetailsDto filterCondition = new();
        //    if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
        //    {
        //        filterCondition = JsonConvert.DeserializeObject<ScientifiCNoProjectDetailsDto>(requestDto.FilterConditionJson);
        //    }

        //    var ccList = await _dbContext.Queryable<ScientifiCNoProject>()
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZSRPN.Contains(filterCondition.Name))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CurrencyOfCost), (pro) => pro.ZPROJCOSTCUR.Contains(filterCondition.CurrencyOfCost))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ForeignName), (pro) => pro.ZSRPN_FN.Contains(filterCondition.ForeignName))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.MDCode), (pro) => pro.ZSRP.Contains(filterCondition.MDCode))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PjectState), (pro) => pro.ZKPSTATE.Contains(filterCondition.PjectState))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TotalCost), (pro) => pro.ZPROJCOST.Contains(filterCondition.TotalCost))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Year), (pro) => pro.ZPROJYEA.Contains(filterCondition.Year))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PlanEndDate), (pro) => pro.ZPFINDATE.Contains(filterCondition.PlanEndDate))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PlanStartDate), (pro) => pro.ZPSTARTDATE.Contains(filterCondition.PlanStartDate))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ProfessionalType), (pro) => pro.ZMAJORTYPE.Contains(filterCondition.ProfessionalType))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SupMDCode), (pro) => pro.ZSRPUP.Contains(filterCondition.SupMDCode))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ProfessionalType), (pro) => pro.ZSRPCLASS.Contains(filterCondition.ProfessionalType))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
        //        .Where((cc) => cc.IsDelete == 1)
        //        .Select((cc) => new ScientifiCNoProjectDetailsDto
        //        {
        //            Id = cc.Id.ToString(),
        //            Name = cc.ZSRPN,
        //            CurrencyOfCost = cc.ZPROJCOSTCUR,
        //            ForeignName = cc.ZSRPN_FN,
        //            IsHighTech = cc.ZHITECH == "1" ? "是" : "否",
        //            MDCode = cc.ZSRP,
        //            PjectState = cc.ZKPSTATE,
        //            TotalCost = cc.ZPROJCOST,
        //            Year = cc.ZPROJYEA,
        //            IsOutsourced = cc.ZOUTSOURCING == "1" ? "是" : "否",
        //            PlanEndDate = cc.ZPFINDATE,
        //            PlanStartDate = cc.ZPSTARTDATE,
        //            ProfessionalType = cc.ZMAJORTYPE,
        //            State = cc.ZPSTATE,
        //            SupMDCode = cc.ZSRPUP,
        //            TypeCode = cc.ZSRPCLASS,
        //            CreateTime = cc.CreateTime,
        //            UpdateTime = cc.UpdateTime
        //        })
        //        .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

        //    responseAjaxResult.Count = total;
        //    responseAjaxResult.SuccessResult(ccList);
        //    return responseAjaxResult;
        //}
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
                    TypeCode = cc.ZSRPCLASS,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            LanguageDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<LanguageDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<Language>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZLANG_ZH.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.DirCode), (pro) => pro.ZLANG_BIB.Contains(filterCondition.DirCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.EnglishName), (pro) => pro.ZLANG_EN.Contains(filterCondition.EnglishName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TermCode), (pro) => pro.ZLANG_TER.Contains(filterCondition.TermCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            BankCardDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<BankCardDetailsDto>(requestDto.FilterConditionJson);
            }
            var ccList = await _dbContext.Queryable<BankCard>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZKOINH.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccountCurrency), (pro) => pro.ZCURR.Contains(filterCondition.AccountCurrency))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccountStatus), (pro) => pro.ZBANKSTA.Contains(filterCondition.AccountStatus))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BankAccount), (pro) => pro.ZBANKN.Contains(filterCondition.BankAccount))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BankNoPK), (pro) => pro.ZBANK.Contains(filterCondition.BankNoPK))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.City), (pro) => pro.ZCITY2.Contains(filterCondition.City))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Country), (pro) => pro.ZZCOUNTR2.Contains(filterCondition.Country))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.County), (pro) => pro.ZCOUNTY2.Contains(filterCondition.County))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.DealUnitCode), (pro) => pro.ZBP.Contains(filterCondition.DealUnitCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FinancialOrgCode), (pro) => pro.ZFINC.Contains(filterCondition.FinancialOrgCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.FinancialOrgName), (pro) => pro.ZFINAME.Contains(filterCondition.FinancialOrgName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Province), (pro) => pro.ZPROVINC2.Contains(filterCondition.Province))
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

            //过滤条件
            DeviceDetailCodeDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<DeviceDetailCodeDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<DeviceDetailCode>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZMNAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Descption), (pro) => pro.ZMNAMES.Contains(filterCondition.Descption))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.MDCode), (pro) => pro.ZMATERIAL.Contains(filterCondition.MDCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ProductNameCode), (pro) => pro.ZCLASS.Contains(filterCondition.ProductNameCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Remark), (pro) => pro.ZREMARK.Contains(filterCondition.Remark))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
        //public async Task<ResponseAjaxResult<List<AccountingDepartmentDetailsDto>>> GetAccountingDepartmentSearchAsync(FilterCondition requestDto)
        //{
        //    var responseAjaxResult = new ResponseAjaxResult<List<AccountingDepartmentDetailsDto>>();
        //    RefAsync<int> total = 0;

        //    //过滤条件
        //    AccountingDepartmentDetailsDto filterCondition = new();
        //    if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
        //    {
        //        filterCondition = JsonConvert.DeserializeObject<AccountingDepartmentDetailsDto>(requestDto.FilterConditionJson);
        //    }

        //    var ccList = await _dbContext.Queryable<AccountingDepartment>()
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZDNAME_CHS.Contains(filterCondition.Name))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccDepCode), (pro) => pro.ZDCODE.Contains(filterCondition.AccDepCode))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccDepELName), (pro) => pro.ZDNAME_EN.Contains(filterCondition.AccDepELName))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccDepTCCName), (pro) => pro.ZDNAME_CHT.Contains(filterCondition.AccDepTCCName))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccDepId), (pro) => pro.ZDID.Contains(filterCondition.AccDepId))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccOrgId), (pro) => pro.ZACID.Contains(filterCondition.AccOrgId))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccOrgCode), (pro) => pro.ZACORGNO.Contains(filterCondition.AccOrgCode))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SupAccDepId), (pro) => pro.ZDPARENTID.Contains(filterCondition.SupAccDepId))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
        //        .Where((cc) => cc.IsDelete == 1)
        //        .Select((cc) => new AccountingDepartmentDetailsDto
        //        {
        //            Id = cc.Id.ToString(),
        //            Name = cc.ZDNAME_CHS,
        //            AccDepCode = cc.ZDCODE,
        //            AccDepELName = cc.ZDNAME_EN,
        //            AccDepTCCName = cc.ZDNAME_CHT,
        //            State = cc.ZDATSTATE == "1" ? "无效" : "未无效",
        //            AccDepId = cc.ZDID,
        //            AccOrgCode = cc.ZACORGNO,
        //            AccOrgId = cc.ZACID,
        //            DataIdentifier = cc.ZDELETE == "1" ? "删除" : "正常",
        //            SupAccDepId = cc.ZDPARENTID,
        //            CreateTime = cc.CreateTime,
        //            UpdateTime = cc.UpdateTime
        //        })
        //        .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

        //    responseAjaxResult.Count = total;
        //    responseAjaxResult.SuccessResult(ccList);
        //    return responseAjaxResult;
        //}
        public async Task<ResponseAjaxResult<List<DHAccountingDept>>> GetAccountingDepartmentSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DHAccountingDept>>();
            RefAsync<int> total = 0;

            //过滤条件
            DHAccountingDept filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<DHAccountingDept>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<DHAccountingDept>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zacid), (pro) => pro.Zacid.Contains(filterCondition.Zacid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zacorgno), (pro) => pro.Zacorgno.Contains(filterCondition.Zacorgno))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zdatstate), (pro) => pro.Zdatstate.Contains(filterCondition.Zdatstate))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zdcode), (pro) => pro.Zdcode.Contains(filterCondition.Zdcode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zdelete), (pro) => pro.Zdelete.Contains(filterCondition.Zdelete))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zdid), (pro) => pro.Zdid.Contains(filterCondition.Zdid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZdnameChs), (pro) => pro.ZdnameChs.Contains(filterCondition.ZdnameChs))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZdnameCht), (pro) => pro.ZdnameCht.Contains(filterCondition.ZdnameCht))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZdnameEn), (pro) => pro.ZdnameEn.Contains(filterCondition.ZdnameEn))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zdparentid), (pro) => pro.Zdparentid.Contains(filterCondition.Zdparentid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new DHAccountingDept
                {
                    Zacid = cc.Zacid,
                    Zacorgno = cc.Zacorgno,
                    Zdatstate = cc.Zdatstate,
                    Zdcode = cc.Zdcode,
                    Zdelete = cc.IsDelete.ToString(),
                    IsDelete = cc.IsDelete,
                    Zdid = cc.Zdid,
                    ZdnameChs = cc.ZdnameChs,
                    ZdnameCht = cc.ZdnameCht,
                    ZdnameEn = cc.ZdnameEn,
                    Zdparentid = cc.Zdparentid,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    State = cc.ZDATSTATE == "1" ? "无效" : "未无效",
                    AccDepId = cc.ZDID,
                    AccOrgCode = cc.ZACORGNO,
                    AccOrgId = cc.ZACID,
                    DataIdentifier = cc.ZDELETE == "1" ? "删除" : "正常",
                    SupAccDepId = cc.ZDPARENTID,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            RelationalContractsDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<RelationalContractsDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<RelationalContracts>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Code), (pro) => pro.ZDELEGATE_ORG.Contains(filterCondition.Code))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.DetailedLine), (pro) => pro.ZNUMC4.Contains(filterCondition.DetailedLine))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.MDCode), (pro) => pro.MDM_CODE.Contains(filterCondition.MDCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgCode), (pro) => pro.OID.Contains(filterCondition.OrgCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccOrgCode), (pro) => pro.ZACO.Contains(filterCondition.AccOrgCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TreeCode), (pro) => pro.ZTREEID.Contains(filterCondition.TreeCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    ViewIdentification = cc.ZMVIEW_FLAG,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    ViewIdentification = cc.ZMVIEW_FLAG,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            RegionalDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<RegionalDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<Regional>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Code), (pro) => pro.ZCRHCODE.Contains(filterCondition.Code))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AreaRange), (pro) => pro.ZCRHSCOPE.Contains(filterCondition.AreaRange))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Description), (pro) => pro.ZCRHNAME.Contains(filterCondition.Description))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZCRHABBR.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            UnitMeasurementDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<UnitMeasurementDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<UnitMeasurement>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Code), (pro) => pro.ZUNITCODE.Contains(filterCondition.Code))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZUNITNAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new UnitMeasurementDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZUNITCODE,
                    Name = cc.ZUNITNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    Version = cc.ZVERSION,
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            ProjectClassificationDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<ProjectClassificationDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<ProjectClassification>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BSectorSecName), (pro) => pro.ZBUSTD2NAME.Contains(filterCondition.BSectorSecName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BSectorRemark), (pro) => pro.ZBUSTDREMARKS.Contains(filterCondition.BSectorRemark))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BSectorOneName), (pro) => pro.ZBUSTD1NAME.Contains(filterCondition.BSectorOneName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BSectorThirdName), (pro) => pro.ZBUSTD3NAME.Contains(filterCondition.BSectorThirdName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BusinessRemark), (pro) => pro.ZZCPBREMARKS.Contains(filterCondition.BusinessRemark))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CCCCBTypeName), (pro) => pro.Z12TOPBNAME.Contains(filterCondition.CCCCBTypeName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CCCCBTypeSecName), (pro) => pro.ZCPBC2NAME.Contains(filterCondition.CCCCBTypeSecName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CCCCBTypeThirdName), (pro) => pro.ZCPBC3NAME.Contains(filterCondition.CCCCBTypeThirdName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CCCCRiverLakeAndSeaName), (pro) => pro.ZRRLSNAME.Contains(filterCondition.CCCCRiverLakeAndSeaName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ChanYeOneName), (pro) => pro.ZICSTD1NAME.Contains(filterCondition.ChanYeOneName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ChanYeRemark), (pro) => pro.ZICSTDREMARKS.Contains(filterCondition.ChanYeRemark))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ChanYeSecName), (pro) => pro.ZICSTD2NAME.Contains(filterCondition.ChanYeSecName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ChanYeThirdCode), (pro) => pro.ZICSTD3ID.Contains(filterCondition.ChanYeThirdCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ChanYeThirdName), (pro) => pro.ZICSTD3NAME.Contains(filterCondition.ChanYeThirdName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BSectorOneCode), (pro) => pro.ZBUSTD1ID.Contains(filterCondition.BSectorOneCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BSectorSecCode), (pro) => pro.ZBUSTD2ID.Contains(filterCondition.BSectorSecCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BSectorThirdCode), (pro) => pro.ZBUSTD3ID.Contains(filterCondition.BSectorThirdCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CCCCBTypeCode), (pro) => pro.Z12TOPBID.Contains(filterCondition.CCCCBTypeCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CCCCBTypeOneCode), (pro) => pro.ZCPBC1ID.Contains(filterCondition.CCCCBTypeOneCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CCCCBTypeSecCode), (pro) => pro.ZCPBC2ID.Contains(filterCondition.CCCCBTypeSecCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CCCCBTypeThirdCode), (pro) => pro.ZCPBC3ID.Contains(filterCondition.CCCCBTypeThirdCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CCCCRiverLakeAndSeaCode), (pro) => pro.ZRRLSID.Contains(filterCondition.CCCCRiverLakeAndSeaCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ChanYeOneCode), (pro) => pro.ZICSTD1ID.Contains(filterCondition.ChanYeOneCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ChanYeSecCode), (pro) => pro.ZICSTD2ID.Contains(filterCondition.ChanYeSecCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZCPBC1NAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    ChanYeThirdCode = cc.ZICSTD3ID,
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
                    ThirdNewBType = cc.ZNEW3TOB == "1" ? "是" : "否",
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    ChanYeThirdCode = cc.ZICSTD3ID,
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
                    ThirdNewBType = cc.ZNEW3TOB == "1" ? "是" : "否",
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            RegionalCenterDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<RegionalCenterDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<RegionalCenter>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Code), (pro) => pro.ZCRCCODE.Contains(filterCondition.Code))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Description), (pro) => pro.ZCRCNAME.Contains(filterCondition.Description))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new RegionalCenterDetailsDto
                {
                    Id = cc.Id.ToString(),
                    Code = cc.ZCRCCODE,
                    Description = cc.ZCRCNAME,
                    State = cc.ZSTATE == "1" ? "有效" : "无效",
                    DataIdentifier = cc.ZDELETE == "1" ? "未删除" : "已删除",
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            NationalEconomyDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<NationalEconomyDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<NationalEconomy>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Code), (pro) => pro.ZNEQCODE.Contains(filterCondition.Code))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Descption), (pro) => pro.ZNEQDESC.Contains(filterCondition.Descption))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZNEQNAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SupCode), (pro) => pro.ZNEQCODEUP.Contains(filterCondition.SupCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;
        }
        ///// <summary>
        ///// 获取行政机构和核算机构映射关系 列表
        ///// </summary>
        ///// <param name="requestDto"></param>
        ///// <returns></returns>
        //public async Task<ResponseAjaxResult<List<AdministrativeAccountingMapperDetailsDto>>> GetAdministrativeAccountingMapperSearchAsync(FilterCondition requestDto)
        //{
        //    var responseAjaxResult = new ResponseAjaxResult<List<AdministrativeAccountingMapperDetailsDto>>();
        //    RefAsync<int> total = 0;

        //    //过滤条件
        //    AdministrativeAccountingMapperDetailsDto filterCondition = new();
        //    if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
        //    {
        //        filterCondition = JsonConvert.DeserializeObject<AdministrativeAccountingMapperDetailsDto>(requestDto.FilterConditionJson);
        //    }

        //    var ccList = await _dbContext.Queryable<AdministrativeAccountingMapper>()
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccOrgCode), (pro) => pro.ZAORGNO.Contains(filterCondition.AccOrgCode))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AccOrgId), (pro) => pro.ZAID.Contains(filterCondition.AccOrgId))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.AdministrativeOrgCode), (pro) => pro.ZORGCODE.Contains(filterCondition.AdministrativeOrgCode))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
        //        .Where((cc) => cc.IsDelete == 1)
        //        .Select((cc) => new AdministrativeAccountingMapperDetailsDto
        //        {
        //            Id = cc.Id.ToString(),
        //            AccOrgCode = cc.ZAORGNO,
        //            AccOrgId = cc.ZAID,
        //            AdministrativeOrgCode = cc.ZORGCODE,
        //            AdministrativeOrgId = cc.ZORGID,
        //            DataIdentifier = cc.ZDELETE,
        //            KeyId = cc.ZID,
        //            CreateTime = cc.CreateTime,
        //            UpdateTime = cc.UpdateTime
        //        })
        //        .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

        //    responseAjaxResult.Count = total;
        //    responseAjaxResult.SuccessResult(ccList);
        //    return responseAjaxResult;
        //}
        /// <summary>
        /// DH行政和核算机构映射 列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DHAdministrative>>> GetAdministrativeAccountingMapperSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DHAdministrative>>();
            RefAsync<int> total = 0;

            //过滤条件
            DHAdministrative filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<DHAdministrative>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<DHAdministrative>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzaorgno), (pro) => pro.Fzaorgno.Contains(filterCondition.Fzaorgno))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzstate), (pro) => pro.Fzstate.Contains(filterCondition.Fzstate))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzorgid), (pro) => pro.Fzorgid.Contains(filterCondition.Fzorgid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzorgcode), (pro) => pro.Fzorgcode.Contains(filterCondition.Fzorgcode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Fzaid), (pro) => pro.Fzaid.Contains(filterCondition.Fzaid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreatedAt.ToString()), (pro) => Convert.ToDateTime(pro.CreatedAt).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreatedAt).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdatedAt.ToString()), (pro) => Convert.ToDateTime(pro.UpdatedAt).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdatedAt).ToString("yyyy-MM-dd"))
                .Where((cc) => cc.Fzdelete == "1")
                .Select((cc) => new DHAdministrative
                {
                    Id = cc.Id,
                    CreatedAt = cc.CreatedAt,
                    Fzaid = cc.Fzaid,
                    Fzaorgno = cc.Fzaorgno,
                    Fzdelete = cc.IsDelete.ToString(),
                    Fzid = cc.Fzid,
                    Fzorgcode = cc.Fzorgcode,
                    Fzorgid = cc.Fzorgid,
                    Fzstate = cc.Fzstate,
                    Fzversion = cc.Fzversion,
                    UpdatedAt = cc.UpdatedAt
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
                    KeyId = cc.ZID,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
                })
                .FirstAsync();

            responseAjaxResult.SuccessResult(result);
            return responseAjaxResult;

        }
        /// <summary>
        ///  多组织-税务代管组织(行政)列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<EscrowOrganizationDetailsDto>>> GetEscrowOrganizationSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<EscrowOrganizationDetailsDto>>();
            RefAsync<int> total = 0;

            //过滤条件
            EscrowOrganizationDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<EscrowOrganizationDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<EscrowOrganization>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Country), (pro) => pro.CAREA.Contains(filterCondition.Country))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.IsIndependenceAcc), (pro) => pro.IS_INDEPENDENT.Contains(filterCondition.IsIndependenceAcc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.LocationOfOrg), (pro) => pro.ORGPROVINCE.Contains(filterCondition.LocationOfOrg))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.NAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.NameEnglish), (pro) => pro.ENGLISHNAME.Contains(filterCondition.NameEnglish))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.NameLLanguage), (pro) => pro.ZZTNAME_LOC.Contains(filterCondition.NameLLanguage))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgStatus), (pro) => pro.STATUS.Contains(filterCondition.OrgStatus))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.RegionalAttr), (pro) => pro.TERRITORYPRO.Contains(filterCondition.RegionalAttr))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Remark), (pro) => pro.NOTE.Contains(filterCondition.Remark))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Shareholding), (pro) => pro.SHAREHOLDINGS.Contains(filterCondition.Shareholding))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ShortNameChinese), (pro) => pro.SHORTNAME.Contains(filterCondition.ShortNameChinese))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TelAddress), (pro) => pro.ZADDRESS.Contains(filterCondition.TelAddress))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.HROrgMDCode), (pro) => pro.OID.Contains(filterCondition.HROrgMDCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgAttr), (pro) => pro.TYPE.Contains(filterCondition.OrgAttr))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgChildAttr), (pro) => pro.TYPEEXT.Contains(filterCondition.OrgChildAttr))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgCode), (pro) => pro.OCODE.Contains(filterCondition.OrgCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgGruleCode), (pro) => pro.ORULE.Contains(filterCondition.OrgGruleCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.OrgMDCode), (pro) => pro.MDM_CODE.Contains(filterCondition.OrgMDCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.RegistrationNo), (pro) => pro.REGISTERCODE.Contains(filterCondition.RegistrationNo))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ShortNameEnglish), (pro) => pro.ENGLISHSHORTNAME.Contains(filterCondition.ShortNameEnglish))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ShortNameLLanguage), (pro) => pro.ZZTSHNAME_LOC.Contains(filterCondition.ShortNameLLanguage))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SupHROrgMDCode), (pro) => pro.POID.Contains(filterCondition.SupHROrgMDCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SupOrgMDCode), (pro) => pro.ZORGUP.Contains(filterCondition.SupOrgMDCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TreeLevel), (pro) => pro.GRADE.Contains(filterCondition.TreeLevel))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UnitSec), (pro) => pro.GPOID.Contains(filterCondition.UnitSec))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            //行政区划
            var locationKey = ccList.Select(x => x.LocationOfOrg).ToList();
            var location = await _dbContext.Queryable<AdministrativeDivision>()
                .Where(t => t.IsDelete == 1 && locationKey.Contains(t.ZADDVSCODE))
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZADDVSCODE, Name = t.ZADDVSNAME })
                .ToListAsync();

            //国家地区
            var countrysKey = ccList.Select(x => x.Country).ToList();
            var country = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1 && countrysKey.Contains(t.ZCOUNTRYCODE))
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZCOUNTRYCODE, Name = t.ZCOUNTRYNAME })
                .ToListAsync();

            //值域信息
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();

            foreach (var item in ccList)
            {
                //机构所在地
                item.LocationOfOrg = GetAdministrativeDivision(item.LocationOfOrg, location);

                //国家地区
                item.Country = GetCountryRegion(item.Country, country);

                //持股情况
                item.Shareholding = valDomain.FirstOrDefault(x => item.Shareholding == x.ZDOM_VALUE && x.ZDOM_CODE == "ZHOLDING")?.ZDOM_NAME;

                //是否独立核算
                item.IsIndependenceAcc = valDomain.FirstOrDefault(x => item.IsIndependenceAcc == x.ZDOM_VALUE && x.ZDOM_CODE == "ZCHECKIND")?.ZDOM_NAME;

                //机构状态
                item.OrgStatus = valDomain.FirstOrDefault(x => item.OrgStatus == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGSTATE")?.ZDOM_NAME;

                //机构属性
                item.OrgAttr = valDomain.FirstOrDefault(x => item.OrgAttr == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGATTR")?.ZDOM_NAME;

                //机构子属性
                item.OrgChildAttr = valDomain.FirstOrDefault(x => item.OrgChildAttr == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGCHILDATTR")?.ZDOM_NAME;
            }

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 行政组织-多组织
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DHOrganzationDep>>> GetXZOrganzationSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DHOrganzationDep>>();
            RefAsync<int> total = 0;

            //过滤条件
            DHOrganzationDep filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<DHOrganzationDep>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<DHOrganzationDep>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.MdmCode), (pro) => pro.MdmCode.Contains(filterCondition.MdmCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Oid), (pro) => pro.Oid.Contains(filterCondition.Oid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztnameZh), (pro) => pro.ZztnameZh.Contains(filterCondition.ZztnameZh))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztshnameChs), (pro) => pro.ZztshnameChs.Contains(filterCondition.ZztshnameChs))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztshnameEn), (pro) => pro.ZztshnameEn.Contains(filterCondition.ZztshnameEn))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztnameLoc), (pro) => pro.ZztnameLoc.Contains(filterCondition.ZztnameLoc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztshnameLoc), (pro) => pro.ZztshnameLoc.Contains(filterCondition.ZztshnameLoc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zostate), (pro) => pro.Zostate.Contains(filterCondition.Zostate))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zorgup), (pro) => pro.Zorgup.Contains(filterCondition.Zorgup))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Poid), (pro) => pro.Poid.Contains(filterCondition.Poid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zorule), (pro) => pro.Zorule.Contains(filterCondition.Zorule))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zgpoid), (pro) => pro.Zgpoid.Contains(filterCondition.Zgpoid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZoLevel), (pro) => pro.ZoLevel.Contains(filterCondition.ZoLevel))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zsno), (pro) => pro.Zsno.Contains(filterCondition.Zsno))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zorgno), (pro) => pro.Zorgno.Contains(filterCondition.Zorgno))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zoattr), (pro) => pro.Zoattr.Contains(filterCondition.Zoattr))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zocattr), (pro) => pro.Zocattr.Contains(filterCondition.Zocattr))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zcyname), (pro) => pro.Zcyname.Contains(filterCondition.Zcyname))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zorgloc), (pro) => pro.Zorgloc.Contains(filterCondition.Zorgloc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zcuscc), (pro) => pro.Zcuscc.Contains(filterCondition.Zcuscc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zaddress), (pro) => pro.Zaddress.Contains(filterCondition.Zaddress))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zholding), (pro) => pro.Zholding.Contains(filterCondition.Zholding))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zcheckind), (pro) => pro.Zcheckind.Contains(filterCondition.Zcheckind))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Ztreeid1), (pro) => pro.Ztreeid1.Contains(filterCondition.Ztreeid1))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Ztreever), (pro) => pro.Ztreever.Contains(filterCondition.Ztreever))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ViewFlag), (pro) => pro.ViewFlag.Contains(filterCondition.ViewFlag))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zentc), (pro) => pro.Zentc.Contains(filterCondition.Zentc))
                //.WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                //.WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                //.Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new DHOrganzationDep
                {
                    Id = cc.Id,
                    MdmCode = cc.MdmCode,
                    Oid = cc.Oid,
                    Poid = cc.Poid,
                    ViewFlag = cc.ViewFlag,
                    Zaddress = cc.Zaddress,
                    Zcheckind = cc.Zcuscc,
                    Zcuscc = cc.Zcuscc,
                    Zcyname = cc.Zcyname,
                    Zentc = cc.Zentc,
                    Zgpoid = cc.Zgpoid,
                    Zholding = cc.Zholding,
                    Zoattr = cc.Zoattr,
                    Zocattr = cc.Zocattr,
                    ZoLevel = cc.ZoLevel,
                    Zorgloc = cc.Zorgloc,
                    Zorgno = cc.Zorgno,
                    Zorgup = cc.Zorgup,
                    Zorule = cc.Zorule,
                    Zregional = cc.Zregional,
                    Zostate = cc.Zostate,
                    Ztreeid1 = cc.Ztreeid1,
                    Zsno = cc.Zsno,
                    Ztreever = cc.Ztreever,
                    ZztnameLoc = cc.ZztnameLoc,
                    ZztnameZh = cc.ZztnameZh,
                    ZztshnameEn = cc.ZztshnameEn,
                    ZztshnameLoc = cc.ZztshnameLoc,
                    ZztshnameChs = cc.ZztshnameChs
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            //值域信息
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();

            //国家地区
            var countrysKey = ccList.Select(x => x.Zcyname).ToList();
            var country = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1 && countrysKey.Contains(t.ZCOUNTRYCODE))
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZCOUNTRYCODE, Name = t.ZCOUNTRYNAME })
                .ToListAsync();

            //行政区划
            var locationKey = ccList.Select(x => x.Zorgloc).ToList();
            var location = await _dbContext.Queryable<AdministrativeDivision>()
                .Where(t => t.IsDelete == 1 && locationKey.Contains(t.ZADDVSCODE))
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZADDVSCODE, Name = t.ZADDVSNAME })
                .ToListAsync();

            foreach (var item in ccList)
            {
                //机构属性
                item.Zoattr = valDomain.FirstOrDefault(x => item.Zoattr == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGATTR")?.ZDOM_NAME;

                //机构子属性
                item.Zocattr = valDomain.FirstOrDefault(x => item.Zocattr == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGCHILDATTR")?.ZDOM_NAME;

                //是否独立核算
                item.Zcheckind = valDomain.FirstOrDefault(x => item.Zcheckind == x.ZDOM_VALUE && x.ZDOM_CODE == "ZCHECKIND")?.ZDOM_NAME;

                //机构状态
                item.Zostate = valDomain.FirstOrDefault(x => item.Zostate == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGSTATE")?.ZDOM_NAME;

                //持股情况
                item.Zholding = valDomain.FirstOrDefault(x => item.Zholding == x.ZDOM_VALUE && x.ZDOM_CODE == "ZHOLDING")?.ZDOM_NAME;

                //企业分类代码
                item.Zentc = valDomain.FirstOrDefault(x => item.Zentc == x.ZDOM_VALUE && x.ZDOM_CODE == "ZENTC")?.ZDOM_NAME;

                //国家地区
                item.Zcyname = GetCountryRegion(item.Zcyname, country);

                //机构所在地
                item.Zorgloc = GetAdministrativeDivision(item.Zorgloc, location);
            }

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
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
        public async Task<ResponseAjaxResult<List<DHOpportunityDto>>> GetBusinessNoCpportunitySearchAsync(FilterCondition requestDto, bool isJingWai)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DHOpportunityDto>>();
            RefAsync<int> total = 0;

            //过滤条件
            DHOpportunityDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<DHOpportunityDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<DHOpportunity>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreatedAt.ToString()), (pro) => pro.CreatedAt.ToString().Contains(filterCondition.CreatedAt.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdatedAt.ToString()), (pro) => pro.UpdatedAt.ToString().Contains(filterCondition.UpdatedAt.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Z2NDORG), (pro) => pro.Z2NDORG.Contains(filterCondition.Z2NDORG))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZBOP), (pro) => pro.ZPROJTYPE.Contains(filterCondition.ZBOP))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZBOPN), (pro) => pro.ZBOPN.Contains(filterCondition.ZBOPN))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZBOPN_EN), (pro) => pro.ZBOPN_EN.Contains(filterCondition.ZBOPN_EN))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZCPBC), (pro) => pro.ZCPBC.Contains(filterCondition.ZCPBC))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZCY2NDORG), (pro) => pro.ZCY2NDORG.Contains(filterCondition.ZCY2NDORG))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZORG), (pro) => pro.ZORG.Contains(filterCondition.ZORG))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZORG_QUAL), (pro) => pro.ZORG_QUAL.Contains(filterCondition.ZORG_QUAL))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPROJLOC), (pro) => pro.ZPROJLOC.Contains(filterCondition.ZPROJLOC))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPROJTYPE), (pro) => pro.ZPROJTYPE.Contains(filterCondition.ZPROJTYPE))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZSFOLDATE.ToString()), (pro) => pro.ZSFOLDATE.ToString().Contains(filterCondition.ZSFOLDATE.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZSTATE), (pro) => pro.ZSTATE.Contains(filterCondition.ZSTATE))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZTAXMETHOD), (pro) => pro.ZTAXMETHOD.Contains(filterCondition.ZTAXMETHOD))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZZCOUNTRY), (pro) => pro.ZZCOUNTRY.Contains(filterCondition.ZZCOUNTRY))
                //.WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                //.WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                .WhereIF((isJingWai), (cc) => cc.ZZCOUNTRY == "142")
                .WhereIF((!isJingWai), (cc) => cc.ZZCOUNTRY != "142")
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new DHOpportunityDto
                {
                    Zdelete = cc.IsDelete.ToString(),
                    CreatedAt = cc.CreatedAt,
                    UpdatedAt = cc.UpdatedAt,
                    Z2NDORG = cc.Z2NDORG,
                    ZAWARDP_LIST = cc.ZAWARDP_LIST,
                    ZBOP = cc.ZBOP,
                    ZBOPN = cc.ZBOPN,
                    ZBOPN_EN = cc.ZBOPN_EN,
                    ZCPBC = cc.ZCPBC,
                    ZCY2NDORG = cc.ZCY2NDORG,
                    ZORG = cc.ZORG,
                    ZORG_QUAL = cc.ZORG_QUAL,
                    ZPROJLOC = cc.ZPROJLOC,
                    ZPROJTYPE = cc.ZPROJTYPE,
                    ZSFOLDATE = cc.ZSFOLDATE,
                    ZSTATE = cc.ZSTATE,
                    ZTAXMETHOD = cc.ZTAXMETHOD,
                    ZZCOUNTRY = cc.ZZCOUNTRY
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            //值域信息
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();

            //行政区划
            var locationKey = ccList.Select(x => x.ZPROJLOC).ToList();
            var location = await _dbContext.Queryable<AdministrativeDivision>()
                .Where(t => t.IsDelete == 1 && locationKey.Contains(t.ZADDVSCODE))
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZADDVSCODE, Name = t.ZADDVSNAME })
                .ToListAsync();

            //项目机构 
            var pjectOrgKey = ccList.Select(x => x.ZORG_QUAL).ToList();
            var pjectOrgKey2 = ccList.Select(x => x.ZORG).ToList();
            var pjectOrgKey3 = ccList.Select(x => x.ZCY2NDORG).ToList();
            var pjectOrgKey4 = ccList.Select(x => x.Z2NDORG).ToList();
            var pjectOrg = await _dbContext.Queryable<Institution>()
                .Where(t => t.IsDelete == 1 && (pjectOrgKey.Contains(t.OID) || pjectOrgKey2.Contains(t.OID) || pjectOrgKey3.Contains(t.OID) || pjectOrgKey4.Contains(t.OID)))
                .Select(t => new InstutionRespDto { Oid = t.OID, PoId = t.POID, Grule = t.GRULE, Name = t.NAME })
                .ToListAsync();

            //国家地区
            var countrysKey = ccList.Select(x => x.ZZCOUNTRY).ToList();
            var country = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1 && countrysKey.Contains(t.ZCOUNTRYCODE))
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZCOUNTRYCODE, Name = t.ZCOUNTRYNAME })
                .ToListAsync();

            foreach (var item in ccList)
            {
                if (!string.IsNullOrWhiteSpace(item.ZAWARDP_LIST))
                {
                    var jsonObject = JObject.Parse(item.ZAWARDP_LIST);
                    if (!string.IsNullOrWhiteSpace(jsonObject["item"].ToString()))
                    {
                        item.DHAwardpList = JsonConvert.DeserializeObject<List<DHAwardpList>>(jsonObject["item"].ToString());
                    }
                }

                //项目类型
                item.ZPROJTYPE = GetValueDomain(item.ZPROJTYPE, valDomain, "ZPROJTYPE");

                //项目所在地
                item.ZPROJLOC = GetAdministrativeDivision(item.ZPROJLOC, location);

                //计税方式
                item.ZTAXMETHOD = GetValueDomain(item.ZTAXMETHOD, valDomain, "ZTAXMETHOD");

                //参与单位
                item.ZCY2NDORG = GetInstitutionName(item.ZCY2NDORG, pjectOrg);

                //资质单位
                item.ZORG_QUAL = GetInstitutionName(item.ZORG_QUAL, pjectOrg);

                //跟踪单位
                item.ZORG = GetInstitutionName(item.ZORG, pjectOrg);

                //所属二级单位
                item.Z2NDORG = GetInstitutionName(item.Z2NDORG, pjectOrg);

                //国家地区
                item.ZZCOUNTRY = GetCountryRegion(item.ZZCOUNTRY, country);

                //中交项目业务分类
                item.ZCPBC = GetValueDomain(item.ZCPBC, valDomain, "ZCPBC");

            }

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        //public async Task<ResponseAjaxResult<List<BusinessNoCpportunityDetailsDto>>> GetBusinessNoCpportunitySearchAsync(FilterCondition requestDto, bool isJingWai)
        //{
        //    var responseAjaxResult = new ResponseAjaxResult<List<BusinessNoCpportunityDetailsDto>>();
        //    RefAsync<int> total = 0;

        //    //过滤条件
        //    BusinessNoCpportunityDetailsDto filterCondition = new();
        //    if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
        //    {
        //        filterCondition = JsonConvert.DeserializeObject<BusinessNoCpportunityDetailsDto>(requestDto.FilterConditionJson);
        //    }

        //    var ccList = await _dbContext.Queryable<BusinessCpportunity>()
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Country), (pro) => pro.ZZCOUNTRY.Contains(filterCondition.Country))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BPjectForeignName), (pro) => pro.ZBOPN_EN.Contains(filterCondition.BPjectForeignName))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BPjectMDCode), (pro) => pro.ZBOP.Contains(filterCondition.BPjectMDCode))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZBOPN.Contains(filterCondition.Name))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PjectType), (pro) => pro.ZPROJTYPE.Contains(filterCondition.PjectType))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.QualificationUnit), (pro) => pro.ZORG_QUAL.Contains(filterCondition.QualificationUnit))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.State), (pro) => pro.ZSTATE.Contains(filterCondition.State))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TaxationMethod), (pro) => pro.ZTAXMETHOD.Contains(filterCondition.TaxationMethod))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.BTypeOfCCCCProjects), (pro) => pro.ZCPBC.Contains(filterCondition.BTypeOfCCCCProjects))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ParticipatingUnits), (pro) => pro.ZCY2NDORG.Contains(filterCondition.ParticipatingUnits))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.PjectLocation), (pro) => pro.ZPROJLOC.Contains(filterCondition.PjectLocation))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.StartTrackingDate), (pro) => pro.ZSFOLDATE.Contains(filterCondition.StartTrackingDate))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.TrackingUnit), (pro) => pro.ZORG.Contains(filterCondition.TrackingUnit))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UnitSec), (pro) => pro.Z2NDORG.Contains(filterCondition.UnitSec))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
        //        .WhereIF((isJingWai), (cc) => cc.ZZCOUNTRY == "142")
        //        .WhereIF((!isJingWai), (cc) => cc.ZZCOUNTRY != "142")
        //        .Where((cc) => cc.IsDelete == 1)
        //        .Select((cc) => new BusinessNoCpportunityDetailsDto
        //        {
        //            Id = cc.Id.ToString(),
        //            Country = cc.ZZCOUNTRY,
        //            BPjectForeignName = cc.ZBOPN_EN,
        //            BPjectMDCode = cc.ZBOP,
        //            Name = cc.ZBOPN,
        //            PjectType = cc.ZPROJTYPE,
        //            QualificationUnit = cc.ZORG_QUAL,
        //            State = cc.ZSTATE == "1" ? "有效" : "无效",
        //            TaxationMethod = cc.ZTAXMETHOD,
        //            BTypeOfCCCCProjects = cc.ZCPBC,
        //            ParticipatingUnits = cc.ZCY2NDORG,
        //            PjectLocation = cc.ZPROJLOC,
        //            StartTrackingDate = cc.ZSFOLDATE,
        //            TrackingUnit = cc.ZORG,
        //            UnitSec = cc.Z2NDORG,
        //            CreateTime = cc.CreateTime,
        //            UpdateTime = cc.UpdateTime
        //        })
        //        .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

        //    responseAjaxResult.Count = total;
        //    responseAjaxResult.SuccessResult(ccList);
        //    return responseAjaxResult;
        //}
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
                    UnitSec = cc.Z2NDORG,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            AdministrativeDivisionDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<AdministrativeDivisionDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<AdministrativeDivision>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CodeOfCCCCRegional), (pro) => pro.ZCRHCODE.Contains(filterCondition.CodeOfCCCCRegional))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.RegionalismCode), (pro) => pro.ZADDVSCODE.Contains(filterCondition.RegionalismCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.RegionalismLevel), (pro) => pro.ZADDVSLEVEL.Contains(filterCondition.RegionalismLevel))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZADDVSNAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.State), (pro) => pro.ZSTATE.Contains(filterCondition.State))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.SupRegionalismCode), (pro) => pro.ZADDVSUP.Contains(filterCondition.SupRegionalismCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
        //public async Task<ResponseAjaxResult<List<AccountingOrganizationDetailsDto>>> GetAccountingOrganizationSearchAsync(FilterCondition requestDto)
        //{
        //    var responseAjaxResult = new ResponseAjaxResult<List<AccountingOrganizationDetailsDto>>();
        //    RefAsync<int> total = 0;

        //    //过滤条件
        //    AccountingOrganizationDetailsDto filterCondition = new();
        //    if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
        //    {
        //        filterCondition = JsonConvert.DeserializeObject<AccountingOrganizationDetailsDto>(requestDto.FilterConditionJson);
        //    }

        //    var ccList = await _dbContext.Queryable<AccountingOrganization>()
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.MDM_CODE), (pro) => pro.MDM_CODE.Contains(filterCondition.MDM_CODE))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACNAME_CHS), (pro) => pro.ZACNAME_CHS.Contains(filterCondition.ZACNAME_CHS))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACNAME_EN), (pro) => pro.ZACNAME_EN.Contains(filterCondition.ZACNAME_EN))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACNAME_LOC), (pro) => pro.ZACNAME_LOC.Contains(filterCondition.ZACNAME_LOC))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACO), (pro) => pro.ZACO.Contains(filterCondition.ZACO))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACSHORTNAME_CHS), (pro) => pro.ZACSHORTNAME_CHS.Contains(filterCondition.ZACSHORTNAME_CHS))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACSHORTNAME_EN), (pro) => pro.ZACSHORTNAME_EN.Contains(filterCondition.ZACSHORTNAME_EN))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACSHORTNAME_LOC), (pro) => pro.ZACSHORTNAME_LOC.Contains(filterCondition.ZACSHORTNAME_LOC))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZBRGZW), (pro) => pro.ZBRGZW.Contains(filterCondition.ZBRGZW))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZBRHW), (pro) => pro.ZBRHW.Contains(filterCondition.ZBRHW))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZCONTINENTCODE), (pro) => pro.ZCONTINENTCODE.Contains(filterCondition.ZCONTINENTCODE))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZCYNAME), (pro) => pro.ZCYNAME.Contains(filterCondition.ZCYNAME))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZHTE), (pro) => pro.ZHTE.Contains(filterCondition.ZHTE))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZH_IN_OUT), (pro) => pro.ZH_IN_OUT.Contains(filterCondition.ZH_IN_OUT))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZOSTATE), (pro) => pro.ZOSTATE.Contains(filterCondition.ZOSTATE))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZREMARK), (pro) => pro.ZREMARK.Contains(filterCondition.ZREMARK))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZRPNATURE), (pro) => pro.ZRPNATURE.Contains(filterCondition.ZRPNATURE))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZYORGSTATE), (pro) => pro.ZYORGSTATE.Contains(filterCondition.ZYORGSTATE))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZZTNAME_EN), (pro) => pro.ZZTNAME_EN.Contains(filterCondition.ZZTNAME_EN))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZZTNAME_LOC), (pro) => pro.ZZTNAME_LOC.Contains(filterCondition.ZZTNAME_LOC))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZZTNAME_ZH), (pro) => pro.ZZTNAME_ZH.Contains(filterCondition.ZZTNAME_ZH))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZZTSHNAME_CHS), (pro) => pro.ZZTSHNAME_CHS.Contains(filterCondition.ZZTSHNAME_CHS))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZZTSHNAME_EN), (pro) => pro.ZZTSHNAME_EN.Contains(filterCondition.ZZTSHNAME_EN))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZZTSHNAME_LOC), (pro) => pro.ZZTSHNAME_LOC.Contains(filterCondition.ZZTSHNAME_LOC))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACCOUNT_DATE), (pro) => pro.ZACCOUNT_DATE.Contains(filterCondition.ZACCOUNT_DATE))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACDISABLEYEAR), (pro) => pro.ZACDISABLEYEAR.Contains(filterCondition.ZACDISABLEYEAR))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACID), (pro) => pro.ZACID.Contains(filterCondition.ZACID))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACISDETAIL), (pro) => pro.ZACISDETAIL.Contains(filterCondition.ZACISDETAIL))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACJTHBFWN), (pro) => pro.ZACJTHBFWN.Contains(filterCondition.ZACJTHBFWN))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACLAYER), (pro) => pro.ZACLAYER.Contains(filterCondition.ZACLAYER))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACORGNO), (pro) => pro.ZACORGNO.Contains(filterCondition.ZACORGNO))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACPARENTCODE), (pro) => pro.ZACPARENTCODE.Contains(filterCondition.ZACPARENTCODE))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACPARENTID), (pro) => pro.ZACPARENTID.Contains(filterCondition.ZACPARENTID))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACPATH), (pro) => pro.ZACPATH.Contains(filterCondition.ZACPATH))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACSORTORDER), (pro) => pro.ZACSORTORDER.Contains(filterCondition.ZACSORTORDER))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZAORGSTATE), (pro) => pro.ZAORGSTATE.Contains(filterCondition.ZAORGSTATE))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZAPPROVAL_ORG), (pro) => pro.ZAPPROVAL_ORG.Contains(filterCondition.ZAPPROVAL_ORG))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZBBID), (pro) => pro.ZBBID.Contains(filterCondition.ZBBID))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZBTID), (pro) => pro.ZBTID.Contains(filterCondition.ZBTID))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZBUSINESS_RECOCATION), (pro) => pro.ZBUSINESS_RECOCATION.Contains(filterCondition.ZBUSINESS_RECOCATION))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZTREEID), (pro) => pro.ZTREEID.Contains(filterCondition.ZTREEID))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZBUSINESS_UNIT), (pro) => pro.ZBUSINESS_UNIT.Contains(filterCondition.ZBUSINESS_UNIT))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZCWYGL), (pro) => pro.ZCWYGL.Contains(filterCondition.ZCWYGL))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZCWYGL_REA), (pro) => pro.ZCWYGL_REA.Contains(filterCondition.ZCWYGL_REA))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZDCID), (pro) => pro.ZDCID.Contains(filterCondition.ZDCID))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZDEL_MAP), (pro) => pro.ZDEL_MAP.Contains(filterCondition.ZDEL_MAP))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZDEL_REA), (pro) => pro.ZDEL_REA.Contains(filterCondition.ZDEL_REA))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZIVFLGID), (pro) => pro.ZIVFLGID.Contains(filterCondition.ZIVFLGID))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZNBFYL), (pro) => pro.ZNBFYL.Contains(filterCondition.ZNBFYL))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZORGATTR), (pro) => pro.ZORGATTR.Contains(filterCondition.ZORGATTR))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZORGCHILDATTR), (pro) => pro.ZORGCHILDATTR.Contains(filterCondition.ZORGCHILDATTR))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZORGLOC), (pro) => pro.ZORGLOC.Contains(filterCondition.ZORGLOC))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZQYBBDAT), (pro) => pro.ZQYBBDAT.Contains(filterCondition.ZQYBBDAT))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZREGIONAL), (pro) => pro.ZREGIONAL.Contains(filterCondition.ZREGIONAL))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZREPORT_FLAG), (pro) => pro.ZREPORT_FLAG.Contains(filterCondition.ZREPORT_FLAG))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZREPORT_NODE), (pro) => pro.ZREPORT_NODE.Contains(filterCondition.ZREPORT_NODE))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZREPORT_TIME), (pro) => pro.ZREPORT_TIME.Contains(filterCondition.ZREPORT_TIME))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZRULE), (pro) => pro.ZRULE.Contains(filterCondition.ZRULE))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZSCENTER), (pro) => pro.ZSCENTER.Contains(filterCondition.ZSCENTER))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZSNO), (pro) => pro.ZSNO.Contains(filterCondition.ZSNO))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZTAXMETHOD), (pro) => pro.ZTAXMETHOD.Contains(filterCondition.ZTAXMETHOD))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZTAXPAYER_CATEGORY), (pro) => pro.ZTAXPAYER_CATEGORY.Contains(filterCondition.ZTAXPAYER_CATEGORY))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZTAX_ORGANIZATION), (pro) => pro.ZTAX_ORGANIZATION.Contains(filterCondition.ZTAX_ORGANIZATION))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZTORG), (pro) => pro.ZTORG.Contains(filterCondition.ZTORG))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZTREEVER), (pro) => pro.ZTREEVER.Contains(filterCondition.ZTREEVER))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZTRNO), (pro) => pro.ZTRNO.Contains(filterCondition.ZTRNO))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZUNAME), (pro) => pro.ZUNAME.Contains(filterCondition.ZUNAME))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZZCURRENCY), (pro) => pro.ZZCURRENCY.Contains(filterCondition.ZZCURRENCY))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
        //        .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
        //        .Where((cc) => cc.IsDelete == 1)
        //        .Select((cc) => new AccountingOrganizationDetailsDto
        //        {
        //            Id = cc.Id.ToString(),
        //            MDM_CODE = cc.MDM_CODE,
        //            ZACNAME_CHS = cc.ZACNAME_CHS,
        //            ZACNAME_EN = cc.ZACNAME_EN,
        //            ZACNAME_LOC = cc.ZACNAME_LOC,
        //            ZACO = cc.ZACO,
        //            ZACSHORTNAME_CHS = cc.ZACSHORTNAME_CHS,
        //            ZACSHORTNAME_EN = cc.ZACSHORTNAME_EN,
        //            ZACSHORTNAME_LOC = cc.ZACSHORTNAME_LOC,
        //            ZBRGZW = cc.ZBRGZW,
        //            ZBRHW = cc.ZBRHW,
        //            ZCONTINENTCODE = cc.ZCONTINENTCODE,
        //            ZCYNAME = cc.ZCYNAME,
        //            ZHTE = cc.ZHTE,
        //            ZH_IN_OUT = cc.ZH_IN_OUT,
        //            ZOSTATE = cc.ZOSTATE,
        //            ZREMARK = cc.ZREMARK,
        //            ZRPNATURE = cc.ZRPNATURE,
        //            ZSYSTEM = cc.ZSYSTEM,
        //            ZYORGSTATE = cc.ZYORGSTATE,
        //            ZZTNAME_EN = cc.ZZTNAME_EN,
        //            ZZTNAME_LOC = cc.ZZTNAME_LOC,
        //            ZZTNAME_ZH = cc.ZZTNAME_ZH,
        //            ZZTSHNAME_CHS = cc.ZZTSHNAME_CHS,
        //            ZZTSHNAME_EN = cc.ZZTSHNAME_EN,
        //            ZZTSHNAME_LOC = cc.ZZTSHNAME_LOC,
        //            VIEW_FLAG = cc.VIEW_FLAG,
        //            ZACCOUNT_DATE = cc.ZACCOUNT_DATE,
        //            ZACDISABLEYEAR = cc.ZACDISABLEYEAR,
        //            ZACID = cc.ZACID,
        //            ZACISDETAIL = cc.ZACISDETAIL,
        //            ZACJTHBFWN = cc.ZACJTHBFWN,
        //            ZACLAYER = cc.ZACLAYER,
        //            ZACORGNO = cc.ZACORGNO,
        //            ZACPARENTCODE = cc.ZACPARENTCODE,
        //            ZACPARENTID = cc.ZACPARENTID,
        //            ZACPATH = cc.ZACPATH,
        //            ZACSORTORDER = cc.ZACSORTORDER,
        //            ZAORGSTATE = cc.ZAORGSTATE,
        //            ZAPPROVAL_ORG = cc.ZAPPROVAL_ORG,
        //            ZBBID = cc.ZBBID,
        //            ZBTID = cc.ZBTID,
        //            ZBUSINESS_RECOCATION = cc.ZBUSINESS_RECOCATION,
        //            ZTREEID = cc.ZTREEID,
        //            ZBUSINESS_UNIT = cc.ZBUSINESS_UNIT,
        //            ZCWYGL = cc.ZCWYGL,
        //            ZCWYGL_REA = cc.ZCWYGL_REA,
        //            ZDCID = cc.ZDCID,
        //            ZDEL_MAP = cc.ZDEL_MAP,
        //            ZDEL_REA = cc.ZDEL_REA,
        //            ZIVFLGID = cc.ZIVFLGID,
        //            ZNBFYL = cc.ZNBFYL,
        //            ZORGATTR = cc.ZORGATTR,
        //            ZORGCHILDATTR = cc.ZORGCHILDATTR,
        //            ZORGLOC = cc.ZORGLOC,
        //            ZQYBBDAT = cc.ZQYBBDAT,
        //            ZREGIONAL = cc.ZREGIONAL,
        //            ZREPORT_FLAG = cc.ZREPORT_FLAG,
        //            ZREPORT_NODE = cc.ZREPORT_NODE,
        //            ZREPORT_TIME = cc.ZREPORT_TIME,
        //            ZRULE = cc.ZRULE,
        //            ZSCENTER = cc.ZSCENTER,
        //            ZSNO = cc.ZSNO,
        //            ZTAXMETHOD = cc.ZTAXMETHOD,
        //            ZTAXPAYER_CATEGORY = cc.ZTAXPAYER_CATEGORY,
        //            ZTAX_ORGANIZATION = cc.ZTAX_ORGANIZATION,
        //            ZTORG = cc.ZTORG,
        //            ZTREEVER = cc.ZTREEVER,
        //            ZTRNO = cc.ZTRNO,
        //            ZUNAME = cc.ZUNAME,
        //            ZZCURRENCY = cc.ZZCURRENCY,
        //            CreateTime = cc.CreateTime,
        //            UpdateTime = cc.UpdateTime
        //        })
        //        .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

        //    responseAjaxResult.Count = total;
        //    responseAjaxResult.SuccessResult(ccList);
        //    return responseAjaxResult;
        //}
        public async Task<ResponseAjaxResult<List<DHAdjustAccountsMultipleOrg>>> GetAccountingOrganizationSearchAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DHAdjustAccountsMultipleOrg>>();
            RefAsync<int> total = 0;

            //过滤条件
            DHAdjustAccountsMultipleOrg filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<DHAdjustAccountsMultipleOrg>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<DHAdjustAccountsMultipleOrg>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.MdmCode), (pro) => pro.MdmCode.Contains(filterCondition.MdmCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztshnameLoc), (pro) => pro.ZztshnameLoc.Contains(filterCondition.ZztshnameLoc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztshnameEn), (pro) => pro.ZztshnameEn.Contains(filterCondition.ZztshnameEn))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zaco), (pro) => pro.Zaco.Contains(filterCondition.Zaco))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztnameLoc), (pro) => pro.ZztnameLoc.Contains(filterCondition.ZztnameLoc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztnameZh), (pro) => pro.ZztnameZh.Contains(filterCondition.ZztnameZh))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztnameEn), (pro) => pro.ZztnameEn.Contains(filterCondition.ZztnameEn))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztshnameChs), (pro) => pro.ZztshnameChs.Contains(filterCondition.ZztshnameChs))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZztshnameLoc), (pro) => pro.ZztshnameLoc.Contains(filterCondition.ZztshnameLoc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zyorgstate), (pro) => pro.Zyorgstate.Contains(filterCondition.Zyorgstate))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zremark), (pro) => pro.Zremark.Contains(filterCondition.Zremark))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Ztreeid), (pro) => pro.Ztreeid.Contains(filterCondition.Ztreeid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Ztreever), (pro) => pro.Ztreever.Contains(filterCondition.Ztreever))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Ztorg), (pro) => pro.Ztorg.Contains(filterCondition.Ztorg))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zrule), (pro) => pro.Zrule.Contains(filterCondition.Zrule))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zgpoid), (pro) => pro.Zgpoid.Contains(filterCondition.Zgpoid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zaclayer), (pro) => pro.Zaclayer.Contains(filterCondition.Zaclayer))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zacsortorder), (pro) => pro.Zacsortorder.Contains(filterCondition.Zacsortorder))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZacnameChs), (pro) => pro.ZacnameChs.Contains(filterCondition.ZacnameChs))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZacnameEn), (pro) => pro.ZacnameEn.Contains(filterCondition.ZacnameEn))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZacnameLoc), (pro) => pro.ZacnameLoc.Contains(filterCondition.ZacnameLoc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZacshortnameChs), (pro) => pro.ZacshortnameChs.Contains(filterCondition.ZacshortnameChs))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZacshortnameEn), (pro) => pro.ZacshortnameEn.Contains(filterCondition.ZacshortnameEn))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZacshortnameLoc), (pro) => pro.ZacshortnameLoc.Contains(filterCondition.ZacshortnameLoc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zacparentcode), (pro) => pro.Zacparentcode.Contains(filterCondition.Zacparentcode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zivflgid), (pro) => pro.Zivflgid.Contains(filterCondition.Zivflgid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zacjthbfwn), (pro) => pro.Zacjthbfwn.Contains(filterCondition.Zacjthbfwn))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZreportFlag), (pro) => pro.ZreportFlag.Contains(filterCondition.ZreportFlag))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZreportTime), (pro) => pro.ZreportTime.Contains(filterCondition.ZreportTime))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZreportNode), (pro) => pro.ZreportNode.Contains(filterCondition.ZreportNode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zaorgstate), (pro) => pro.Zaorgstate.Contains(filterCondition.Zaorgstate))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZdelRea), (pro) => pro.ZdelRea.Contains(filterCondition.ZdelRea))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zacdisableyear), (pro) => pro.Zacdisableyear.Contains(filterCondition.Zacdisableyear))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZaccountDate), (pro) => pro.ZaccountDate.Contains(filterCondition.ZaccountDate))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zcyname), (pro) => pro.Zcyname.Contains(filterCondition.Zcyname))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zorgloc), (pro) => pro.Zorgloc.Contains(filterCondition.Zorgloc))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zregional), (pro) => pro.Zregional.Contains(filterCondition.Zregional))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zdcid), (pro) => pro.Zdcid.Contains(filterCondition.Zdcid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zbtid), (pro) => pro.Zbtid.Contains(filterCondition.Zbtid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zzcurrency), (pro) => pro.Zzcurrency.Contains(filterCondition.Zzcurrency))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Ztaxmethod), (pro) => pro.Ztaxmethod.Contains(filterCondition.Ztaxmethod))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZtaxOrganization), (pro) => pro.ZtaxOrganization.Contains(filterCondition.ZtaxOrganization))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZtaxpayerCategory), (pro) => pro.ZtaxpayerCategory.Contains(filterCondition.ZtaxpayerCategory))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Ztrno), (pro) => pro.Ztrno.Contains(filterCondition.Ztrno))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZbusinessUnit), (pro) => pro.ZbusinessUnit.Contains(filterCondition.ZbusinessUnit))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zuname), (pro) => pro.Zuname.Contains(filterCondition.Zuname))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZapprovalOrg), (pro) => pro.ZapprovalOrg.Contains(filterCondition.ZapprovalOrg))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zacisdetail), (pro) => pro.Zacisdetail.Contains(filterCondition.Zacisdetail))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zacpath), (pro) => pro.Zacpath.Contains(filterCondition.Zacpath))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZbusinessRecocation), (pro) => pro.ZbusinessRecocation.Contains(filterCondition.ZbusinessRecocation))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zacorgno), (pro) => pro.Zacorgno.Contains(filterCondition.Zacorgno))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zacid), (pro) => pro.Zacid.Contains(filterCondition.Zacid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zacparentid), (pro) => pro.Zacparentid.Contains(filterCondition.Zacparentid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zorgattr), (pro) => pro.Zorgattr.Contains(filterCondition.Zorgattr))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zorgchildattr), (pro) => pro.Zorgchildattr.Contains(filterCondition.Zorgchildattr))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zbbid), (pro) => pro.Zbbid.Contains(filterCondition.Zbbid))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Znbfyl), (pro) => pro.Znbfyl.Contains(filterCondition.Znbfyl))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zscenter), (pro) => pro.Zscenter.Contains(filterCondition.Zscenter))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zbrgzw), (pro) => pro.Zbrgzw.Contains(filterCondition.Zbrgzw))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zbrhw), (pro) => pro.Zbrhw.Contains(filterCondition.Zbrhw))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zcwygl), (pro) => pro.Zcwygl.Contains(filterCondition.Zcwygl))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZcwyglRea), (pro) => pro.ZcwyglRea.Contains(filterCondition.ZcwyglRea))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zhte), (pro) => pro.Zhte.Contains(filterCondition.Zhte))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zcontinentcode), (pro) => pro.Zcontinentcode.Contains(filterCondition.Zcontinentcode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zrpnature), (pro) => pro.Zrpnature.Contains(filterCondition.Zrpnature))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZhInOut), (pro) => pro.ZhInOut.Contains(filterCondition.ZhInOut))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zsystem), (pro) => pro.Zsystem.Contains(filterCondition.Zsystem))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZviewFlag), (pro) => pro.ZviewFlag.Contains(filterCondition.ZviewFlag))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zdelmap), (pro) => pro.Zdelmap.Contains(filterCondition.Zdelmap))
                //.WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                //.WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                //.Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new DHAdjustAccountsMultipleOrg
                {
                    Id = cc.Id,
                    MdmCode = cc.MdmCode,
                    ZztshnameLoc = cc.ZztshnameLoc,
                    ZztshnameEn = cc.ZztshnameEn,
                    ZztshnameChs = cc.ZztshnameChs,
                    ZztnameZh = cc.ZztnameZh,
                    ZztnameLoc = cc.ZztnameLoc,
                    Zacpath = cc.Zacpath,
                    ZaccountDate = cc.ZaccountDate,
                    Zacdisableyear = cc.Zacdisableyear,
                    Zacid = cc.Zacid,
                    Zacisdetail = cc.Zacisdetail,
                    Zacjthbfwn = cc.Zacjthbfwn,
                    Zaclayer = cc.Zaclayer,
                    ZacnameChs = cc.ZacnameChs,
                    ZacnameEn = cc.ZacnameEn,
                    ZacnameLoc = cc.ZacnameLoc,
                    Zaco = cc.Zaco,
                    Zacorgno = cc.Zacorgno,
                    Zacparentcode = cc.Zacparentcode,
                    Zacparentid = cc.Zacparentid,
                    ZacshortnameChs = cc.ZacshortnameChs,
                    ZacshortnameEn = cc.ZacshortnameEn,
                    ZacshortnameLoc = cc.ZacshortnameLoc,
                    Zacsortorder = cc.Zacsortorder,
                    Zaorgstate = cc.Zaorgstate,
                    ZapprovalOrg = cc.ZapprovalOrg,
                    Zbbid = cc.Zbbid,
                    Zbrgzw = cc.Zbrgzw,
                    Zbrhw = cc.Zbrhw,
                    Zbtid = cc.Zbtid,
                    ZbusinessRecocation = cc.ZbusinessRecocation,
                    ZbusinessUnit = cc.ZbusinessUnit,
                    Zcontinentcode = cc.Zcontinentcode,
                    Zcwygl = cc.Zcwygl,
                    ZcwyglRea = cc.ZcwyglRea,
                    Zcyname = cc.Zcyname,
                    Zdcid = cc.Zdcid,
                    Zdelmap = cc.Zdelmap,
                    ZdelRea = cc.ZdelRea,
                    Zgpoid = cc.Zgpoid,
                    ZhInOut = cc.ZhInOut,
                    Zhte = cc.Zhte,
                    Zivflgid = cc.Zivflgid,
                    Znbfyl = cc.Znbfyl,
                    Zorgattr = cc.Zorgattr,
                    Zorgchildattr = cc.Zorgchildattr,
                    Zorgloc = cc.Zorgloc,
                    Zregional = cc.Zregional,
                    Zremark = cc.Zremark,
                    ZreportFlag = cc.ZreportFlag,
                    ZreportNode = cc.ZreportNode,
                    ZreportTime = cc.ZreportTime,
                    Zrpnature = cc.Zrpnature,
                    Zrule = cc.Zrule,
                    Zscenter = cc.Zscenter,
                    Zsystem = cc.Zsystem,
                    Ztaxmethod = cc.Ztaxmethod,
                    ZtaxOrganization = cc.ZtaxOrganization,
                    ZtaxpayerCategory = cc.ZtaxpayerCategory,
                    Ztorg = cc.Ztorg,
                    Ztreeid = cc.Ztreeid,
                    Ztreever = cc.Ztreever,
                    Ztrno = cc.Ztrno,
                    Zuname = cc.Zuname,
                    ZviewFlag = cc.ZviewFlag,
                    Zyorgstate = cc.Zyorgstate,
                    Zzcurrency = cc.Zzcurrency,
                    ZztnameEn = cc.ZztnameEn
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            //值域
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => !string.IsNullOrWhiteSpace(t.ZDOM_CODE))
                .Select(t => new VDomainRespDto { ZDOM_CODE = t.ZDOM_CODE, ZDOM_DESC = t.ZDOM_DESC, ZDOM_VALUE = t.ZDOM_VALUE, ZDOM_NAME = t.ZDOM_NAME, ZDOM_LEVEL = t.ZDOM_LEVEL })
                .ToListAsync();

            //国家地区
            var countrysKey = ccList.Select(x => x.Zcyname).ToList();
            var country = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1 && countrysKey.Contains(t.ZCOUNTRYCODE))
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZCOUNTRYCODE, Name = t.ZCOUNTRYNAME })
                .ToListAsync();

            //行政区划
            var locationKey = ccList.Select(x => x.Zorgloc).ToList();
            var location = await _dbContext.Queryable<AdministrativeDivision>()
                .Where(t => t.IsDelete == 1 && locationKey.Contains(t.ZADDVSCODE))
                .Select(t => new CountryRegionOrAdminDivisionDto { Code = t.ZADDVSCODE, Name = t.ZADDVSNAME })
                .ToListAsync();

            //币种
            var cIds = ccList.Select(t => t.Zzcurrency).ToList();
            var currency = await _dbContext.Queryable<Currency>()
                .Where(t => t.IsDelete == 1 && cIds.Contains(t.ZCURRENCYCODE))
                .Select(t => new { t.ZCURRENCYCODE, t.ZCURRENCYNAME })
                .ToListAsync();

            foreach (var item in ccList)
            {
                //机构状态
                item.Zaorgstate = valDomain.FirstOrDefault(x => item.Zaorgstate == x.ZDOM_VALUE && x.ZDOM_CODE == "ZORGSTATE")?.ZDOM_NAME;

                //国家地区
                item.Zcyname = GetCountryRegion(item.Zcyname, country);

                //项目所在地
                item.Zorgloc = GetAdministrativeDivision(item.Zorgloc, location);

                //地域属性
                item.Zorgloc = valDomain.FirstOrDefault(x => item.Zorgloc == x.ZDOM_VALUE && x.ZDOM_CODE == "RegionalAttr")?.ZDOM_NAME;

                //币种
                item.Zzcurrency = currency.FirstOrDefault(x => x.ZCURRENCYCODE == item.Zzcurrency)?.ZCURRENCYNAME;

                //计税方式
                item.Ztaxmethod = GetValueDomain(item.Ztaxmethod, valDomain, "ZTAXMETHOD");

                //所属税务组织
                //item.ZtaxOrganization = GetValueDomain(item.ZtaxOrganization, valDomain, "ZTAXMETHOD");



            }

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
                    ZZCURRENCY = cc.ZZCURRENCY,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            CurrencyDetailsDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<CurrencyDetailsDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<Currency>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.State), (pro) => pro.ZSTATE.Contains(filterCondition.State))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Name), (pro) => pro.ZCURRENCYNAME.Contains(filterCondition.Name))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Code), (pro) => pro.ZCURRENCYCODE.Contains(filterCondition.Code))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.LetterCode), (pro) => pro.ZCURRENCYALPHABET.Contains(filterCondition.LetterCode))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.StandardName), (pro) => pro.STANDARDNAMEE.Contains(filterCondition.StandardName))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Remark), (pro) => pro.ZREMARKS.Contains(filterCondition.Remark))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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
                    Version = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
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

            //过滤条件
            ValueDomainReceiveResponseDto filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<ValueDomainReceiveResponseDto>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<ValueDomain>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZDOM_CODE), (pro) => pro.ZDOM_CODE.Contains(filterCondition.ZDOM_CODE))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZDOM_DESC), (pro) => pro.ZDOM_DESC.Contains(filterCondition.ZDOM_DESC))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZDOM_NAME), (pro) => pro.ZDOM_NAME.Contains(filterCondition.ZDOM_NAME))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZDOM_SUP), (pro) => pro.ZDOM_SUP.Contains(filterCondition.ZDOM_SUP))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZDOM_VALUE), (pro) => pro.ZDOM_VALUE.Contains(filterCondition.ZDOM_VALUE))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZREMARKS), (pro) => pro.ZREMARKS.Contains(filterCondition.ZREMARKS))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
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
                    ZVERSION = cc.ZVERSION,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 虚拟项目DH
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<DHVirtualProject>>> GetDHVirtualProjectAsync(FilterCondition requestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<List<DHVirtualProject>>();
            RefAsync<int> total = 0;

            //过滤条件
            DHVirtualProject filterCondition = new();
            if (!string.IsNullOrWhiteSpace(requestDto.FilterConditionJson))
            {
                filterCondition = JsonConvert.DeserializeObject<DHVirtualProject>(requestDto.FilterConditionJson);
            }

            var ccList = await _dbContext.Queryable<DHVirtualProject>()
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.IsDelete.ToString()), (pro) => pro.IsDelete.ToString().Contains(filterCondition.IsDelete.ToString()))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreatedAt), (pro) => pro.CreatedAt.Contains(filterCondition.CreatedAt))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdatedAt), (pro) => pro.UpdatedAt.Contains(filterCondition.UpdatedAt))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zversion), (pro) => pro.Zversion.Contains(filterCondition.Zversion))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZACORGNO), (pro) => pro.ZACORGNO.Contains(filterCondition.ZACORGNO))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Zdelete), (pro) => pro.Zdelete.Contains(filterCondition.Zdelete))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.Z2NDORG), (pro) => pro.Z2NDORG.Contains(filterCondition.Z2NDORG))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZPSTATE), (pro) => pro.ZPSTATE.Contains(filterCondition.ZPSTATE))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZVTPROJN), (pro) => pro.ZVTPROJN.Contains(filterCondition.ZVTPROJN))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.ZVTPROJ), (pro) => pro.ZVTPROJ.Contains(filterCondition.ZVTPROJ))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.CreateTime.ToString()), (pro) => Convert.ToDateTime(pro.CreateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.CreateTime).ToString("yyyy-MM-dd"))
                .WhereIF(filterCondition != null && !string.IsNullOrWhiteSpace(filterCondition.UpdateTime.ToString()), (pro) => Convert.ToDateTime(pro.UpdateTime).ToString("yyyy-MM-dd") == Convert.ToDateTime(filterCondition.UpdateTime).ToString("yyyy-MM-dd"))
                .Where((cc) => cc.IsDelete == 1)
                .Select((cc) => new DHVirtualProject
                {
                    Id = cc.Id,
                    IsDelete = cc.IsDelete,
                    CreatedAt = cc.CreatedAt,
                    UpdatedAt = cc.UpdatedAt,
                    Zversion = cc.Zversion,
                    Z2NDORG = cc.Z2NDORG,
                    ZACORGNO = cc.ZACORGNO,
                    Zdelete = cc.Zdelete,
                    ZPSTATE = cc.ZPSTATE,
                    ZVTPROJ = cc.ZVTPROJ,
                    ZVTPROJN = cc.ZVTPROJN,
                    CreateTime = cc.CreateTime,
                    UpdateTime = cc.UpdateTime
                })
                .ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(ccList);
            return responseAjaxResult;
        }

        #region 条件筛选格式数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table">表</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<FilterConditionDto>>> GetUserFilterColumnsAsync(int table)
        {
            ResponseAjaxResult<List<FilterConditionDto>> responseAjaxResult = new();
            List<FilterConditionDto> options = new();

            //获取表所有列
            List<string> tableColumns = new();
            //整合所有需要筛选的列
            List<string> allColumns = new();

            #region 基础数据
            //值域数据
            var valDomain = await _dbContext.Queryable<ValueDomain>()
                .Where(t => t.IsDelete == 1)
                .Select(t => new FilterChildData { Code = t.ZDOM_CODE, Desc = t.ZDOM_DESC, Key = t.ZDOM_VALUE, Val = t.ZDOM_NAME })
                .ToListAsync();

            //国家
            var countrys = await _dbContext.Queryable<CountryRegion>()
                .Where(t => t.IsDelete == 1)
                .Select(t => new FilterChildData { Key = t.ZCOUNTRYCODE, Val = t.ZCOUNTRYNAME })
                .ToListAsync();

            //省、市、县
            var adisision = await _dbContext.Queryable<AdministrativeDivision>()
                .Where(t => t.IsDelete == 1)
                .Select(t => new FilterChildData { Key = t.ZADDVSCODE, Val = t.ZADDVSNAME, Code = t.ZADDVSLEVEL })
                .ToListAsync();

            //机构业务类型
            var institutionBusType = await _dbContext.Queryable<InstitutionBusinessType>()
                .Where(t => t.IsDelete == 1)
                .Select(t => new FilterChildData { Key = t.Code, Val = t.Name })
                .ToListAsync();

            #endregion

            #region 查询初始化

            switch (table)
            {
                case 1:
                    allColumns = new List<string> { "SEX", "Enable", "Nationality", "Nation", "EmpSort", "Birthday", "EntryTime", "EmpSort", "PositionGradeNorm", "HighEstGrade", "SameHighEstGrade", "PoliticsFace", "UserInfoStatus", "CreateTime", "UpdateTime", "" };
                    var properties = GetProperties<UserSearchDetailsDto>();
                    foreach (var property in properties) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;
                            if (item.Contains("Birthday") || item.Contains("EntryTime"))
                            {
                                columnName = item == "Birthday" ? "出生日期" : "本企业入职时间";
                                type = "Time";//时间
                            }
                            else if (item.Contains("CountryRegion") || item.Contains("Nationality"))
                            {
                                type = "Single";
                                columnName = "国家/地区";
                                optionsChild = countrys;
                            }
                            else if (item.Contains("SEX"))
                            {
                                columnName = "性别";
                                type = "Single";//单选
                                optionsChild = valDomain.Where(x => x.Code == "ZGENDER").ToList();
                            }
                            else if (item.Contains("Enable"))
                            {
                                columnName = "启用状态";
                                type = "Single";//单选
                                optionsChild = valDomain.Where(x => x.Code == "ZPSTATE").ToList();
                            }
                            else if (item.Contains("EmpSort"))
                            {
                                columnName = "用工类型";
                                type = "Single";//单选
                                optionsChild = valDomain.Where(x => x.Code == "ZEMPTYPE").ToList();
                            }
                            else if (item.Contains("UserInfoStatus"))
                            {
                                columnName = "员工状态";
                                type = "Single";//单选
                                optionsChild = valDomain.Where(x => x.Code == "ZEMPSTATE").ToList();
                            }
                            else if (item.Contains("Nation"))
                            {
                                columnName = "民族";
                                type = "Single";//
                                optionsChild = valDomain.Where(x => x.Code == "ZNATION").ToList();
                            }
                            else if (item.Contains("PoliticsFace"))
                            {
                                columnName = "政治面貌（新版）";
                                type = "Single";//
                                //optionsChild = valDomain.Where(x => x.Code == "ZNATION").ToList();
                            }
                            else if (item.Contains("PositionGradeNorm") || item.Contains("HighEstGrade") || item.Contains("SameHighEstGrade"))
                            {
                                columnName = item == "PositionGradeNorm" ? "职级（新版）" : item == "HighEstGrade" ? "新版最高职级（新版）" : "统一的最高职级（新版）";
                                type = "Single";//
                                optionsChild = valDomain.Where(x => x.Code == "ZJOBTYPE").ToList();
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 2:
                    allColumns = new List<string> { "EntClass", "Status", "BizType", "Type", "TypeExt", "OrgProvince", "Carea", "TerritoryPro", "ShareHoldings", "IsIndependent", "Mrut", "ProjectScale", "ProjectManType", "ProjectType", "CreateTime", "UpdateTime", "StartDate" };
                    var properties2 = GetProperties<InstitutionDetatilsDto>();
                    foreach (var property in properties2) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("EntClass"))
                            {
                                type = "Single";
                                columnName = "企业分类";
                                optionsChild = valDomain.Where(x => x.Code == "ZENTC").ToList();
                            }
                            else if (item.Contains("Status"))
                            {
                                columnName = "机构状态";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZORGSTATE").ToList();
                            }
                            else if (item.Contains("ProjectScale"))
                            {
                                columnName = "项目规模";
                                type = "Single";
                                //optionsChild = valDomain.Where(x => x.Code == "ZORGSTATE").ToList();
                            }
                            else if (item.Contains("ProjectManType"))
                            {
                                columnName = "项目管理类型";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZMANAGE_MODE").ToList();
                            }
                            else if (item.Contains("ProjectType"))
                            {
                                columnName = "项目类型";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZPROJTYPE").ToList();
                            }
                            else if (item.Contains("BizType"))
                            {
                                columnName = "机构业务类型";
                                type = "Single";
                                optionsChild = institutionBusType;
                            }
                            else if (item.Contains("Type"))
                            {
                                columnName = "机构属性";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZORGATTR").ToList();
                            }
                            else if (item.Contains("TypeExt"))
                            {
                                columnName = "机构子属性";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZORGCHILDATTR").ToList();
                            }
                            else if (item.Contains("OrgProvince"))
                            {
                                columnName = "机构所在地";
                                type = "Single";
                                optionsChild = adisision;
                            }
                            else if (item.Contains("Carea"))
                            {
                                columnName = "国家名称";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            //else if (item.Contains("TerritoryPro"))
                            //{
                            //    columnName = "地域属性";
                            //    type = "Single";
                            //    //optionsChild = valDomain.Where(x => x.Code == "ZSingleIND").ToList();
                            //}
                            else if (item.Contains("ShareHoldings"))
                            {
                                columnName = "持股情况";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZHOLDING").ToList();
                            }
                            else if (item.Contains("IsIndependent"))
                            {
                                columnName = "是否独立核算";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZCHECKIND").ToList();
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime") || item.Contains("Mrut") || item.Contains("StartDate"))
                            {
                                columnName = item == "Mrut" ? "最近更新时间" : item == "CreateTime" ? "创建时间" : item == "StartDate" ? "开始时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 3:
                    //allColumns = new List<string> { "PlanStartDate", "PlanCompletionDate", "TradingSituation", "StartDateOfInsure", "EndDateOfInsure", "Country", "FundEstablishmentDate", "FundExpirationDate", "LeaseStartDate", "DueDate", "TaxMethod", "OrgMethod", "ConsolidatedTable", "Type", "Location", "Invest", "PjectOrg", "ResponsibleParty", "Currency", "FundOrgForm", "FundManager", "TenantType", "UnitSec", "ReasonForDeactivate", "WinningBidder", "Management", "ParticipateInUnitSecs", "IsJoint", "CreateTime", "UpdateTime", "State" };
                    allColumns = new List<string> { "ZPROJTYPE", "ZZCOUNTRY", "ZPROJLOC", "ZCPBC", "ZINVERSTOR", "ZAPVLDATE", "ZSI", "FundExpirationDate", "ZPRO_ORG", "ZSTARTDATE", "ZFINDATE", "ZRESP", "ZLDLOCGT", "ZCBR", "ZTRADER", "ZISTARTDATE", "ZIFINDATE", "ZZCURRENCY", "ZFUNDORGFORM", "ZPRO_BP", "ZFUNDMTYPE", "ZFSTARTDATE", "ZFFINDATE", "ZLESSEETYPE", "ZLSTARTDATE", "ZLFINDATE", "Z2NDORG", "ZTAXMETHOD", "ZPOS", "ZAWARDMAI", "ZCS", "ZSTATE", "Zdelete", "CreatedAt", "UpdatedAt", "FZmanagemode", "FZwinningc", "", "", "", "", "" };
                    //币种
                    var currency = await _dbContext.Queryable<Currency>()
                        .Where(t => t.IsDelete == 1)
                        .Select(t => new FilterChildData { Key = t.ZCURRENCYCODE, Val = t.ZCURRENCYNAME })
                        .ToListAsync();

                    //机构
                    var institutions = await _dbContext.Queryable<Institution>()
                        .Where(t => t.IsDelete == 1)
                        .Select(t => new FilterChildData { Key = t.OID, Val = t.NAME })
                        .ToListAsync();

                    //行政组织-多组织   责任主体
                    var zrzt = await _dbContext.Queryable<AdministrativeOrganization>()
                        .Where(t => t.IsDelete == 1)
                        .Select(t => new FilterChildData { Key = t.MDM_CODE, Val = t.ZZTNAME_ZH })
                        .ToListAsync();

                    //中标主体  往来单位
                    //只要局的
                    var oids = await _dbContext.Queryable<Institution>().Select(t => t.OID).ToListAsync();
                    var zbzt = await _dbContext.Queryable<CorresUnit>()
                        .Where(t => oids.Contains(t.Z2NDORG))
                        .Select(t => new FilterChildData { Key = t.ZBP, Val = t.ZBPNAME_ZH })
                        .ToListAsync();

                    //var properties3 = GetProperties<ProjectDetailsDto>();
                    var properties3 = GetProperties<DHProjects>();
                    foreach (var property in properties3) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("ZSTARTDATE"))
                            {
                                type = "NumberTime";
                                columnName = "项目计划开始日期";
                            }
                            else if (item.Contains("State"))
                            {
                                columnName = "状态";
                                type = "Single";//单选
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "停用" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "启用" });
                            }
                            else if (item.Contains("ZFINDATE"))
                            {
                                columnName = "项目计划完成日期";
                                type = "NumberTime";
                            }
                            else if (item.Contains("ZPROJTYPE"))
                            {
                                columnName = "项目类型";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZPROJTYPE").ToList();
                            }
                            else if (item.Contains("ZPROJLOC"))
                            {
                                columnName = "项目所在地";
                                type = "Single";
                                optionsChild = adisision.Where(x => x.Code == "3" || x.Code == "4").ToList();
                            }
                            else if (item.Contains("ZPRO_ORG"))
                            {
                                columnName = "项目机构";
                                type = "Single";
                                optionsChild = institutions.ToList();
                            }
                            else if (item.Contains("ZPRO_BP"))
                            {
                                columnName = "商机项目机构";
                                type = "Single";
                                optionsChild = institutions.ToList();
                            }
                            else if (item.Contains("ZRESP"))
                            {
                                columnName = "责任主体";
                                type = "Single";
                                optionsChild = zrzt;
                            }
                            else if (item.Contains("FZmanagemode"))
                            {
                                columnName = "项目管理方式";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZMANAGE_MODE").ToList();
                            }
                            else if (item.Contains("ZCPBC"))
                            {
                                columnName = "中交项目业务分类";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZCPBC").ToList();
                            }
                            else if (item.Contains("ZAPVLDATE"))
                            {
                                columnName = "项目批复/决议时间";
                                type = "NumberTime";
                            }
                            else if (item.Contains("ZINVERSTOR"))
                            {
                                columnName = "投资主体";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZINVERSTOR").ToList();
                            }
                            else if (item.Contains("ZSI"))
                            {
                                columnName = "收入来源";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZSI").ToList();
                            }
                            else if (item.Contains("ZLDLOCGT"))
                            {
                                columnName = "土地成交确认书获取时间";
                                type = "Time";
                            }
                            else if (item.Contains("ZCBR"))
                            {
                                columnName = "工商变更时间";
                                type = "Time";
                            }
                            else if (item.Contains("ZAWARDMAI"))
                            {
                                columnName = "中标主体";
                                type = "Single";
                                optionsChild = zbzt;
                            }
                            else if (item.Contains("ZFUNDORGFORM"))
                            {
                                columnName = "基金组织形式";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZFUNDORGFORM").ToList();
                            }
                            else if (item.Contains("ZFUNDMTYPE"))
                            {
                                columnName = "基金管理人类型";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZFUNDMTYPE").ToList();
                            }
                            else if (item.Contains("ZLESSEETYPE"))
                            {
                                columnName = "承租人类型";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZLESSEETYPE").ToList();
                            }
                            else if (item.Contains("ZSTOPREASON"))
                            {
                                columnName = "停用原因";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZSTOPREASON").ToList();
                            }
                            else if (item.Contains("FZcy2ndorg"))
                            {
                                columnName = "参与二级单位";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZCY2NDORG").ToList();
                            }
                            else if (item.Contains("Z2NDORG"))
                            {
                                columnName = "所属二级单位";
                                type = "Single";
                                optionsChild = institutions;
                            }
                            else if (item.Contains("ZZCURRENCY"))
                            {
                                columnName = "币种";
                                type = "Single";
                                optionsChild = currency;
                            }
                            else if (item.Contains("FZwinningc"))
                            {
                                columnName = "是否联合体项目";
                                type = "Single";//单选
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("ZSTATE"))
                            {
                                columnName = "状态";
                                type = "Single";//单选
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "停用" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "启用" });
                            }
                            else if (item.Contains("Zdelete"))
                            {
                                columnName = "是否删除";
                                type = "Single";//单选
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "已删除" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "未删除" });
                            }
                            else if (item.Contains("ZTRADER"))
                            {
                                columnName = "操盘情况";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZTRADER").ToList();
                            }
                            else if (item.Contains("ZISTARTDATE"))
                            {
                                columnName = "保险起始日期";
                                type = "NumberTime";
                            }
                            else if (item.Contains("ZIFINDATE"))
                            {
                                columnName = "保险终止日期";
                                type = "NumberTime";
                            }
                            else if (item.Contains("ZZCOUNTRY"))
                            {
                                columnName = "国家/地区";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            else if (item.Contains("ZFSTARTDATE"))
                            {
                                columnName = "基金成立日期";
                                type = "NumberTime";
                            }
                            else if (item.Contains("ZFFINDATE"))
                            {
                                columnName = "基金到期日期";
                                type = "NumberTime";
                            }
                            else if (item.Contains("ZLSTARTDATE"))
                            {
                                columnName = "起租日期";
                                type = "NumberTime";
                            }
                            else if (item.Contains("ZLFINDATE"))
                            {
                                columnName = "到期日期";
                                type = "NumberTime";
                            }
                            else if (item.Contains("ZTAXMETHOD"))
                            {
                                columnName = "计税方式";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZTAXMETHOD").ToList();
                            }
                            else if (item.Contains("ZPOS"))
                            {
                                columnName = "项目组织形式";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZPOS").ToList();
                            }
                            else if (item.Contains("ZCS"))
                            {
                                columnName = "并表情况";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZCS").ToList();
                            }
                            else if (item.Contains("CreatedAt") || item.Contains("UpdatedAt"))
                            {
                                columnName = item == "CreatedAt" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 4:
                    allColumns = new List<string> { "CategoryUnit", "Country", "Province", "City", "County", "EnterpriseNature", "TypeOfUnit", "ChangeTime", "CreateTime", "UpdateTime", "StatusOfUnit" };
                    var properties4 = GetProperties<CorresUnitDetailsDto>();
                    foreach (var property in properties4) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("CategoryUnit"))
                            {
                                type = "Single";
                                columnName = "往来单位类别";
                                optionsChild = valDomain.Where(x => x.Code == "ZBPTYPE").ToList();
                            }
                            else if (item.Contains("Province"))
                            {
                                columnName = "省";
                                type = "Single";
                                optionsChild = adisision.Where(x => x.Code == "1").ToList();
                            }
                            else if (item.Contains("Country"))
                            {
                                columnName = "国家/地区";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            else if (item.Contains("City"))
                            {
                                columnName = "市";
                                type = "Single";
                                optionsChild = adisision.Where(x => x.Code == "2").ToList();
                            }
                            else if (item.Contains("County"))
                            {
                                columnName = "县/区";
                                type = "Single";
                                optionsChild = adisision.Where(x => x.Code == "3" || x.Code == "4").ToList();
                            }
                            else if (item.Contains("EnterpriseNature"))
                            {
                                columnName = "企业性质";
                                type = "Single";
                            }
                            else if (item.Contains("TypeOfUnit"))
                            {
                                columnName = "往来单位类型";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZBPKINDS").ToList();
                            }
                            else if (item.Contains("ChangeTime"))
                            {
                                columnName = "修改时间";
                                type = "NumberTime";
                            }
                            else if (item.Contains("StatusOfUnit"))
                            {
                                columnName = "往来单位状态";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZBPSTATE").ToList();
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 5:
                    allColumns = new List<string> { "RoadGuoZiW", "RoadHaiW", "RoadGongJ", "CreateTime", "UpdateTime", };
                    var properties5 = GetProperties<CountryRegionDetailsDto>();
                    foreach (var property in properties5) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("RoadGuoZiW"))
                            {
                                type = "Single";//复选
                                columnName = " 一带一路(国资委)";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("RoadHaiW"))
                            {
                                columnName = "一带一路(海外)";
                                type = "Single";//单选
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("RoadGongJ"))
                            {
                                columnName = "一带一路(共建)";
                                type = "Single";//单选
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 6:
                    allColumns = new List<string> { "CreateTime", "UpdateTime" };
                    var properties6 = GetProperties<CountryContinentDetailsDto>();
                    foreach (var property in properties6) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;
                            if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 7:
                    allColumns = new List<string> { "Country", "Province", "City", "County", "CreateTime", "UpdateTime", };
                    var properties7 = GetProperties<FinancialInstitutionDetailsDto>();
                    foreach (var property in properties7) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("Country"))
                            {
                                columnName = "国家/地区";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            else if (item.Contains("Province"))
                            {
                                columnName = "省";
                                type = "Single";
                                optionsChild = adisision.Where(x => x.Code == "1").ToList();
                            }
                            else if (item.Contains("City"))
                            {
                                columnName = "市";
                                type = "Single";
                                optionsChild = adisision.Where(x => x.Code == "2").ToList();
                            }
                            else if (item.Contains("County"))
                            {
                                columnName = "县/区";
                                type = "Single";
                                optionsChild = adisision.Where(x => x.Code == "3" || x.Code == "4").ToList();
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 8:
                    allColumns = new List<string> { "Level", "CreateTime", "UpdateTime", };
                    var properties8 = GetProperties<DeviceClassCodeDetailsDto>();
                    foreach (var property in properties8) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("Level"))
                            {
                                type = "Single";//复选
                                columnName = "分类层级";
                                optionsChild = valDomain.Where(x => x.Code == "ZCLEVEL").ToList();
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 9:
                    allColumns = new List<string> { "CreateTime", "UpdateTime" };
                    var properties9 = GetProperties<InvoiceTypeDetailshDto>();
                    foreach (var property in properties9) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;
                            if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 10:
                    allColumns = new List<string> { "Fzkpstate", "Fzmajortype", "", "Fzstate", "Fzpstartdate", "Fzpfindate", "CreatedAt", "UpdatedAt", "Fzoutsourcing", "Fzsrpclass" };
                    //allColumns = new List<string> { "IsHighTech", "PjectState", "IsOutsourced", "TypeCode", "PlanStartDate", "PlanEndDate", "State", "CreateTime", "UpdateTime", };
                    //var properties10 = GetProperties<ScientifiCNoProjectDetailsDto>();
                    var properties10 = GetProperties<DHResearch>();
                    foreach (var property in properties10) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("Fzhitech"))
                            {
                                type = "Single";
                                columnName = "是否高新项目";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("Fzkpstate"))
                            {
                                columnName = "项目状态";
                                type = "Single";//单选
                                optionsChild = valDomain.Where(x => x.Code == "ZKPSTATE").ToList();
                            }
                            else if (item.Contains("Fzmajortype"))
                            {
                                columnName = "专业类型";
                                type = "Single";//单选
                                optionsChild = valDomain.Where(x => x.Code == "ZMAJORTYPE").ToList();
                            }
                            else if (item.Contains("Fzoutsourcing"))
                            {
                                columnName = "是否委外项目";
                                type = "Single";//单选
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("Fzsrpclass"))
                            {
                                columnName = "科研项目分类";
                                type = "Single";//单选
                                optionsChild = valDomain.Where(x => x.Code == "ZSRPCLASS").ToList();
                            }
                            else if (item.Contains("Fzpstartdate"))
                            {
                                columnName = "计划开始日期";
                                type = "NumberTime";
                            }
                            else if (item.Contains("Fzpfindate"))
                            {
                                columnName = "计划结束日期";
                                type = "NumberTime";
                            }
                            else if (item.Contains("Fzstate"))
                            {
                                columnName = "状态";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "停用" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "启用" });
                            }
                            else if (item.Contains("CreatedAt") || item.Contains("UpdatedAt"))
                            {
                                columnName = item == "CreatedAt" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 11:
                    allColumns = new List<string> { "CreateTime", "UpdateTime" };
                    var properties11 = GetProperties<LanguageDetailsDto>();
                    foreach (var property in properties11) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;
                            if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 12:
                    allColumns = new List<string> { "Country", "Province", "City", "County", "CreateTime", "UpdateTime", };
                    var properties12 = GetProperties<BankCardDetailsDto>();
                    foreach (var property in properties12) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("Country"))
                            {
                                columnName = "国家/地区";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            else if (item.Contains("Province"))
                            {
                                columnName = "省";
                                type = "Single";
                                optionsChild = adisision.Where(x => x.Code == "1").ToList();
                            }
                            else if (item.Contains("City"))
                            {
                                columnName = "市";
                                type = "Single";
                                optionsChild = adisision.Where(x => x.Code == "2").ToList();
                            }
                            else if (item.Contains("County"))
                            {
                                columnName = "县/区";
                                type = "Single";
                                optionsChild = adisision.Where(x => x.Code == "3" || x.Code == "4").ToList();
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 13:
                    allColumns = new List<string> { "IsCode", "CreateTime", "UpdateTime", };
                    var properties13 = GetProperties<DeviceDetailCodeDetailsDto>();
                    foreach (var property in properties13) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("IsCode"))
                            {
                                columnName = "是否常用编码";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 14:
                    allColumns = new List<string> { "Zdatstate", "", "", };
                    //allColumns = new List<string> { "State", "CreateTime", "UpdateTime", };
                    //var properties14 = GetProperties<AccountingDepartmentDetailsDto>();
                    var properties14 = GetProperties<DHAccountingDept>();
                    foreach (var property in properties14) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("Zdatstate"))
                            {
                                columnName = "核算部门停用标志";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "未停用" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "停用" });
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 15:
                    allColumns = new List<string> { "CreateTime", "UpdateTime" };
                    var properties15 = GetProperties<RelationalContractsDetailsDto>();
                    foreach (var property in properties15) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;
                            if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 16:
                    allColumns = new List<string> { "State", "CreateTime", "UpdateTime", };
                    var properties16 = GetProperties<RegionalDetailsDto>();
                    foreach (var property in properties16) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("State"))
                            {
                                columnName = "状态";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "无效" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "有效" });
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 17:
                    allColumns = new List<string> { "State", "CreateTime", "UpdateTime", };
                    var properties17 = GetProperties<UnitMeasurementDetailsDto>();
                    foreach (var property in properties17) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("State"))
                            {
                                columnName = "状态";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "无效" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "有效" });
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 18:
                    allColumns = new List<string> { "ThirdNewBType", "CreateTime", "UpdateTime" };
                    //var properties18 = GetProperties<ProjectClassificationDetailsDto>();
                    var properties18 = GetProperties<ProjectClassificationDetailsDto>();
                    foreach (var property in properties18) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("ThirdNewBType"))
                            {
                                columnName = "三新业务类型";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 19:
                    allColumns = new List<string> { "State", "CreateTime", "UpdateTime", };
                    var properties19 = GetProperties<RegionalCenterDetailsDto>();
                    foreach (var property in properties19) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("State"))
                            {
                                columnName = "状态";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "无效" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "有效" });
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 20:
                    allColumns = new List<string> { "State", "CreateTime", "UpdateTime", };
                    var properties20 = GetProperties<NationalEconomyDetailsDto>();
                    foreach (var property in properties20) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("State"))
                            {
                                columnName = "状态";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "无效" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "有效" });
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 21:
                    //allColumns = new List<string> { "CreateTime", "UpdateTime" };
                    allColumns = new List<string> { "CreatedAt", "UpdatedAt", "Fzstate" };
                    //var properties21 = GetProperties<AdministrativeAccountingMapperDetailsDto>();
                    var properties21 = GetProperties<DHAdministrative>();
                    foreach (var property in properties21) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;
                            if (item.Contains("CreatedAt") || item.Contains("UpdatedAt"))
                            {
                                columnName = item == "CreatedAt" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            if (item.Contains("Zdatstate"))
                            {
                                columnName = "停用标志";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "未停用" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "停用" });
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 22:
                    allColumns = new List<string> { "OrgAttr", "OrgChildAttr", "OrgStatus", "LocationOfOrg", "Country", "RegionalAttr", "Shareholding", "CreateTime", "UpdateTime", };
                    var properties22 = GetProperties<EscrowOrganizationDetailsDto>();
                    foreach (var property in properties22) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("Country"))
                            {
                                columnName = "国家/地区";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            else if (item.Contains("OrgAttr"))
                            {
                                columnName = "机构属性";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZORGATTR").ToList();
                            }
                            else if (item.Contains("OrgChildAttr"))
                            {
                                columnName = "机构子属性";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZORGCHILDATTR").ToList();
                            }
                            else if (item.Contains("OrgStatus"))
                            {
                                columnName = "机构状态";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZORGSTATE").ToList();
                            }
                            else if (item.Contains("LocationOfOrg"))
                            {
                                columnName = "机构状态";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZORGSTATE").ToList(); ;
                            }
                            else if (item.Contains("RegionalAttr"))
                            {
                                columnName = "地域属性";
                                type = "Single";
                                //optionsChild = valDomain.Where(x => x.Code == "1").ToList();
                            }
                            else if (item.Contains("Shareholding"))
                            {
                                columnName = "持股情况";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZHOLDING").ToList();
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 23:
                case 24:
                    //allColumns = new List<string> { "ZZCOUNTRY", "ZPROJLOC", "ZCPBC", "ZSFOLDATE", "ZORG", "Z2NDORG", "ZSTATE", "ZORG_QUAL", "ZTAXMETHOD", "Zdelete", "CreatedAt", "UpdatedAt", };
                    allColumns = new List<string> { "ZZCOUNTRY", "ZPROJLOC", "ZCPBC", "ZSFOLDATE", "ZORG", "Z2NDORG", "ZSTATE", "ZORG_QUAL", "ZTAXMETHOD", "Zdelete", "CreatedAt", "UpdatedAt", };
                    //var properties24 = GetProperties<BusinessNoCpportunityDetailsDto>();
                    //机构
                    var institutionss = await _dbContext.Queryable<Institution>()
                        .Where(t => t.IsDelete == 1)
                        .Select(t => new FilterChildData { Key = t.OID, Val = t.NAME })
                        .ToListAsync();
                    var properties24 = GetProperties<DHOpportunity>();
                    foreach (var property in properties24) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("ZZCOUNTRY"))
                            {
                                columnName = "国家/地区";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            if (item.Contains("ZPROJLOC"))
                            {
                                columnName = "项目所在地";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            if (item.Contains("ZCPBC"))
                            {
                                columnName = "中交项目业务分类";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZCPBC").ToList();
                            }
                            if (item.Contains("ZORG"))
                            {
                                columnName = "跟踪单位";
                                type = "Single";
                                optionsChild = institutionss;
                            }
                            if (item.Contains("Z2NDORG"))
                            {
                                columnName = "所属二级单位";
                                type = "Single";
                                optionsChild = institutionss;
                            }
                            if (item.Contains("ZORG_QUAL"))
                            {
                                columnName = "资质单位";
                                type = "Single";
                                optionsChild = institutionss;
                            }
                            if (item.Contains("ZTAXMETHOD"))
                            {
                                columnName = "计税方式";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZTAXMETHOD").ToList();
                            }
                            if (item.Contains("ZSFOLDATE"))
                            {
                                columnName = "开始跟踪日期";
                                type = "Time";
                            }
                            else if (item.Contains("Zdelete"))
                            {
                                columnName = "是否删除";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "已删除" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "未删除" });
                            }
                            else if (item.Contains("ZSTATE"))
                            {
                                columnName = "状态";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "停用" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "启用" });
                            }
                            else if (item.Contains("CreatedAt") || item.Contains("UpdatedAt"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 25:
                    allColumns = new List<string> { "State", "CreateTime", "UpdateTime", };
                    var properties25 = GetProperties<AdministrativeDivisionDetailsDto>();
                    foreach (var property in properties25) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("Country"))
                            {
                                columnName = "国家/地区";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            else if (item.Contains("State"))
                            {
                                columnName = "状态";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "停用" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "启用" });
                            }
                            else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 26:
                    //    allColumns = new List<string> { "ZIVFLGID", "ZREPORT_TIME", "ZAORGSTATE", "ZACDISABLEYEAR", "ZACISDETAIL", "ZCWYGL_REA", "ZHTE", "ZH_IN_OUT", "ZOSTATE", "ZCYNAME", "ZREGIONAL", "CreateTime", "UpdateTime", };
                    allColumns = new List<string> { "Zivflgid", "Zyorgstate", "Zaorgstate", "ZreportTime", "Zacdisableyear", "ZcwyglRea", "Zacisdetail", "", "Zhte", "ZhInOut", "Zcyname", "", "", "", };
                    //var properties26 = GetProperties<AccountingOrganizationDetailsDto>();
                    var properties26 = GetProperties<DHAdjustAccountsMultipleOrg>();
                    foreach (var property in properties26) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;

                            if (item.Contains("ZCYNAME"))
                            {
                                columnName = "国家/地区";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            else if (item.Contains("Zivflgid"))
                            {
                                columnName = "是否投资项目/公司";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("ZreportTime"))
                            {
                                columnName = "转入不启用报表时间";
                                type = "NumberTime";
                            }
                            else if (item.Contains("Zyorgstate"))
                            {
                                columnName = "机构状态";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZORGSTATE").ToList();
                            }
                            else if (item.Contains("Zacdisableyear"))
                            {
                                columnName = "停用日期";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("Zacisdetail"))
                            {
                                columnName = "是否明细";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("ZcwyglRea"))
                            {
                                columnName = "不启用财务云财务管理原因说明";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZD_DIS_CWY_REACODE").ToList();
                            }
                            else if (item.Contains("Zhte"))
                            {
                                columnName = "是否高新企业";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "否" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "是" });
                            }
                            else if (item.Contains("ZhInOut"))
                            {
                                columnName = "境内/境外";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "境内" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "境外" });
                            }
                            else if (item.Contains("Zaorgstate"))
                            {
                                columnName = "核算机构状态";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZD_MVIEW_ZY_ORGSTATE").ToList();
                            }
                            else if (item.Contains("Zcyname"))
                            {
                                columnName = "国家/地区";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            //else if (item.Contains("ZREGIONAL"))
                            //{
                            //    columnName = "地域属性";
                            //    type = "Single";
                            //    //optionsChild = valDomain.Where(x => x.Code == "1").ToList();
                            //}
                            //else if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            //{
                            //    columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                            //    type = "Time";//时间
                            //}
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 27:
                    allColumns = new List<string> { "State" };
                    var properties27 = GetProperties<CurrencyDetailsDto>();
                    foreach (var property in properties27) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;
                            if (item.Contains("State"))
                            {
                                columnName = "状态";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "无效" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "有效" });
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 28:
                    allColumns = new List<string> { "CreateTime", "UpdateTime" };
                    var properties28 = GetProperties<ValueDomainReceiveResponseDto>();
                    foreach (var property in properties28) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;
                            if (item.Contains("CreateTime") || item.Contains("UpdateTime"))
                            {
                                columnName = item == "CreateTime" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 29:
                    allColumns = new List<string> { "CreatedAt", "UpdatedAt", "Z2NDORG", "Zdelete", "ZPSTATE", "", "" };
                    //机构
                    var institutionsss = await _dbContext.Queryable<Institution>()
                        .Where(t => t.IsDelete == 1)
                        .Select(t => new FilterChildData { Key = t.OID, Val = t.NAME })
                        .ToListAsync();

                    var properties29 = GetProperties<DHVirtualProject>();
                    foreach (var property in properties29) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;
                            if (item.Contains("CreatedAt") || item.Contains("UpdatedAt"))
                            {
                                columnName = item == "CreatedAt" ? "创建时间" : "修改时间";
                                type = "Time";//时间
                            }
                            else if (item.Contains("Z2NDORG"))
                            {
                                columnName = "所属二级单位";
                                type = "Time";//时间
                                optionsChild = institutionsss;
                            }
                            else if (item.Contains("Zdelete"))
                            {
                                columnName = "是否删除";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "已删除" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "未删除" });
                            }
                            else if (item.Contains("ZPSTATE"))
                            {
                                columnName = "状态";
                                type = "Single";
                                optionsChild.Add(new FilterChildData { Key = "0", Val = "停用" });
                                optionsChild.Add(new FilterChildData { Key = "1", Val = "启用" });
                            }
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
                case 30:
                    //allColumns = new List<string> { "ZOSTATE", "", "ZOATTR", "ZOCATTR", "ZCYNAME", "ZORGLOC", "ZHOLDING", "ZCHECKIND", "ZENTC", "", "" };
                    allColumns = new List<string> { "Zoattr", "", "Zocattr", "Zcyname", "Zorgloc", "Zholding", "Zcheckind", "Zostate", "Zentc", "", "" };
                    //allColumns = new List<string> { "Zcyname", "Zorgloc", "Zregional", "Zostate", "", "", "" };

                    //var properties30 = GetProperties<AdministrativeOrganization>();
                    var properties30 = GetProperties<DHOrganzationDep>();
                    foreach (var property in properties30) { tableColumns.Add(property.Name); }
                    foreach (var item in tableColumns)
                    {
                        if (allColumns.Contains(item))
                        {
                            List<FilterChildData> optionsChild = new();
                            string type = string.Empty;
                            string columnName = string.Empty;
                            if (item.Contains("Zcyname"))
                            {
                                columnName = "国家名称";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            else if (item.Contains("Zorgloc"))
                            {
                                columnName = "机构所在地";
                                type = "Single";
                                optionsChild = countrys;
                            }
                            else if (item.Contains("Zostate"))
                            {
                                columnName = "机构状态";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZOSTATE").ToList();
                            }
                            else if (item.Contains("Zoattr"))
                            {
                                columnName = "机构属性";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZOATTR").ToList();
                            }
                            else if (item.Contains("Zocattr"))
                            {
                                columnName = "机构子属性";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZOCATTR").ToList();
                            }
                            else if (item.Contains("Zholding"))
                            {
                                columnName = "持股情况";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZHOLDING").ToList();
                            }
                            else if (item.Contains("Zcheckind"))
                            {
                                columnName = "是否独立核算";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZCHECKIND").ToList();
                            }
                            else if (item.Contains("Zentc"))
                            {
                                columnName = "企业分类代码";
                                type = "Single";
                                optionsChild = valDomain.Where(x => x.Code == "ZENTC").ToList();
                            }
                            //if (item.Contains("ZCYNAME"))
                            //{
                            //    columnName = "国家名称";
                            //    type = "Single";
                            //    optionsChild = countrys;
                            //}
                            //else if (item.Contains("ZORGLOC"))
                            //{
                            //    columnName = "机构所在地";
                            //    type = "Single";
                            //    optionsChild = countrys;
                            //}
                            //else if (item.Contains("ZOSTATE"))
                            //{
                            //    columnName = "机构状态";
                            //    type = "Single";
                            //    optionsChild = valDomain.Where(x => x.Code == "ZOSTATE").ToList();
                            //}
                            //else if (item.Contains("ZOATTR"))
                            //{
                            //    columnName = "机构属性";
                            //    type = "Single";
                            //    optionsChild = valDomain.Where(x => x.Code == "ZOATTR").ToList();
                            //}
                            //else if (item.Contains("ZOCATTR"))
                            //{
                            //    columnName = "机构子属性";
                            //    type = "Single";
                            //    optionsChild = valDomain.Where(x => x.Code == "ZOCATTR").ToList();
                            //}
                            //else if (item.Contains("ZHOLDING"))
                            //{
                            //    columnName = "持股情况";
                            //    type = "Single";
                            //    optionsChild = valDomain.Where(x => x.Code == "ZHOLDING").ToList();
                            //}
                            //else if (item.Contains("ZCHECKIND"))
                            //{
                            //    columnName = "是否独立核算";
                            //    type = "Single";
                            //    optionsChild = valDomain.Where(x => x.Code == "ZCHECKIND").ToList();
                            //}
                            //else if (item.Contains("ZENTC"))
                            //{
                            //    columnName = "企业分类代码";
                            //    type = "Single";
                            //    optionsChild = valDomain.Where(x => x.Code == "ZENTC").ToList();
                            //}
                            options.Add(new FilterConditionDto { CoulmnName = columnName, CoulmnKey = char.ToLower(item[0]) + item.Substring(1), Options = optionsChild, Type = type });
                        }
                    }
                    break;
            }
            #endregion

            responseAjaxResult.SuccessResult(options);
            return responseAjaxResult;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
        #endregion
        #region 数据脱敏规则  字段静态写入
        /// <summary>
        /// 数据脱敏规则  字段静态写入
        /// </summary>
        /// <param name="interfaceId">应用系统接口id</param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> SetFiledAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();
            List<SystemInterfaceField> ddd = new();
            var excludedProperties = new HashSet<string> { "CreatedAt", "CreateTime", "CreateId", "UpdateId", "DeleteTime", "Timestamp", "IsDelete", "UpdatedAt", "ZAWARDP_LIST", "DeleteId", "Zdelete", "Fzstate", "Fzversion", "Id", "Version", "DataIdentifier", "Ids", "TreeCode", "StatusOfUnit", "Fzdelete", "CreateBy", "CreatTime", "ChangeTime", "Children", "ModifiedBy", "SourceSystem", "UpdateBy", "UpdateTime", "State", "FzitAi", "FzitAg", "FzitAk", "FzitAh", "FzitDe", "FzitAj", "ViewIdentification", "ViewFlag", "Ztreeid1" };

            var resList = await _dbContext.AsTenant().QueryableWithAttr<SystemInterface>()
                .Select(t => new { t.Id, t.InterfaceName, t.Remark })
              .ToListAsync();

            foreach (var r in resList)
            {
                List<string> filds = new();
                switch (r.InterfaceName)
                {
                    //case "GetUserSearchAsync":
                    //case "GetUserDetailsAsync":
                    //    var properties = GetProperties<UserSearchDetailsDto>();
                    //    foreach (var property in properties)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetInstitutionsAsync":
                    //case "GetInstitutionDetailsAsync":
                    //    var properties2 = GetProperties<InstitutionDetatilsDto>();
                    //    foreach (var property in properties2)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetProjectSearchAsync":
                    //case "GetProjectDetailsAsync":
                    //    //var properties3 = GetProperties<ProjectDetailsDto>();
                    //    var properties3 = GetProperties<DHProjects>();
                    //    foreach (var property in properties3)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetCorresUnitSearchAsync":
                    //case "GetCorresUnitDetailAsync":
                    //    var properties4 = GetProperties<CorresUnitDetailsDto>();
                    //    foreach (var property in properties4)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetCountryRegionSearchAsync":
                    //case "GetCountryRegionDetailsAsync":
                    //    var properties5 = GetProperties<CountryRegionDetailsDto>();
                    //    foreach (var property in properties5)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetCountryContinentSearchAsync":
                    //case "GetCountryContinentDetailsAsync":
                    //    var properties6 = GetProperties<CountryContinentDetailsDto>();
                    //    foreach (var property in properties6)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetFinancialInstitutionSearchAsync":
                    //case "GetFinancialInstitutionDetailsAsync":
                    //    var properties7 = GetProperties<FinancialInstitutionDetailsDto>();
                    //    foreach (var property in properties7)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetDeviceClassCodeSearchAsync":
                    //case "GetDeviceClassCodeDetailsAsync":
                    //    var properties8 = GetProperties<DeviceClassCodeDetailsDto>();
                    //    foreach (var property in properties8)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name.ToLower()
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetInvoiceTypeSearchAsync":
                    //case "GetInvoiceTypeDetailsASync":
                    //    var properties9 = GetProperties<InvoiceTypeDetailshDto>();
                    //    foreach (var property in properties9)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetScientifiCNoProjectSearchAsync":
                    //case "GetScientifiCNoProjectDetailsAsync":
                    //    //var properties10 = GetProperties<ScientifiCNoProjectDetailsDto>();
                    //    var properties10 = GetProperties<DHResearch>();
                    //    foreach (var property in properties10)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name.ToLower()
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetLanguageSearchAsync":
                    //case "GetLanguageDetailsAsync":
                    //    var properties11 = GetProperties<LanguageDetailsDto>();
                    //    foreach (var property in properties11)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetBankCardSearchAsync":
                    //case "GetBankCardDetailsAsync":
                    //    var properties12 = GetProperties<BankCardDetailsDto>();
                    //    foreach (var property in properties12)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetDeviceDetailCodeSearchAsync":
                    //case "GetDeviceDetailCodeDetailsAsync":
                    //    var properties13 = GetProperties<DeviceDetailCodeDetailsDto>();
                    //    foreach (var property in properties13)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetAccountingDepartmentSearchAsync":
                    //case "GetAccountingDepartmentDetailsAsync":
                    //    //var properties14 = GetProperties<AccountingDepartmentDetailsDto>();
                    //    var properties14 = GetProperties<DHAccountingDept>();
                    //    foreach (var property in properties14)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetRelationalContractsSearchAsync":
                    //case "GetRelationalContractsDetailsAsync":
                    //    var properties15 = GetProperties<RelationalContractsDetailsDto>();
                    //    foreach (var property in properties15)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetRegionalDetailsAsync":
                    //case "GetRegionalSearchAsync":
                    //    var properties16 = GetProperties<RegionalDetailsDto>();
                    //    foreach (var property in properties16)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetUnitMeasurementSearchAsync":
                    //case "GetUnitMeasurementDetailsAsync":
                    //    var properties17 = GetProperties<UnitMeasurementDetailsDto>();
                    //    foreach (var property in properties17)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetProjectClassificationSearchAsync":
                    //case "GetProjectClassificationDetailsAsync":
                    //    var properties18 = GetProperties<ProjectClassificationDetailsDto>();
                    //    foreach (var property in properties18)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetRegionalCenterSearchAsync":
                    //case "GetRegionalCenterDetailsAsync":
                    //    var properties19 = GetProperties<RegionalCenterDetailsDto>();
                    //    foreach (var property in properties19)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetNationalEconomySearchAsync":
                    //case "GetNationalEconomyDetailsAsync":
                    //    var properties20 = GetProperties<NationalEconomyDetailsDto>();
                    //    foreach (var property in properties20)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetAdministrativeAccountingMapperSearchAsync":
                    //case "GetAdministrativeAccountingMapperDetailsAsync":
                    //    //var properties21 = GetProperties<AdministrativeAccountingMapperDetailsDto>();
                    //    var properties21 = GetProperties<DHAdministrative>();
                    //    foreach (var property in properties21)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetEscrowOrganizationSearchAsync":
                    //case "GetEscrowOrganizationDetailsAsync":
                    //    var properties23 = GetProperties<EscrowOrganizationDetailsDto>();
                    //    foreach (var property in properties23)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetBusinessNoCpportunitySearchAsync":
                    //case "GetBusinessCpportunitySearchAsync":
                    //case "GetBusinessNoCpportunityDetailsAsync":
                    //    //var properties24 = GetProperties<BusinessNoCpportunityDetailsDto>();
                    //    var properties24 = GetProperties<DHOpportunity>();
                    //    foreach (var property in properties24)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetAdministrativeDivisionSearchAsync":
                    //case "GetAdministrativeDivisionDetailsAsync":
                    //    var properties25 = GetProperties<AdministrativeDivisionDetailsDto>();
                    //    foreach (var property in properties25)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetAccountingOrganizationSearchAsync":
                    //case "GetAccountingOrganizationDetailsAsync":
                    //    //var properties26 = GetProperties<AccountingOrganizationDetailsDto>();
                    //    var properties26 = GetProperties<DHAdjustAccountsMultipleOrg>();
                    //    foreach (var property in properties26)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetCurrencySearchAsync":
                    //case "GetCurrencyDetailsAsync":
                    //    var properties27 = GetProperties<CurrencyDetailsDto>();
                    //    foreach (var property in properties27)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetValueDomainReceiveAsync":
                    //    var properties28 = GetProperties<ValueDomainReceiveResponseDto>();
                    //    foreach (var property in properties28)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    //case "GetDHVirtualProjectAsync":
                    //    var properties29 = GetProperties<DHVirtualProject>();
                    //    foreach (var property in properties29)
                    //    {
                    //        if (!excludedProperties.Contains(property.Name))
                    //        {
                    //            ddd.Add(new SystemInterfaceField
                    //            {
                    //                Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                    //                AppSystemInterfaceId = r.Id,
                    //                FieidName = property.Name
                    //            });
                    //        }
                    //    }
                    //    break;
                    case "GetXZOrganzationSearchAsync":
                        //var properties30 = GetProperties<AdministrativeOrganization>();
                        var properties30 = GetProperties<DHOrganzationDep>();
                        foreach (var property in properties30)
                        {
                            if (!excludedProperties.Contains(property.Name))
                            {
                                ddd.Add(new SystemInterfaceField
                                {
                                    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                                    AppSystemInterfaceId = r.Id,
                                    FieidName = property.Name
                                });
                            }
                        }
                        break;
                }
            }
            ddd.ForEach(x => x.FieidName = char.ToLower(x.FieidName[0]) + x.FieidName.Substring(1));

            await _dbContext.AsTenant().InsertableWithAttr(ddd).ExecuteCommandAsync();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;

        }
        /// <summary>
        /// 修改静态字段名字
        /// </summary>
        /// <param name="modify"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ModifyNameAsync(List<SystemInterfaceField> modify)
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            await _dbContext.AsTenant().UpdateableWithAttr(modify).WhereColumns(x => x.Id).UpdateColumns(x => x.FieidZHName).ExecuteCommandAsync();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }

        public async Task<ResponseAjaxResult<List<SearchDataDesensitizationRule>>> GetSearchDataDesensitizationRuleAsync(string interfaceId)
        {
            ResponseAjaxResult<List<SearchDataDesensitizationRule>> responseAjaxResult = new();

            var dList = await _dbContext.AsTenant().QueryableWithAttr<SystemInterfaceField>()
              .Where(t => t.AppSystemInterfaceId.ToString() == interfaceId)
              .Select(t => new SearchDataDesensitizationRule
              {
                  Id = t.Id.ToString(),
                  AppSystemInterfaceId = t.AppSystemInterfaceId.ToString(),
                  FieidName = t.FieidName,
                  FieidZHName = t.FieidZHName
              })
              .ToListAsync();

            responseAjaxResult.SuccessResult(dList);
            return responseAjaxResult;
        }
        /// <summary>
        /// 写字段规则
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ModifyAsync()
        {
            ResponseAjaxResult<bool> responseAjaxResult = new();

            var mainRes = await _dbContext.AsTenant().QueryableWithAttr<AppInterfaceAuthorization>().ToListAsync();

            var detailsRes = await _dbContext.AsTenant().QueryableWithAttr<SystemInterfaceField>().ToListAsync();

            List<DataDesensitizationRule> add = new();
            foreach (var item in mainRes)
            {
                var res = detailsRes.Where(x => x.AppSystemInterfaceId == item.Id).ToList();
                foreach (var item2 in res)
                {
                    add.Add(new DataDesensitizationRule
                    {
                        Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
                        AppSystemApiId = item.AppSystemId,
                        AppSystemInterfaceFieIdId = item2.Id,
                        DesensitizationType = 0
                    });
                }
            }
            await _dbContext.AsTenant().InsertableWithAttr(add).ExecuteCommandAsync();
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion
    }
}

