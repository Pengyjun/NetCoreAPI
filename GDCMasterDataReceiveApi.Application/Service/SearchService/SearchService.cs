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
        private readonly IBaseService _baseService;
        private readonly IDataAuthorityService _dataAuthorityService;
        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="dataAuthorityService"></param>
        /// <param name="baseService"></param>
        public SearchService(ISqlSugarClient dbContext, IBaseService baseService, IDataAuthorityService dataAuthorityService)
        {
            this._dbContext = dbContext;
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
                    NAME = u.NAME,
                    CERT_NO = u.CERT_NO,
                    OFFICE_DEPID = u.OFFICE_DEPID,
                    EMAIL = u.EMAIL,
                    EMP_CODE = u.EMP_CODE,
                    Enable = u.Enable == 1 ? "启用" : "禁用",
                    UserInfoStatus = us.OneName,
                    PHONE = u.PHONE,
                    OFFICE_DEPID_Name = ins.NAME
                }).ToPageListAsync(requestDto.PageIndex, requestDto.PageSize, total);

            //获取机构信息
            var institutions = await _dbContext.Queryable<Institution>()
                .Where(t => t.IsDelete == 1)
                .Select(t => new UInstutionDto { OID = t.OID, POID = t.POID, GRULE = t.GRULE, NAME = t.NAME })
                .ToListAsync();

            foreach (var institution in userInfos)
            {
                institution.CompanyName = GetUserCompany(institution.OFFICE_DEPID, institutions);
            }

            responseAjaxResult.Count = total;
            responseAjaxResult.SuccessResult(userInfos);
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
            var uInsInfo = uInstutionDtos.FirstOrDefault(x => x.OID == oid);
            if (uInsInfo != null)
            {
                //获取用户机构全部规则  反查机构信息
                if (!string.IsNullOrWhiteSpace(uInsInfo.GRULE))
                {
                    var ruleIds = uInsInfo.GRULE.Trim('-').Split('-').ToList();
                    var companyNames = ruleIds
                        .Select(id => uInstutionDtos.FirstOrDefault(inst => inst.OID == id))
                        .Where(inst => inst != null)
                        .Select(inst => inst?.NAME)
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

            return responseAjaxResult;
        }
    }
}
