using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectMasterData;
using GHMonitoringCenterApi.Application.Contracts.IService.ProjectMasterData;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.ProjectMasterData
{
    /// <summary>
    /// 项目业务主数据
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProjectMasterDataController : ControllerBase
    {
        #region 依赖注入
        public IProjectMasterDataService projectMasterDataService { get; set; }
        public ProjectMasterDataController(IProjectMasterDataService projectMasterDataService)
        {
            this.projectMasterDataService = projectMasterDataService;
        }
        #endregion
        /// <summary>
        /// 获取项目主数据列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("ProjectMasterData")]
        public async Task<ResponseAjaxResult<List<SearchProjectMasterDataResponseDto>>> SearchProjectMasterDataAsync([FromQuery]BaseRequestDto baseRequestDto)
        {
            return await projectMasterDataService.SearchProjectMasterDataAsync(baseRequestDto);
        }




    }
}
