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
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DHData;
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
using GDCMasterDataReceiveApi.Domain.Models;
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
        /// 用户列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<UserSearchDetailsDto>>> GetUserSearchAsync(FilterCondition requestDto);
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
        Task<ResponseAjaxResult<List<InstitutionDetatilsDto>>> GetInstitutionAsync(FilterCondition requestDto);
        /// <summary>
        /// 新版 左侧机构树
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<InstitutionResponseDto>> GetInstitutionTreeAsync(FilterCondition requestDto);
        /// <summary>
        /// 新版 左侧机构树对应详情
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<InstitutionDetatilsDto>>> GetInstitutionTreeDetailsAsync(FilterCondition requestDto);
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
        //Task<ResponseAjaxResult<List<ProjectDetailsDto>>> GetProjectSearchAsync(FilterCondition requestDto);
        Task<ResponseAjaxResult<List<DHProjects>>> GetProjectSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 虚拟项目DH
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<DHVirtualProject>>> GetDHVirtualProjectAsync(FilterCondition requestDto);
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
        Task<ResponseAjaxResult<List<CorresUnitDetailsDto>>> GetCorresUnitSearchAsync(FilterCondition requestDto);
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
        Task<ResponseAjaxResult<List<CountryRegionDetailsDto>>> GetCountryRegionSearchAsync(FilterCondition requestDto);
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
        Task<ResponseAjaxResult<List<CountryContinentDetailsDto>>> GetCountryContinentSearchAsync(FilterCondition requestDto);
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
        Task<ResponseAjaxResult<List<FinancialInstitutionDetailsDto>>> GetFinancialInstitutionSearchAsync(FilterCondition requestDto);
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
        Task<ResponseAjaxResult<List<DeviceClassCodeDetailsDto>>> GetDeviceClassCodeSearchAsync(FilterCondition requestDto);
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
        Task<ResponseAjaxResult<List<InvoiceTypeDetailshDto>>> GetInvoiceTypeSearchAsync(FilterCondition requestDto);
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
        //Task<ResponseAjaxResult<List<ScientifiCNoProjectDetailsDto>>> GetScientifiCNoProjectSearchAsync(FilterCondition requestDto);
        Task<ResponseAjaxResult<List<DHResearchDto>>> GetScientifiCNoProjectSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 科研项目明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ScientifiCNoProjectDetailsDto>> GetScientifiCNoProjectDetailsAsync(string id);
        /// <summary>
        /// 语言语种列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<LanguageDetailsDto>>> GetLanguageSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 语言语种明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<LanguageDetailsDto>> GetLanguageDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BankCardDetailsDto>>> GetBankCardSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<BankCardDetailsDto>> GetBankCardDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<DeviceDetailCodeDetailsDto>>> GetDeviceDetailCodeSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<DeviceDetailCodeDetailsDto>> GetDeviceDetailCodeDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        //Task<ResponseAjaxResult<List<AccountingDepartmentDetailsDto>>> GetAccountingDepartmentSearchAsync(FilterCondition requestDto);
        Task<ResponseAjaxResult<List<DHAccountingDept>>> GetAccountingDepartmentSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<AccountingDepartmentDetailsDto>> GetAccountingDepartmentDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<DHMdmMultOrgAgencyRelPage>>> GetRelationalContractsSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<RelationalContractsDetailsDto>> GetRelationalContractsDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<RegionalDetailsDto>>> GetRegionalSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<RegionalDetailsDto>> GetRegionalDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<UnitMeasurementDetailsDto>>> GetUnitMeasurementSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<UnitMeasurementDetailsDto>> GetUnitMeasurementDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProjectClassificationDetailsDto>>> GetProjectClassificationSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectClassificationDetailsDto>> GetProjectClassificationDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<RegionalCenterDetailsDto>>> GetRegionalCenterSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<RegionalCenterDetailsDto>> GetRegionalCenterDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<NationalEconomyDetailsDto>>> GetNationalEconomySearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<NationalEconomyDetailsDto>> GetNationalEconomyDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        //Task<ResponseAjaxResult<List<AdministrativeAccountingMapperDetailsDto>>> GetAdministrativeAccountingMapperSearchAsync(FilterCondition requestDto);
        Task<ResponseAjaxResult<List<DHAdministrative>>> GetAdministrativeAccountingMapperSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<EscrowOrganizationDetailsDto>>> GetEscrowOrganizationSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<AdministrativeAccountingMapperDetailsDto>> GetAdministrativeAccountingMapperDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        //Task<ResponseAjaxResult<List<EscrowOrganizationDetailsDto>>> GetEscrowOrganizationSearchAsync(FilterCondition requestDto);
        Task<ResponseAjaxResult<List<DHOrganzationDep>>> GetXZOrganzationSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<EscrowOrganizationDetailsDto>> GetEscrowOrganizationDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="isJingWai"></param>
        /// <returns></returns>
        //Task<ResponseAjaxResult<List<BusinessNoCpportunityDetailsDto>>> GetBusinessNoCpportunitySearchAsync(FilterCondition requestDto,bool isJingWai);
        Task<ResponseAjaxResult<List<DHOpportunityDto>>> GetBusinessNoCpportunitySearchAsync(FilterCondition requestDto,bool isJingWai);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<BusinessNoCpportunityDetailsDto>> GetBusinessNoCpportunityDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<AdministrativeDivisionDetailsDto>>> GetAdministrativeDivisionSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<AdministrativeDivisionDetailsDto>> GetAdministrativeDivisionDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        //Task<ResponseAjaxResult<List<AccountingOrganizationDetailsDto>>> GetAccountingOrganizationSearchAsync(FilterCondition requestDto);
        Task<ResponseAjaxResult<List<DHAdjustAccountsMultipleOrg>>> GetAccountingOrganizationSearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<AccountingOrganizationDetailsDto>> GetAccountingOrganizationDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<CurrencyDetailsDto>>> GetCurrencySearchAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<CurrencyDetailsDto>> GetCurrencyDetailsAsync(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ValueDomainReceiveResponseDto>>> GetValueDomainReceiveAsync(FilterCondition requestDto);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<FilterConditionDto>>> GetUserFilterColumnsAsync(int table);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<DHMdmManagementOrgage>>> GetDHMdmMultOrgAgencyRelPageAsync(FilterCondition requestDto);
        /// <summary>
        /// 通用字典数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<BasePullDownResponseDto>>> GetDicTableAsync(int type);

        Task<ResponseAjaxResult<bool>> SetFiledAsync();
        Task<ResponseAjaxResult<bool>> ModifyNameAsync(List<SystemInterfaceField> modify);
        Task<ResponseAjaxResult<List<SearchDataDesensitizationRule>>> GetSearchDataDesensitizationRuleAsync(string interfaceId);
        Task<ResponseAjaxResult<bool>> ModifyAsync();
    }
}
