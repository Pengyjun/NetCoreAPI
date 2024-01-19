using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Service.Projects;
using GHMonitoringCenterApi.CustomAttribute;
using GHMonitoringCenterApi.Domain.Shared.Util;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GHMonitoringCenterApi.Application.Contracts.IService.Push;
using GHMonitoringCenterApi.Filters;

namespace GHMonitoringCenterApi.Controllers.Push
{

    /// <summary>
    ///  推送控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ThreadLockFilter]
    public class PushController : BaseController
    {
        /// <summary>
        /// 推送到Pom业务层
        /// </summary>

        private readonly IPushPomService _pushPomService;

        /// <summary>
        /// 推送注入
        /// </summary>
        public PushController(IPushPomService pushPomService)
        {
            _pushPomService = pushPomService;
        }
        /// <summary>
        ///推送项目信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("PushProjects")]
        public async Task<ResponseAjaxResult<bool>> PushProjectAsync()
        {
            return await _pushPomService.PushProjectAsync();
        }

        

        /// <summary>
        ///推送项目月报
        /// </summary>
        /// <returns></returns>
        [HttpPost("PushMonthReports")]
        public async Task<ResponseAjaxResult<bool>> PushMonthReportsAsync()
        {
            return await _pushPomService.PushMonthReportsAsync();
        }

        /// <summary>
        /// 自有船舶月报
        /// </summary>
        /// <returns></returns>
        [HttpPost("PushOwnerShipMonthReports")]
        public async Task<ResponseAjaxResult<bool>> PushOwnerShipMonthReportsAsync()
        {
            return await _pushPomService.PushOwnerShipMonthReportsAsync();
        }

        /// <summary>
        /// 推送分包船舶月报
        /// </summary>
        /// <returns></returns>
        [HttpPost("PushSubShipMonthReports")]
        public async Task<ResponseAjaxResult<bool>> PushSubShipMonthReportsAsync()
        {
            return await _pushPomService.PushSubShipMonthReportsAsync();
        }

		/// <summary>
		/// 推送分包船舶
		/// </summary>
		/// <returns></returns>
		[HttpPost("PushSubShips")]
		public async Task<ResponseAjaxResult<bool>> PushSubShipsAsync()
		{
			return await _pushPomService.PushSubShipsAsync();
		}

		/// <summary>
		///推送船舶日报
		/// </summary>
		/// <returns></returns>
		[HttpPost("PushShipDayReports")]
        public async Task<ResponseAjaxResult<bool>> PushShipDayReportsAsync()
        {
            return await _pushPomService.PushShipDayReportsAsync();
        }

        /// <summary>
        ///推送项目日报
        /// </summary>
        /// <returns></returns>
        [HttpPost("PushProjectDayReports")]
        public async Task<ResponseAjaxResult<bool>> PushProectDayReportsAsync()
        {
            return await _pushPomService.PushDayReportsAsync();
        }

        /// <summary>
        ///推送安监日报
        /// </summary>
        /// <returns></returns>
        [HttpPost("PushSafeDayReports")]
        public async Task<ResponseAjaxResult<bool>> PushSafeDayReportsAsync()
        {
            return await _pushPomService.PushSafeDayReportsAsync();
        }
    }
}
