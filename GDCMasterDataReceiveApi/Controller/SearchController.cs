using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RoomNumber;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ScientifiCNoProject;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService;
using GDCMasterDataReceiveApi.Domain.Shared;
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
        [HttpGet("GetFilterParams")]
        public ResponseAjaxResult<List<FilterParams>> GetFilterParams()
        {
            return _baseService.GetFilterParams();
        }
        /// <summary>
        /// 楼栋列表
        /// </summary>
        /// <param name="louDongDto"></param>
        /// <returns></returns>
        [HttpGet("GetSearchLouDong")]
        public async Task<ResponseAjaxResult<List<LouDongSearchDto>>> GetSearchLouDongAsync([FromQuery] LouDongRequestDto louDongDto)
            => await _searchService.GetSearchLouDongAsync(louDongDto);
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetUserSearch")]
        public async Task<ResponseAjaxResult<List<UserSearchResponseDto>>> GetUserSearchAsync([FromQuery] UserSearchRequestDto requestDto)
        {
            return await _searchService.GetUserSearchAsync(requestDto);
        }
        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        [HttpGet("GetUserDetails")]
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
        public async Task<ResponseAjaxResult<CountryContinentDetailsDto>> GetCountryContinentDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetCountryContinentDetailsAsync(id);
        }
        /// <summary>
        /// 获取金融机构列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetFinancialInstitutionSearch")]
        public async Task<ResponseAjaxResult<List<FinancialInstitutionSearchDto>>> GetFinancialInstitutionSearchAsync([FromQuery] FinancialInstitutionRequestDto requestDto)
        {
            return await _searchService.GetFinancialInstitutionSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取金融机构详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetFinancialInstitutionDetails")]
        public async Task<ResponseAjaxResult<FinancialInstitutionDetailsDto>> GetFinancialInstitutionDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetFinancialInstitutionDetailsAsync(id);
        }
        /// <summary>
        /// 物资设备分类编码列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceClassCodeSearch")]
        public async Task<ResponseAjaxResult<List<DeviceClassCodeSearchDto>>> GetDeviceClassCodeSearchAsync([FromQuery] DeviceClassCodeRequestDto requestDto)
        {
            return await _searchService.GetDeviceClassCodeSearchAsync(requestDto);
        }
        /// <summary>
        /// 物资设备分类编码明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceClassCodeDetails")]
        public async Task<ResponseAjaxResult<DeviceClassCodeDetailsDto>> GetDeviceClassCodeDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetDeviceClassCodeDetailsAsync(id);
        }
        /// <summary>
        /// 发票类型列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetInvoiceTypeSearch")]
        public async Task<ResponseAjaxResult<List<InvoiceTypeSearchDto>>> GetInvoiceTypeSearchAsync([FromQuery] InvoiceTypeRequestDto requestDto)
        {
            return await _searchService.GetInvoiceTypeSearchAsync(requestDto);
        }
        /// <summary>
        /// 发票类型明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetInvoiceTypeDetails")]
        public async Task<ResponseAjaxResult<InvoiceTypeDetailshDto>> GetInvoiceTypeDetailsASync([FromQuery] string id)
        {
            return await _searchService.GetInvoiceTypeDetailsASync(id);
        }
        /// <summary>
        /// 科研项目列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet(" GetScientifiCNoProjectSearch")]
        public async Task<ResponseAjaxResult<List<ScientifiCNoProjectSearchDto>>> GetScientifiCNoProjectSearchAsync([FromQuery] ScientifiCNoProjectRequestDto requestDto)
        {
            return await _searchService.GetScientifiCNoProjectSearchAsync(requestDto);
        }
        /// <summary>
        /// 科研项目明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetScientifiCNoProjectDetails")]
        public async Task<ResponseAjaxResult<ScientifiCNoProjectDetailsDto>> GetScientifiCNoProjectDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetScientifiCNoProjectDetailsAsync(id);
        }
        /// <summary>
        /// 获取房号列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetRoomNumberSearch")]
        public async Task<ResponseAjaxResult<List<RoomNumberSearchDto>>> GetRoomNumberSearchAsync([FromQuery] RoomNumberRequestDto requestDto)
        {
            return await _searchService.GetRoomNumberSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取房号详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetRoomNumberDetails")]
        public async Task<ResponseAjaxResult<RoomNumberDetailsDto>> GetRoomNumberDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetRoomNumberDetailsAsync(id);
        }
        /// <summary>
        /// 语言语种列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("GetLanguageSearch")]
        public async Task<ResponseAjaxResult<List<LanguageSearchDto>>> GetLanguageSearchAsync([FromQuery] LanguageRequestDto requestDto)
        {
            return await _searchService.GetLanguageSearchAsync(requestDto);
        }
        /// <summary>
        /// 语言语种明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetLanguageDetails")]
        public async Task<ResponseAjaxResult<LanguageDetailsDto>> GetLanguageDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetLanguageDetailsAsync(id);
        }
    }
}
