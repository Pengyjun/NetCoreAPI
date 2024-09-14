using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingDepartment;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AccountingOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeAccountingMapper;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeDivision;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.AdministrativeOrganization;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.BusinessNoCpportunity;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceDetailCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.LouDong;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.NationalEconomy;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.POPManagOrg;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ProjectClassification;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Regional;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RegionalCenter;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RelationalContracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RoomNumber;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ScientifiCNoProject;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.UnitMeasurement;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    /// <summary>
    /// 接收主数据推送接口控制器（此处控制器名称小写因接口规定）
    /// </summary>
    [ApiController]
    public class ReceiveController : BaseController
    {
        /// <summary>
        /// 引入服务
        /// </summary>
        private readonly IReceiveService _receiveService;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="receiveService"></param>
        public ReceiveController(IReceiveService receiveService)
        {
            this._receiveService = receiveService;
        }

        #region 主数据各类数据获取
        ///// <summary>
        ///// 获取通用字典数据
        ///// </summary>
        ///// <returns></returns>
        //[UnitOfWork]
        //[HttpPost("Common")]
        //public Task<MDMResponseResult> CommonDataAsync() => _receiveService.CommonDataAsync();

        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/EscrowOrganization")]
        public async Task<MDMResponseResult> EscrowOrganizationDataAsync() => await _receiveService.EscrowOrganizationDataAsync();
        /// <summary>
        /// 商机项目(含境外商机项目) 国家地区区分  142境内，142以为境外
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/BusinessProject")]
        public async Task<MDMResponseResult> BusinessProjectDataAsync([FromBody] BaseReceiveDataRequestDto<BusinessCpportunityItem> baseReceiveDataRequestDto) => await _receiveService.BusinessProjectDataAsync(baseReceiveDataRequestDto);


        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/Regional")]
        public async Task<MDMResponseResult> RegionalDataAsync([FromBody] BaseReceiveDataRequestDto<RegionalItem> baseReceiveDataRequestDto) => await _receiveService.RegionalDataAsync(baseReceiveDataRequestDto);
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/UnitMeasurement")]
        public async Task<MDMResponseResult> UnitMeasurementDataAsync([FromBody] BaseReceiveDataRequestDto<UnitMeasurementItem> baseReceiveDataRequestDto) => await _receiveService.UnitMeasurementDataAsync(baseReceiveDataRequestDto);
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/ProjectClassification")]
        public async Task<MDMResponseResult> ProjectClassificationDataAsync([FromBody] BaseReceiveDataRequestDto<ProjectClassificationItem> baseReceiveDataRequestDto) => await _receiveService.ProjectClassificationDataAsync(baseReceiveDataRequestDto);
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/DeviceClassCode")]
        public async Task<MDMResponseResult> DeviceClassCodeDataAsync([FromBody] BaseReceiveDataRequestDto<DeviceClassCodeItem> receiveDataMDMRequestDto) => await _receiveService.DeviceClassCodeDataAsync(receiveDataMDMRequestDto);

        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/RegionalCenter")]
        public async Task<MDMResponseResult> RegionalCenterDataAsync([FromBody] BaseReceiveDataRequestDto<RegionalCenterItem> baseReceiveDataRequestDto) => await _receiveService.RegionalCenterDataAsync(baseReceiveDataRequestDto);
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/NationalEconomy")]
        public async Task<MDMResponseResult> NationalEconomyDataAsync([FromBody] BaseReceiveDataRequestDto<NationalEconomyItem> baseReceiveDataRequestDto) => await _receiveService.NationalEconomyDataAsync(baseReceiveDataRequestDto);

        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/InvoiceType")]
        public async Task<MDMResponseResult> InvoiceTypeDataAsync([FromBody] BaseReceiveDataRequestDto<InvoiceTypeItem> receiveDataMDMRequestDto) => await _receiveService.InvoiceTypeDataAsync(receiveDataMDMRequestDto);


        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/ScientifiCNoProject")]
        public async Task<MDMResponseResult> ScientifiCNoProjectDataAsync([FromBody] BaseReceiveDataRequestDto<ScientifiCNoProjectItem> receiveDataMDMRequestDto) => await _receiveService.ScientifiCNoProjectDataAsync(receiveDataMDMRequestDto);
        /// <summary>
        /// 商机项目(不含境外商机项目)国家地区区分  142境内，142以为境外
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/BusinessNoCpportunity")]
        public async Task<MDMResponseResult> BusinessNoCpportunityAsync([FromBody] BaseReceiveDataRequestDto<BusinessCpportunityItem> baseReceiveDataRequestDto) => await _receiveService.BusinessProjectDataAsync(baseReceiveDataRequestDto);
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/RelationalContracts")]
        public async Task<MDMResponseResult> RelationalContractsDataAsync([FromBody] BaseReceiveDataRequestDto<RelationalContractsItem> baseReceiveDataRequestDto) => await _receiveService.RelationalContractsDataAsync(baseReceiveDataRequestDto);
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/ManagementOrganization")]
        public async Task<MDMResponseResult> ManagementOrganizationDataAsync([FromBody] BaseReceiveDataRequestDto<POPMangOrgItem> baseReceiveDataRequestDto) => await _receiveService.ManagementOrganizationDataAsync(baseReceiveDataRequestDto);
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/LouDong")]
        public async Task<MDMResponseResult> LouDongDataAsync([FromBody] BaseReceiveDataRequestDto<LouDongItem> receiveDataMDMRequestDto) => await _receiveService.LouDongDataAsync(receiveDataMDMRequestDto);
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/RoomNumber")]
        public async Task<MDMResponseResult> RoomNumberDataAsync([FromBody] BaseReceiveDataRequestDto<RoomNumberItem> receiveDataMDMRequestDto) => await _receiveService.RoomNumberDataAsync(receiveDataMDMRequestDto);

        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/Language")]
        public async Task<MDMResponseResult> LanguageDataAsync([FromBody] BaseReceiveDataRequestDto<LanguageItem> receiveDataMDMRequestDto) => await _receiveService.LanguageDataAsync(receiveDataMDMRequestDto);
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/DeviceDetailCode")]
        public async Task<MDMResponseResult> DeviceDetailCodeDataAsync([FromBody] BaseReceiveDataRequestDto<DeviceDetailCodeItem> baseReceiveDataRequestDto) => await _receiveService.DeviceDetailCodeDataAsync(baseReceiveDataRequestDto);



        #endregion



        #region 核算部门
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/AccountingDepartment")]
        public async Task<MDMResponseResult> AccountingDepartmentDataAsync(BaseReceiveDataRequestDto<AccountingDepartmentReceiveDto> baseReceiveDataRequestDto) => await _receiveService.AccountingDepartmentDataAsync(baseReceiveDataRequestDto);
        #endregion


        #region 核算机构
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/AccountingOrganization")]
        public async Task<MDMResponseResult> AccountingOrganizationDataAsync(BaseReceiveDataRequestDto<AccountingOrganizationReceiveDto> baseReceive) => await _receiveService.AccountingOrganizationDataAsync(baseReceive);
        #endregion


        #region 行政机构和核算机构映射关系
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/AdministrativeAccountingMapper")]
        public async Task<MDMResponseResult> AdministrativeAccountingMapperDataAsync(BaseReceiveDataRequestDto<AdministrativeAccountingMapperItem> baseReceiveDataRequestDto) => await _receiveService.AdministrativeAccountingMapperDataAsync(baseReceiveDataRequestDto);
        #endregion

        #region 多组织-行政组织
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/AdministrativeOrganization")]
        public async Task<MDMResponseResult> AdministrativeOrganizationDataAsync(BaseReceiveDataRequestDto<AdministrativeOrganizationReceiveRequestDto> baseReceiveDataRequestDto) => await _receiveService.AdministrativeOrganizationDataAsync(baseReceiveDataRequestDto);
        #endregion


        #region 境内行政区划
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/AdministrativeDivision")]
        public async Task<MDMResponseResult> AdministrativeDivisionDataAsync(BaseReceiveDataRequestDto<AdministrativeDivisionItem> baseReceiveDataRequestDto) => await _receiveService.AdministrativeDivisionDataAsync(baseReceiveDataRequestDto);
        #endregion


        #region 获取值域信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseReceiveDataRequestDto"></param>
        /// <returns></returns>
        [HttpPost("/api/mdm/Receive/Common")]
        [UnitOfWork]
        public async Task<MDMResponseResult> ProjectDataAsync([FromBody] BaseReceiveDataRequestDto<ValueDomainReceiveRequestDto> baseReceiveDataRequestDto)
        {
            return await _receiveService.CommonDataAsync(baseReceiveDataRequestDto);
        }
        #endregion

        #region 通用字典数据获取(币种  国家地区  大洲  语种   等)

        #region 接收币种数据
        /// <summary>
        /// 接收币种数据
        /// </summary>
        /// <param name="baseReceiveDataRequestDto"></param>
        /// <returns></returns>
        [HttpPost("/api/mdm/Receive/Currency")]
        public async Task<MDMResponseResult> CurrencyAsync([FromBody] BaseReceiveDataRequestDto<CurrencyReceiveDto> baseReceiveDataRequestDto) => await _receiveService.CurrencyDataAsync(baseReceiveDataRequestDto);
        #endregion

        #region 国家地区

        /// <summary>
        /// 国家地区
        /// </summary>
        /// <returns></returns>
        [HttpPost("/api/mdm/Receive/CountryRegion")]
        [UnitOfWork]
        public async Task<MDMResponseResult> CountryRegionDataAsync([FromBody] BaseReceiveDataRequestDto<CountryRegionReceiveDto> baseReceiveDataRequest) => await _receiveService.CountryRegionDataAsync(baseReceiveDataRequest);
        #endregion


        #region 大洲
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/CountryContinent")]
        public async Task<MDMResponseResult> CountryContinentDataAsync(BaseReceiveDataRequestDto<CountryContinentReceiveDto> baseReceiveDataRequestDto) => await _receiveService.CountryContinentDataAsync(baseReceiveDataRequestDto);

        #endregion

        #endregion

        #region 项目主数据
        /// <summary>
        /// 接收项目主数据 
        /// </summary>
        /// <returns></returns>
        [HttpPost("/api/mdm/Receive/Project")]
        [UnitOfWork]
        public async Task<MDMResponseResult> ProjectDataAsync([FromBody] BaseReceiveDataRequestDto<ProjectItem> receiveDataMDMRequestDto)
        {
            return await _receiveService.ProjectDataAsync(receiveDataMDMRequestDto);
        }
        #endregion

        #region 金融机构主数据
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/FinancialInstitution")]
        public Task<MDMResponseResult> FinancialInstitutionDataAsync(BaseReceiveDataRequestDto<FinancialInstitutionReceiveDto> baseReceiveDataRequestDto) => _receiveService.FinancialInstitutionDataAsync(baseReceiveDataRequestDto);
        #endregion

        #region 往来单位主数据
        /// <summary>
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("/api/mdm/Receive/CorresUnit")]
        public Task<MDMResponseResult> CorresUnitDataAsync(BaseReceiveDataRequestDto<CorresUnitReceiveDto> baseReceiveDataRequestDto) => _receiveService.CorresUnitDataAsync(baseReceiveDataRequestDto);


        #endregion

        #region 接收4A的人员和机构主数据
        /// <summary>
        /// 人员
        /// </summary>
        /// <param name="receiveUserRequestDto"></param>
        /// <returns></returns>
        [Route("/api/4A/Receive/Person")]
        [HttpPost]
        public async Task<MDMResponseResult> PersonAsync([FromBody] ReceiveUserRequestDto receiveUserRequestDto)
        {
            return await _receiveService.PersonDataAsync(receiveUserRequestDto);
        }

        /// <summary>
        /// 机构
        /// </summary>
        /// <param name="receiveInstitutionRequestDto"></param>
        /// <returns></returns>
        [Route("/api/4A/Receive/Institution")]
        [HttpPost]
        public async Task<MDMResponseResult> InstitutionAsync([FromBody] ReceiveInstitutionRequestDto receiveInstitutionRequestDto)
        {
            return await _receiveService.InstitutionDataAsync(receiveInstitutionRequestDto);
        }
        #endregion
    }
}
