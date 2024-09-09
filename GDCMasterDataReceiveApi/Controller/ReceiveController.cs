using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.Institution;
using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.CountryRegion;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Currency;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.Project;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;
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
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("CorresUnit")]
        public Task<MDMResponseResult> CorresUnitDataAsync() => _receiveService.CorresUnitDataAsync();
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("EscrowOrganization")]
        public Task<MDMResponseResult> EscrowOrganizationDataAsync() => _receiveService.EscrowOrganizationDataAsync();
        /// <summary>
        /// 商机项目(含境外商机项目)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BusinessProject")]
        public Task<MDMResponseResult> BusinessProjectDataAsync() => _receiveService.BusinessProjectDataAsync();
        /// <summary>
        /// 国家地区
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("CountryRegion")]
        public Task<ResponseAjaxResult<ResponseResult>> CountryRegionDataAsync([FromBody] RequestResult<CountryRegionReceiveDto> requestDto) => _receiveService.CountryRegionDataAsync(requestDto);
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("CountryContinent")]
        public Task<MDMResponseResult> CountryContinentDataAsync() => _receiveService.CountryContinentDataAsync();
        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Regional")]
        public Task<MDMResponseResult> RegionalDataAsync() => _receiveService.RegionalDataAsync();
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("UnitMeasurement")]
        public Task<MDMResponseResult> UnitMeasurementDataAsync() => _receiveService.UnitMeasurementDataAsync();
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ProjectClassification")]
        public Task<MDMResponseResult> ProjectClassificationDataAsync() => _receiveService.ProjectClassificationDataAsync();
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("FinancialInstitution")]
        public Task<MDMResponseResult> FinancialInstitutionDataAsync() => _receiveService.FinancialInstitutionDataAsync();
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("DeviceClassCode")]
        public Task<MDMResponseResult> DeviceClassCodeDataAsync() => _receiveService.DeviceClassCodeDataAsync();
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AccountingDepartment")]
        public Task<MDMResponseResult> AccountingDepartmentDataAsync() => _receiveService.AccountingDepartmentDataAsync();
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RegionalCenter")]
        public Task<MDMResponseResult> RegionalCenterDataAsync() => _receiveService.RegionalCenterDataAsync();
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BankCard")]
        public Task<MDMResponseResult> BankCardDataAsync() => _receiveService.BankCardDataAsync();
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("NationalEconomy")]
        public Task<MDMResponseResult> NationalEconomyDataAsync() => _receiveService.NationalEconomyDataAsync();
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeOrganization")]
        public Task<MDMResponseResult> AdministrativeOrganizationDataAsync() => _receiveService.AdministrativeOrganizationDataAsync();
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("InvoiceType")]
        public Task<MDMResponseResult> InvoiceTypeDataAsync() => _receiveService.InvoiceTypeDataAsync();
    
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeAccountingMapper")]
        public Task<MDMResponseResult> AdministrativeAccountingMapperDataAsync() => _receiveService.AdministrativeAccountingMapperDataAsync();
        
        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ScientifiCNoProject")]
        public Task<MDMResponseResult> ScientifiCNoProjectDataAsync() => _receiveService.ScientifiCNoProjectDataAsync();
        /// <summary>
        /// 商机项目(不含境外商机项目)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BusinessNoCpportunity")]
        public Task<MDMResponseResult> BusinessNoCpportunityDataAsync() => _receiveService.BusinessNoCpportunityDataAsync();
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RelationalContracts")]
        public Task<MDMResponseResult> RelationalContractsDataAsync() => _receiveService.RelationalContractsDataAsync();
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ManagementOrganization")]
        public Task<MDMResponseResult> ManagementOrganizationDataAsync() => _receiveService.ManagementOrganizationDataAsync();
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [AllowAnonymous]
        [HttpPost("LouDong")]
        public Task<MDMResponseResult> LouDongDataAsync() => _receiveService.LouDongDataAsync();
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RoomNumber")]
        public Task<MDMResponseResult> RoomNumberDataAsync() => _receiveService.RoomNumberDataAsync();
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeDivision")]
        public Task<MDMResponseResult> AdministrativeDivisionDataAsync() => _receiveService.AdministrativeDivisionDataAsync();
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Language")]
        public Task<MDMResponseResult> LanguageDataAsync() => _receiveService.LanguageDataAsync();
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("DeviceDetailCode")]
        public Task<MDMResponseResult> DeviceDetailCodeDataAsync() => _receiveService.DeviceDetailCodeDataAsync();
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AccountingOrganization")]
        public Task<MDMResponseResult> AccountingOrganizationDataAsync() => _receiveService.AccountingOrganizationDataAsync();

        ///// <summary>
        ///// 机构主数据
        ///// </summary>
        ///// <returns></returns>
        //[UnitOfWork]
        //[HttpPost("Institution")]
        //public Task<MDMResponseResult> InstitutionDataAsync() => _receiveService.InstitutionDataAsync();



        #endregion




        #region 通用字典数据获取(币种  国家地区  大洲  语种   等)
        /// <summary>
        /// 接收币种数据
        /// </summary>
        /// <param name="baseReceiveDataRequestDto"></param>
        /// <returns></returns>
        [HttpPost("Currency")]
        public async Task<MDMResponseResult> CurrencyAsync([FromBody] BaseReceiveDataRequestDto<CurrencyReceiveDto>  baseReceiveDataRequestDto) => await _receiveService.CurrencyDataAsync(baseReceiveDataRequestDto);
        #endregion


        #region 项目主数据
        /// <summary>
        /// 接收项目主数据 
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Project")]
        public async Task<MDMResponseResult> ProjectDataAsync([FromBody] BaseReceiveDataRequestDto<ProjectItem>  receiveDataMDMRequestDto) => await _receiveService.ProjectDataAsync(receiveDataMDMRequestDto);
        #endregion


        #region 接收4A的人员和机构数据
        /// <summary>
        /// 人员
        /// </summary>
        /// <param name="receiveUserRequestDto"></param>
        /// <returns></returns>
        [Route("api/4A/Receive/Person")]
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
        [Route("api/4A/Receive/Institution")]
        [HttpPost]
        public async Task<MDMResponseResult> InstitutionAsync([FromBody] ReceiveInstitutionRequestDto receiveInstitutionRequestDto)
        {
            return await _receiveService.InstitutionDataAsync(receiveInstitutionRequestDto);
        }
        #endregion
    }
}
