using GHMonitoringCenterApi.Application.Contracts.Dto;
using GHMonitoringCenterApi.Application.Contracts.Dto.ShipSurvey;
using GHMonitoringCenterApi.Application.Contracts.IService.Ship;
using GHMonitoringCenterApi.Application.Contracts.IService.ShipSurvey;
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GHMonitoringCenterApi.Controllers.ShipSurvey
{
    /// <summary>
    /// 船舶检验控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShipSurveyController : BaseController
    {

        #region 依赖注入
        /// <summary>
        /// 依赖注入
        /// </summary>
        public IShipSurveyService  shipSurveyService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipService"></param>
        public ShipSurveyController(IShipSurveyService shipSurveyService)
        {
            this.shipSurveyService = shipSurveyService;
        }
        #endregion

        /// <summary>
        /// 修改船舶检验
        /// </summary>
        /// <param name="saveShipSurveyRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost("ModifyShipSurvey")]
        public async Task<ResponseAjaxResult<bool>> ModifyShipSurveyAsync(SaveShipSurveyRequestDto saveShipSurveyRequestDto)
        {
            return await shipSurveyService.ModifyShipSurveyAsync(saveShipSurveyRequestDto);
        }
        /// <summary>
        /// 查询船舶检验
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("SearchShipSurvey")]
        public async Task<ResponseAjaxResult<List<ShipSurveyResponseDto>>> SearchShipSurveyAsync([FromQuery]BaseRequestDto baseRequestDto)
        {
            return  await shipSurveyService.SearchShipSurveyAsync(baseRequestDto);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("ExcelShipSurvey")]
        public async Task<IActionResult> ExcelShipSurveyAsync([FromQuery] BaseRequestDto baseRequestDto)
        {
            baseRequestDto.PageSize = 1000;
           var data= await shipSurveyService.SearchShipSurveyAsync(baseRequestDto);
            List<string> ignoreColumns = new List<string>();
            ignoreColumns.Add("Id");
            ignoreColumns.Add("IsBeyond");
           return await ExcelImportAsync(data.Data, ignoreColumns, "船舶检验");
        }
    }
}
