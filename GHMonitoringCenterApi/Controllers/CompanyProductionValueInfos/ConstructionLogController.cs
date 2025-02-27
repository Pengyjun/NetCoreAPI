using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionLog;
using GHMonitoringCenterApi.Application.Contracts.IService.CompanyProductionValueInfos;
using GHMonitoringCenterApi.Application.Contracts.IService.ConstructionLog;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.CompanyProductionValueInfos
{

    /// <summary>
    /// 公司产值信息
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CompanyProductionValueInfoController : BaseController
    {
        #region 依赖注入
        public ICompanyProductionValueInfoService companyProductionValueInfoService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyProductionValueInfoService"></param>
        public CompanyProductionValueInfoController(ICompanyProductionValueInfoService companyProductionValueInfoService)
        {
            this.companyProductionValueInfoService = companyProductionValueInfoService;
        }

        #endregion
        /// <summary>
        /// 获取公司产值信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchCompanyProductionValueInfo")]
        public async Task<ResponseAjaxResult<List<CompanyProductionValueInfoResponseDto>>> SearchCompanyProductionValueInfoAsync([FromQuery] CompanyProductionValueInfoRequsetDto requsetDto)
        {
            return await companyProductionValueInfoService.SearchCompanyProductionValueInfoAsync(requsetDto);
        }
        /// <summary>
        /// 添加或修改公司产值信息列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("AddORUpdateCompanyProductionValueInfo")]
        public async Task<ResponseAjaxResult<bool>> AddORUpdateCompanyProductionValueInfoAsync([FromBody] AddOrUpdateCompanyProductionValueInfoRequestDto requestDto)
        {
            return await companyProductionValueInfoService.AddORUpdateCompanyProductionValueInfoAsync(requestDto);
        }
        /// <summary>
        ///删除公司产值信息列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("DelectCompanyProductionValueInfo")]
        public async Task<ResponseAjaxResult<bool>> DelectCompanyProductionValueInfoAsync([FromBody] DeleteCompanyProductionValueInfoRequestDto requestDto)
        {
            return await companyProductionValueInfoService.DelectCompanyProductionValueInfoAsync(requestDto);
        }
    }
}
