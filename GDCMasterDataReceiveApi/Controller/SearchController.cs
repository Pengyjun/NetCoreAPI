using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 列表控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchService;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="searchService"></param>
        public SearchController(ISearchService searchService)
        {
            this._searchService = searchService;
        }
        /// <summary>
        /// 楼栋列表
        /// </summary>
        /// <param name="louDongDto"></param>
        /// <returns></returns>
        [HttpPost("aa")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<LouDongDto>>> GetSearchLouDongAsync([FromBody] LouDongRequestDto louDongDto) => await _searchService.GetSearchLouDongAsync(louDongDto);
        /// <summary>
        /// 增改楼栋
        /// </summary>
        /// <param name="receiveDtos"></param>
        /// <returns></returns>
        [HttpPost("bb")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> AddOrModifyLouDongAsync([FromBody] List<LouDongReceiveDto> receiveDtos) => await _searchService.AddOrModifyLouDongAsync(receiveDtos);
    }
}
