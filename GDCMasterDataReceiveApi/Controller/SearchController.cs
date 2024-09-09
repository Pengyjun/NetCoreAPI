using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
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
        private readonly IBaseService _baseService;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="searchService"></param>
        /// <param name="baseService"></param>
        public SearchController(ISearchService searchService, IBaseService baseService)
        {
            this._searchService = searchService;
            this._baseService = baseService;
        }
        /// <summary>
        /// 默认加载获取条件参数
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetFilterParams")]
        public List<FilterParams> GetFilterParams()
        {
            return _baseService.GetFilterParams();
        }
        /// <summary>
        /// 楼栋列表
        /// </summary>
        /// <param name="louDongDto"></param>
        /// <returns></returns>
        [HttpPost("GetSearchLouDong")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<LouDongDto>>> GetSearchLouDongAsync([FromBody] LouDongRequestDto louDongDto)
            => await _searchService.GetSearchLouDongAsync(louDongDto);
        /// <summary>
        /// 增改楼栋
        /// </summary>
        /// <param name="receiveDtos"></param>
        /// <returns></returns>
        [HttpPost("AddOrModifyLouDong")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<bool>> AddOrModifyLouDongAsync([FromBody] List<LouDongReceiveDto> receiveDtos)
            => await _searchService.AddOrModifyLouDongAsync(receiveDtos);

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetUserSearch")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<UserSearchResponseDto>>> GetUserSearchAsync([FromBody] UserSearchRequestDto requestDto)
        {
            return await _searchService.GetUserSearchAsync(requestDto);
        }

    }
}
