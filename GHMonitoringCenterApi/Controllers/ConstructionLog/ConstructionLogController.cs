using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionLog;
using GHMonitoringCenterApi.Application.Contracts.IService.ConstructionLog;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.ConstructionLog
{


   
    /// <summary>
    /// 施工日志
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
   
    public class ConstructionLogController : BaseController
    {
        #region 依赖注入
        public IConstructionLogService constructionLogService { get; set; }
        public ConstructionLogController (IConstructionLogService constructionLogService)
        {
            this.constructionLogService = constructionLogService;
        }
        #endregion
        /// <summary>
        /// 获取施工日志列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchConstructionLog")]
        public async Task<ResponseAjaxResult<List<ConstructionLogResponseDto>>> SearchConstructionLogAsync([FromQuery] ConstructionLogRequestDto constructionLogRequestDto)
        {
            return await constructionLogService.SearchConstructionLogAsync(constructionLogRequestDto, CurrentUser.CurrentLoginInstitutionOid);
        }
        /// <summary>
        /// 获取已填报日志日期
        /// </summary>
        /// <param name="constructionLogRequestDto"></param>
        /// <returns></returns>
        [HttpGet("SearchCompletedConstructionLog")]
        public async Task<ResponseAjaxResult<List<SearchCompletedConstructionLogRequestDto>>> SearchCompletedConstructionLogAsync([FromQuery] SearchCompletedConstructionLogResponseDto searchCompletedConstructionLogResponseDto)
        {
            return await constructionLogService.SearchCompletedConstructionLogAsync(searchCompletedConstructionLogResponseDto.Time,searchCompletedConstructionLogResponseDto.ProjectId.Value);
        }
        /// <summary>
        /// 获取施工日志详情
        /// </summary>
        /// <param name="searchCompletedConstructionLogResponseDto"></param>
        /// <returns></returns>
        [HttpGet("SearchConstructionLoDetailsg")]
        public async Task<ResponseAjaxResult<SearchConstructionLoDetailsgResponseDto>> SearchConstructionLogDetailsgAsync([FromQuery]SearchConstructionLoDetailsgRequestDto searchConstructionLoDetailsgRequestDto )
        {
            return await constructionLogService.SearchConstructionLogDetailsgAsync(searchConstructionLoDetailsgRequestDto.Id, searchConstructionLoDetailsgRequestDto.DateTime);
        }
    }
}
