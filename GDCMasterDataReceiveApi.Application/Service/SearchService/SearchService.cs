using AutoMapper;
using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Enums;
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
        public async Task<ResponseAjaxResult<UserSearchOtherColumns>> GetUserDetailsAsync(string uId)
        {
            var responseAjaxResult = new ResponseAjaxResult<UserSearchOtherColumns>();

            var uDetails = await _dbContext.Queryable<User>()
                .Where(t => t.IsDelete == 1 && t.Id.ToString() == uId)
                .FirstAsync();

            if (uDetails != null)
            {
                var res = _mapper.Map<User, UserSearchOtherColumns>(uDetails);
                responseAjaxResult.Count = 1;
                responseAjaxResult.SuccessResult(res);
            }
            else
            {
                responseAjaxResult.FailResult(HttpStatusCode.DataNotEXIST, "无数据");
            }
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
    }
}
