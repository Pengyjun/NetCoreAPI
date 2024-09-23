using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingDepartment;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeAccountingMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeDivision;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.BankCard;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.BusinessNoCpportunity;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceDetailCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.EscrowOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.NationalEconomy;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RelationalContracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ScientifiCNoProject;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
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
        /// 用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetUserSearch")]
        public async Task<ResponseAjaxResult<List<UserSearchDetailsDto>>> GetUserSearchAsync([FromBody] FilterCondition requestDto)
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
        [HttpPost("GetInstitutions")]
        public async Task<ResponseAjaxResult<List<InstitutionDetatilsDto>>> GetInstitutionsAsync([FromBody] FilterCondition requestDto)
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
        [HttpPost("GetProjectSearch")]
        public async Task<ResponseAjaxResult<List<ProjectDetailsDto>>> GetProjectSearchAsync([FromBody] FilterCondition requestDto)
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
        [HttpPost("GetCorresUnitSearch")]
        public async Task<ResponseAjaxResult<List<CorresUnitDetailsDto>>> GetCorresUnitSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetCorresUnitSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取往来单位详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCorresUnitDetails")]
        public async Task<ResponseAjaxResult<CorresUnitDetailsDto>> GetCorresUnitDetailAsync([FromQuery] string id)
        {
            return await _searchService.GetCorresUnitDetailAsync(id);
        }
        /// <summary>
        /// 获取国家地区列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetCountryRegionSearch")]
        public async Task<ResponseAjaxResult<List<CountryRegionDetailsDto>>> GetCountryRegionSearchAsync([FromBody] FilterCondition requestDto)
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
        [HttpPost("GetCountryContinentSearch")]
        public async Task<ResponseAjaxResult<List<CountryContinentDetailsDto>>> GetCountryContinentSearchAsync([FromBody] FilterCondition requestDto)
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
        [HttpPost("GetFinancialInstitutionSearch")]
        public async Task<ResponseAjaxResult<List<FinancialInstitutionDetailsDto>>> GetFinancialInstitutionSearchAsync([FromBody] FilterCondition requestDto)
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
        [HttpPost("GetDeviceClassCodeSearch")]
        public async Task<ResponseAjaxResult<List<DeviceClassCodeDetailsDto>>> GetDeviceClassCodeSearchAsync([FromBody] FilterCondition requestDto)
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
        [HttpPost("GetInvoiceTypeSearch")]
        public async Task<ResponseAjaxResult<List<InvoiceTypeDetailshDto>>> GetInvoiceTypeSearchAsync([FromBody] FilterCondition requestDto)
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
        [HttpPost("GetScientifiCNoProjectSearch")]
        public async Task<ResponseAjaxResult<List<ScientifiCNoProjectDetailsDto>>> GetScientifiCNoProjectSearchAsync([FromBody] FilterCondition requestDto)
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
        /// 语言语种列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetLanguageSearch")]
        public async Task<ResponseAjaxResult<List<LanguageDetailsDto>>> GetLanguageSearchAsync([FromBody] FilterCondition requestDto)
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
        /// <summary>
        /// 获取银行账号列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetBankCardSearch")]
        public async Task<ResponseAjaxResult<List<BankCardDetailsDto>>> GetBankCardSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetBankCardSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取银行账号详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetBankCardDetails")]
        public async Task<ResponseAjaxResult<BankCardDetailsDto>> GetBankCardDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetBankCardDetailsAsync(id);
        }
        /// <summary>
        /// 获取物资设备明细编码
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetDeviceDetailCodeSearch")]
        public async Task<ResponseAjaxResult<List<DeviceDetailCodeDetailsDto>>> GetDeviceDetailCodeSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetDeviceDetailCodeSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取物资设备明细编码 详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetDeviceDetailCodeDetails")]
        public async Task<ResponseAjaxResult<DeviceDetailCodeDetailsDto>> GetDeviceDetailCodeDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetDeviceDetailCodeDetailsAsync(id);
        }
        /// <summary>
        /// 获取核算部门列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetAccountingDepartmentSearch")]
        public async Task<ResponseAjaxResult<List<AccountingDepartmentDetailsDto>>> GetAccountingDepartmentSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetAccountingDepartmentSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取核算部门详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetAccountingDepartmentDetails")]
        public async Task<ResponseAjaxResult<AccountingDepartmentDetailsDto>> GetAccountingDepartmentDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetAccountingDepartmentDetailsAsync(id);
        }
        /// <summary>
        /// 获取委托关系列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetRelationalContractsSearch")]
        public async Task<ResponseAjaxResult<List<RelationalContractsDetailsDto>>> GetRelationalContractsSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetRelationalContractsSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取委托关系详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetRelationalContractsDetails")]
        public async Task<ResponseAjaxResult<RelationalContractsDetailsDto>> GetRelationalContractsDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetRelationalContractsDetailsAsync(id);
        }
        /// <summary>
        /// 中交区域总部列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetRegionalSearch")]
        public async Task<ResponseAjaxResult<List<RegionalDetailsDto>>> GetRegionalSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetRegionalSearchAsync(requestDto);
        }
        /// <summary>
        /// 中交区域总部详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetRegionalDetails")]
        public async Task<ResponseAjaxResult<RegionalDetailsDto>> GetRegionalDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetRegionalDetailsAsync(id);
        }
        /// <summary>
        /// 获取计量单位列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetUnitMeasurementSearch")]
        public async Task<ResponseAjaxResult<List<UnitMeasurementDetailsDto>>> GetUnitMeasurementSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetUnitMeasurementSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取计量单位详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUnitMeasurementDetails")]
        public async Task<ResponseAjaxResult<UnitMeasurementDetailsDto>> GetUnitMeasurementDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetUnitMeasurementDetailsAsync(id);
        }
        /// <summary>
        /// 获取中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系 列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetProjectClassificationSearch")]
        public async Task<ResponseAjaxResult<List<ProjectClassificationDetailsDto>>> GetProjectClassificationSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetProjectClassificationSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProjectClassificationDetails")]
        public async Task<ResponseAjaxResult<ProjectClassificationDetailsDto>> GetProjectClassificationDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetProjectClassificationDetailsAsync(id);
        }
        /// <summary>
        /// 获取中交区域中心列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetRegionalCenterSearch")]
        public async Task<ResponseAjaxResult<List<RegionalCenterDetailsDto>>> GetRegionalCenterSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetRegionalCenterSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取中交区域中心详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetRegionalCenterDetails")]
        public async Task<ResponseAjaxResult<RegionalCenterDetailsDto>> GetRegionalCenterDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetRegionalCenterDetailsAsync(id);
        }
        /// <summary>
        /// 获取国民经济行业分类列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetNationalEconomySearch")]
        public async Task<ResponseAjaxResult<List<NationalEconomyDetailsDto>>> GetNationalEconomySearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetNationalEconomySearchAsync(requestDto);
        }
        /// <summary>
        /// 获取国民经济行业分类详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetNationalEconomyDetails")]
        public async Task<ResponseAjaxResult<NationalEconomyDetailsDto>> GetNationalEconomyDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetNationalEconomyDetailsAsync(id);
        }
        /// <summary>
        /// 获取行政机构和核算机构映射关系 列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetAdministrativeAccountingMapperSearch")]
        public async Task<ResponseAjaxResult<List<AdministrativeAccountingMapperDetailsDto>>> GetAdministrativeAccountingMapperSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetAdministrativeAccountingMapperSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取行政机构和核算机构映射关系 明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetAdministrativeAccountingMapperDetails")]
        public async Task<ResponseAjaxResult<AdministrativeAccountingMapperDetailsDto>> GetAdministrativeAccountingMapperDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetAdministrativeAccountingMapperDetailsAsync(id);
        }
        /// <summary>
        /// 多组织-税务代管组织(行政)列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetEscrowOrganizationSearch")]
        public async Task<ResponseAjaxResult<List<EscrowOrganizationDetailsDto>>> GetEscrowOrganizationSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetEscrowOrganizationSearchAsync(requestDto);
        }
        /// <summary>
        /// 多组织-税务代管组织(行政) 详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetEscrowOrganizationDetails")]
        public async Task<ResponseAjaxResult<EscrowOrganizationDetailsDto>> GetEscrowOrganizationDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetEscrowOrganizationDetailsAsync(id);
        }
        /// <summary>
        /// 商机项目(不含境外商机项目) 列表 国家地区区分  142境内，142以为境外
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetBusinessNoCpportunitySearch")]
        public async Task<ResponseAjaxResult<List<BusinessNoCpportunityDetailsDto>>> GetBusinessNoCpportunitySearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetBusinessNoCpportunitySearchAsync(requestDto, false);
        }
        /// <summary>
        /// 商机项目(含境外商机项目) 列表 国家地区区分  142境内，142以为境外
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetBusinessCpportunitySearch")]
        public async Task<ResponseAjaxResult<List<BusinessNoCpportunityDetailsDto>>> GetBusinessCpportunitySearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetBusinessNoCpportunitySearchAsync(requestDto, true);
        }
        /// <summary>
        /// 商机项目(不含境外商机项目) 详情 国家地区区分  142境内，142以为境外
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetBusinessNoCpportunityDetails")]
        public async Task<ResponseAjaxResult<BusinessNoCpportunityDetailsDto>> GetBusinessNoCpportunityDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetBusinessNoCpportunityDetailsAsync(id);
        }
        /// <summary>
        /// 获取境内行政区划 列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetAdministrativeDivisionSearch")]
        public async Task<ResponseAjaxResult<List<AdministrativeDivisionDetailsDto>>> GetAdministrativeDivisionSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetAdministrativeDivisionSearchAsync(requestDto);
        }
        /// <summary>
        /// 获取境内行政区划 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetAdministrativeDivisionDetails")]
        public async Task<ResponseAjaxResult<AdministrativeDivisionDetailsDto>> GetAdministrativeDivisionDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetAdministrativeDivisionDetailsAsync(id);
        }
        /// <summary>
        /// 多组织-核算机构 列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetAccountingOrganizationSearch")]
        public async Task<ResponseAjaxResult<List<AccountingOrganizationDetailsDto>>> GetAccountingOrganizationSearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetAccountingOrganizationSearchAsync(requestDto);
        }
        /// <summary>
        /// 多组织-核算机构 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetAccountingOrganizationDetails")]
        public async Task<ResponseAjaxResult<AccountingOrganizationDetailsDto>> GetAccountingOrganizationDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetAccountingOrganizationDetailsAsync(id);
        }
        /// <summary>
        /// 获取币种列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetCurrencySearch")]
        public async Task<ResponseAjaxResult<List<CurrencyDetailsDto>>> GetCurrencySearchAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetCurrencySearchAsync(requestDto);
        }
        /// <summary>
        /// 获取币种详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCurrencyDetails")]
        public async Task<ResponseAjaxResult<CurrencyDetailsDto>> GetCurrencyDetailsAsync([FromQuery] string id)
        {
            return await _searchService.GetCurrencyDetailsAsync(id);
        }
        /// <summary>
        /// 获取值域列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("GetValueDomainReceive")]
        public async Task<ResponseAjaxResult<List<ValueDomainReceiveResponseDto>>> GetValueDomainReceiveAsync([FromBody] FilterCondition requestDto)
        {
            return await _searchService.GetValueDomainReceiveAsync(requestDto);
        }
        /// <summary>
        /// 条件筛选列
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        [HttpGet("GetUserFilterColumns")]
        public async Task<ResponseAjaxResult<List<FilterConditionDto>>> GetUserFilterColumnsAsync([FromQuery] int table)
        {
            
            return await _searchService.GetUserFilterColumnsAsync(table);
        }

    }
}
