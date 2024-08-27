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
        /// 冬天查询
        /// </summary>
        /// <param name="louDongDto"></param>
        /// <returns></returns>
        [HttpPost("aa")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<LouDongDto>>> GetSearchLouDongAsync([FromBody] LouDongRequestDto louDongDto) => await _searchService.GetSearchLouDongAsync(louDongDto);
    }
}
