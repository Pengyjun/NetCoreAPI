using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CorresUnit;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryContinent;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.DeviceClassCode;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.InvoiceType;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Language;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.FinancialInstitution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.RoomNumber;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ScientifiCNoProject;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// 获取通用字典数据
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Common")]
        public Task<MDMResponseResult> CommonDataAsync() => _receiveService.CommonDataAsync();
       
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("EscrowOrganization")]
        public async Task<MDMResponseResult> EscrowOrganizationDataAsync() => await _receiveService.EscrowOrganizationDataAsync();
        /// <summary>
        /// 商机项目(含境外商机项目)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BusinessProject")]
        public Task<MDMResponseResult> BusinessProjectDataAsync() => _receiveService.BusinessProjectDataAsync();


        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Regional")]
        public async Task<MDMResponseResult> RegionalDataAsync() => await _receiveService.RegionalDataAsync();
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("UnitMeasurement")]
        public async Task<MDMResponseResult> UnitMeasurementDataAsync() => await _receiveService.UnitMeasurementDataAsync();
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ProjectClassification")]
        public async Task<MDMResponseResult> ProjectClassificationDataAsync() => await _receiveService.ProjectClassificationDataAsync();
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("DeviceClassCode")]
        public async Task<MDMResponseResult> DeviceClassCodeDataAsync([FromBody] BaseReceiveDataRequestDto<DeviceClassCodeItem> receiveDataMDMRequestDto) => await _receiveService.DeviceClassCodeDataAsync(receiveDataMDMRequestDto);
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AccountingDepartment")]
        public async Task<MDMResponseResult> AccountingDepartmentDataAsync() => await _receiveService.AccountingDepartmentDataAsync();
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RegionalCenter")]
        public async Task<MDMResponseResult> RegionalCenterDataAsync() => await _receiveService.RegionalCenterDataAsync();
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BankCard")]
        public async Task<MDMResponseResult> BankCardDataAsync() => await _receiveService.BankCardDataAsync();
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("NationalEconomy")]
        public async Task<MDMResponseResult> NationalEconomyDataAsync() => await _receiveService.NationalEconomyDataAsync();
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeOrganization")]
        public async Task<MDMResponseResult> AdministrativeOrganizationDataAsync() => await _receiveService.AdministrativeOrganizationDataAsync();
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("InvoiceType")]
        public async Task<MDMResponseResult> InvoiceTypeDataAsync([FromBody] BaseReceiveDataRequestDto<InvoiceTypeItem> receiveDataMDMRequestDto) => await _receiveService.InvoiceTypeDataAsync(receiveDataMDMRequestDto);

        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeAccountingMapper")]
        public async Task<MDMResponseResult> AdministrativeAccountingMapperDataAsync() => await _receiveService.AdministrativeAccountingMapperDataAsync();

        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ScientifiCNoProject")]
        public async Task<MDMResponseResult> ScientifiCNoProjectDataAsync([FromBody] BaseReceiveDataRequestDto<ScientifiCNoProjectItem> receiveDataMDMRequestDto) => await _receiveService.ScientifiCNoProjectDataAsync(receiveDataMDMRequestDto);
        /// <summary>
        /// 商机项目(不含境外商机项目)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BusinessNoCpportunity")]
        public async Task<MDMResponseResult> BusinessNoCpportunityDataAsync() => await _receiveService.BusinessNoCpportunityDataAsync();
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RelationalContracts")]
        public async Task<MDMResponseResult> RelationalContractsDataAsync() => await _receiveService.RelationalContractsDataAsync();
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ManagementOrganization")]
        public async Task<MDMResponseResult> ManagementOrganizationDataAsync() => await _receiveService.ManagementOrganizationDataAsync();
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [AllowAnonymous]
        [HttpPost("LouDong")]
        public async Task<MDMResponseResult> LouDongDataAsync() => await _receiveService.LouDongDataAsync();
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RoomNumber")]
        public async Task<MDMResponseResult> RoomNumberDataAsync([FromBody] BaseReceiveDataRequestDto<RoomNumberItem> receiveDataMDMRequestDto) => await _receiveService.RoomNumberDataAsync(receiveDataMDMRequestDto);
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeDivision")]
        public async Task<MDMResponseResult> AdministrativeDivisionDataAsync() => await _receiveService.AdministrativeDivisionDataAsync();
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Language")]
        public async Task<MDMResponseResult> LanguageDataAsync([FromBody] BaseReceiveDataRequestDto<LanguageItem> receiveDataMDMRequestDto) => await _receiveService.LanguageDataAsync(receiveDataMDMRequestDto);
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("DeviceDetailCode")]
        public async Task<MDMResponseResult> DeviceDetailCodeDataAsync() => await _receiveService.DeviceDetailCodeDataAsync();
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AccountingOrganization")]
        public async Task<MDMResponseResult> AccountingOrganizationDataAsync() => await _receiveService.AccountingOrganizationDataAsync();

        ///// <summary>
        ///// 机构主数据
        ///// </summary>
        ///// <returns></returns>
        //[UnitOfWork]
        //[HttpPost("Institution")]
        //public async Task<MDMResponseResult> InstitutionDataAsync() => await _receiveService.InstitutionDataAsync();



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

        [HttpPost("CountryRegion")]
        public async Task<MDMResponseResult> CountryRegionDataAsync([FromBody] BaseReceiveDataRequestDto<CountryRegionReceiveDto> baseReceiveDataRequest) => await _receiveService.CountryRegionDataAsync(baseReceiveDataRequest);
        #endregion


        #region 大洲
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("CountryContinent")]
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
        public async Task<MDMResponseResult> ProjectDataAsync([FromBody] BaseReceiveDataRequestDto<ProjectItem>  receiveDataMDMRequestDto) => await _receiveService.ProjectDataAsync(receiveDataMDMRequestDto);
        #endregion

        #region 金融机构主数据
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("FinancialInstitution")]
        public Task<MDMResponseResult> FinancialInstitutionDataAsync(BaseReceiveDataRequestDto<FinancialInstitutionReceiveDto> baseReceiveDataRequestDto) => _receiveService.FinancialInstitutionDataAsync(baseReceiveDataRequestDto);
        #endregion

        #region 往来单位主数据
        /// <summary>
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("CorresUnit")]
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
