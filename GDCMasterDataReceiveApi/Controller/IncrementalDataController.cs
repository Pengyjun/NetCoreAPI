using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.IncrementalData;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 增量数据统计
    /// </summary>
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class IncrementalDataController : BaseController
    {
        private readonly IIncrementalDataSearchService _incrementalDataSearchService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="incrementalDataSearchService"></param>
        public IncrementalDataController(IIncrementalDataSearchService incrementalDataSearchService)
        {
            this._incrementalDataSearchService = incrementalDataSearchService;
        }
        /// <summary>
        /// 获取每日增量数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetIncrementalSearch")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<IncrementalDataDto>> GetIncrementalSearchAsync([FromQuery] IncrementalSearchRequestDto requestDto)
        {
            return await _incrementalDataSearchService.GetIncrementalSearchAsync(requestDto);
        }


        /// <summary>
        /// 首页统计主数据数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchEachMainDataCount")]
        public async Task<ResponseAjaxResult<EachMainDataCountResponseDto>> SearchEachMainDataCountAsync()
        {
            return await _incrementalDataSearchService.SearchEachMainDataCountAsync();
        }

        /// <summary>
        ///  首页各个公司主数据统计数量
        ///  1 是人员数量
        ///  2 是机构数量
        ///  3 是项目数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchEachCompanyMainDataCount")]
        public async Task<ResponseAjaxResult<List<EachCompanyMainDataCountResponseDto>>> SearchEachCompanyMainDataCountAsync(int type)
        {
            return await _incrementalDataSearchService.SearchEachCompanyMainDataCountAsync(type);
        }
        /// <summary>
        ///  首页各个主数据增长数量
        /// </summary>
        /// <param name="timeStr"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        [HttpGet("SearchEachDayMainDataCount")]
        public async Task<ResponseAjaxResult<List<EachMainDataCountResponseDto>>> SearchEachDayMainDataCountAsync(string timeStr, string timeEnd)
        {
            return await _incrementalDataSearchService.SearchEachDayMainDataCountAsync(timeStr, timeEnd);
        }
        /// <summary>
        /// API统计   type=1 是按时间    2是系统名称
        /// </summary>
        /// <param name="timeStr"></param>
        /// <param name="timeEnd"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("SearchCallApiCount")]
        public async Task<ResponseAjaxResult<List<EachAPIInterdaceCountResponseDto>>> SearchCallApiCountAsync(string timeStr, string timeEnd,int type)

        {
            return await _incrementalDataSearchService.SearchCallInterfaceCountAsync(timeStr, timeEnd,type);
        }

        /// <summary>
        ///  API统计下钻  type=1 是按时间    2是系统名称
        /// </summary>
        /// <param name="timeStr"></param>
        /// <param name="type"></param>
        /// <param name="appKey"></param>
        /// <returns></returns>
        [HttpGet("SearchRunHole")]
        public async Task<ResponseAjaxResult<List<EachAPIInterdaceItem>>> SearchRunHoleAsync(string timeStr, int type, string appKey)
        {
            return await _incrementalDataSearchService.SearchRunHoleAsync(timeStr,type, appKey);
        }
    }
}
