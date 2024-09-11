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
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.ISearchService
{
    /// <summary>
    /// 列表接口
    /// </summary>
    [DependencyInjection]
    public interface ISearchService
    {
        /// <summary>
        /// 楼栋列表
        /// </summary>
        /// <param name="louDongDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<LouDongSearchDto>>> GetSearchLouDongAsync(LouDongRequestDto louDongDto);
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<UserSearchResponseDto>>> GetUserSearchAsync(UserSearchRequestDto requestDto);
        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<UserSearchDetailsDto>> GetUserDetailsAsync(string uId);
        /// <summary>
        /// 机构树
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<InstitutionDto>>> GetInstitutionAsync(InstitutionRequestDto requestDto);
        /// <summary>
        /// 机构详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<InstitutionDetatilsDto>> GetInstitutionDetailsAsync(string Id);
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectSearchDto>>> GetProjectSearchAsync(ProjectRequestDto requestDto);
        /// <summary>
        /// 项目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectDetailsDto>> GetProjectDetailsAsync(string id);
        /// <summary>
        /// 往来单位列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<CorresUnitSearchDto>>> GetCorresUnitSearchAsync(CorresUnitRequestDto requestDto);
        /// <summary>
        /// 往来单位详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<CorresUnitDetailsDto>> GetCorresUnitDetailAsync(string id);
        /// <summary>
        /// 获取国家地区列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<CountryRegionSearchDto>>> GetCountryRegionSearchAsync(CountryRegionRequestDto requestDto);
        /// <summary>
        /// 获取国家地区详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<CountryRegionDetailsDto>> GetCountryRegionDetailsAsync(string id);
        /// <summary>
        /// 获取大洲列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<CountryContinentSearchDto>>> GetCountryContinentSearchAsync(CountryContinentRequestDto requestDto);
        /// <summary>
        /// 获取大洲详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<CountryContinentDetailsDto>> GetCountryContinentDetailsAsync(string id);
        /// <summary>
        /// 获取金融机构列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<FinancialInstitutionSearchDto>>> GetFinancialInstitutionSearchAsync(FinancialInstitutionRequestDto requestDto);
        /// <summary>
        /// 获取金融机构详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<FinancialInstitutionDetailsDto>> GetFinancialInstitutionDetailsAsync(string id);
        /// <summary>
        /// 物资设备分类编码列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<DeviceClassCodeSearchDto>>> GetDeviceClassCodeSearchAsync(DeviceClassCodeRequestDto requestDto);
        /// <summary>
        /// 物资设备分类编码明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<DeviceClassCodeDetailsDto>> GetDeviceClassCodeDetailsAsync(string id);
        /// <summary>
        /// 发票类型列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<InvoiceTypeSearchDto>>> GetInvoiceTypeSearchAsync(InvoiceTypeRequestDto requestDto);
        /// <summary>
        /// 发票类型明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<InvoiceTypeDetailshDto>> GetInvoiceTypeDetailsASync(string id);
        /// <summary>
        /// 科研项目列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ScientifiCNoProjectSearchDto>>> GetScientifiCNoProjectSearchAsync(ScientifiCNoProjectRequestDto requestDto);
        /// <summary>
        /// 科研项目明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ScientifiCNoProjectDetailsDto>> GetScientifiCNoProjectDetailsAsync(string id);
        /// <summary>
        /// 获取房号列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<RoomNumberSearchDto>>> GetRoomNumberSearchAsync(RoomNumberRequestDto requestDto);
        /// <summary>
        /// 获取房号详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<RoomNumberDetailsDto>> GetRoomNumberDetailsAsync(string id);
        /// <summary>
        /// 语言语种列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<LanguageSearchDto>>> GetLanguageSearchAsync(LanguageRequestDto requestDto);
        /// <summary>
        /// 语言语种明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<LanguageDetailsDto>> GetLanguageDetailsAsync(string id);
    }
}
