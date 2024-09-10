using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
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
        public ResponseAjaxResult<List<FilterParams>> GetFilterParams()
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
        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        [HttpGet("GetUserDetails")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<UserSearchDetailsDto>> GetUserDetailsAsync([FromQuery] string uId)
        {
            return await _searchService.GetUserDetailsAsync(uId);
        }
        /// <summary>
        /// 获取机构数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetInstitutions")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<InstitutionDto>>> GetInstitutionsAsync([FromQuery] InstitutionRequestDto requestDto)
        {
            return await _searchService.GetInstitutionAsync(requestDto);
        }
        /// <summary>
        /// 获取机构详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetInstitutionDetails")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<InstitutionDetatilsDto>> GetInstitutionDetailsAsync([FromQuery] string Id)
        {
            return await _searchService.GetInstitutionDetailsAsync(Id);
        }
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetProjectSearch")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<ProjectSearchDto>>> GetProjectSearchAsync([FromQuery] ProjectRequestDto requestDto)
        {
            return await _searchService.GetProjectSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProjectDetails")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<ProjectDetailsDto>> GetProjectDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetProjectDetailsAsync(id);
        }
        /// <summary>
        /// 获取往来单位列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetCorresUnitSearch")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<CorresUnitSearchDto>>> GetCorresUnitSearchAsync([FromQuery] CorresUnitRequestDto requestDto)
        {
            return await _searchService.GetCorresUnitSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取往来单位详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCorresUnitDetail")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<CorresUnitDetailsDto>> GetCorresUnitDetailAsync([FromQuery] string id)
        {
            return await _searchService.GetCorresUnitDetailAsync(id);
        }
        /// <summary>
        /// 获取国家地区列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetCountryRegionSearch")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<CountryRegionSearchDto>>> GetCountryRegionSearchAsync([FromQuery] CountryRegionRequestDto requestDto)
        {
            return await _searchService.GetCountryRegionSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取国家地区详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCountryRegionDetails")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<CountryRegionDetailsDto>> GetCountryRegionDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetCountryRegionDetailsAsync(id);
        }
        /// <summary>
        /// 获取大洲列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetCountryContinentSearch")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<List<CountryContinentSearchDto>>> GetCountryContinentSearchAsync([FromQuery] CountryContinentRequestDto requestDto)
        {
            return await _searchService.GetCountryContinentSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取大洲详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCountryContinentDetails")]
        [AllowAnonymous]
        public async Task<ResponseAjaxResult<CountryContinentDetailsDto>> GetCountryContinentDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetCountryContinentDetailsAsync(id);
        }
    }
}
