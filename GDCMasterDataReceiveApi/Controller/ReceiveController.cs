using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using GDCMasterDataReceiveApi.Domain.Shared.WebSecurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilsSharp;

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
        public Task<ResponseAjaxResult<bool>> CommonDataAsync() => _receiveService.CommonDataAsync();
        /// <summary>
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("CorresUnit")]
        public Task<ResponseAjaxResult<bool>> CorresUnitDataAsync() => _receiveService.CorresUnitDataAsync();
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("EscrowOrganization")]
        public Task<ResponseAjaxResult<bool>> EscrowOrganizationDataAsync() => _receiveService.EscrowOrganizationDataAsync();
        /// <summary>
        /// 商机项目(含境外商机项目)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BusinessProject")]
        public Task<ResponseAjaxResult<bool>> BusinessProjectDataAsync() => _receiveService.BusinessProjectDataAsync();
        /// <summary>
        /// 国家地区
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("CountryRegion")]
        public Task<ResponseAjaxResult<bool>> CountryRegionDataAsync() => _receiveService.CountryRegionDataAsync();
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("CountryContinent")]
        public Task<ResponseAjaxResult<bool>> CountryContinentDataAsync() => _receiveService.CountryContinentDataAsync();
        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Regional")]
        public Task<ResponseAjaxResult<bool>> RegionalDataAsync() => _receiveService.RegionalDataAsync();
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("UnitMeasurement")]
        public Task<ResponseAjaxResult<bool>> UnitMeasurementDataAsync() => _receiveService.UnitMeasurementDataAsync();
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ProjectClassification")]
        public Task<ResponseAjaxResult<bool>> ProjectClassificationDataAsync() => _receiveService.ProjectClassificationDataAsync();
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("FinancialInstitution")]
        public Task<ResponseAjaxResult<bool>> FinancialInstitutionDataAsync() => _receiveService.FinancialInstitutionDataAsync();
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("DeviceClassCode")]
        public Task<ResponseAjaxResult<bool>> DeviceClassCodeDataAsync() => _receiveService.DeviceClassCodeDataAsync();
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AccountingDepartment")]
        public Task<ResponseAjaxResult<bool>> AccountingDepartmentDataAsync() => _receiveService.AccountingDepartmentDataAsync();
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RegionalCenter")]
        public Task<ResponseAjaxResult<bool>> RegionalCenterDataAsync() => _receiveService.RegionalCenterDataAsync();
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BankCard")]
        public Task<ResponseAjaxResult<bool>> BankCardDataAsync() => _receiveService.BankCardDataAsync();
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("NationalEconomy")]
        public Task<ResponseAjaxResult<bool>> NationalEconomyDataAsync() => _receiveService.NationalEconomyDataAsync();
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeOrganization")]
        public Task<ResponseAjaxResult<bool>> AdministrativeOrganizationDataAsync() => _receiveService.AdministrativeOrganizationDataAsync();
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("InvoiceType")]
        public Task<ResponseAjaxResult<bool>> InvoiceTypeDataAsync() => _receiveService.InvoiceTypeDataAsync();
        /// <summary>
        /// 币种
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Currency")]
        public Task<ResponseAjaxResult<bool>> CurrencyDataAsync() => _receiveService.CurrencyDataAsync();
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeAccountingMapper")]
        public Task<ResponseAjaxResult<bool>> AdministrativeAccountingMapperDataAsync() => _receiveService.AdministrativeAccountingMapperDataAsync();
        /// <summary>
        /// 项目类
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Project")]
        public Task<ResponseAjaxResult<bool>> ProjectDataAsync() => _receiveService.ProjectDataAsync();
        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ScientifiCNoProject")]
        public Task<ResponseAjaxResult<bool>> ScientifiCNoProjectDataAsync() => _receiveService.ScientifiCNoProjectDataAsync();
        /// <summary>
        /// 商机项目(不含境外商机项目)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BusinessNoCpportunity")]
        public Task<ResponseAjaxResult<bool>> BusinessNoCpportunityDataAsync() => _receiveService.BusinessNoCpportunityDataAsync();
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RelationalContracts")]
        public Task<ResponseAjaxResult<bool>> RelationalContractsDataAsync() => _receiveService.RelationalContractsDataAsync();
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ManagementOrganization")]
        public Task<ResponseAjaxResult<bool>> ManagementOrganizationDataAsync() => _receiveService.ManagementOrganizationDataAsync();
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [AllowAnonymous]
        [HttpPost("LouDong")]
        public Task<ResponseAjaxResult<bool>> LouDongDataAsync() => _receiveService.LouDongDataAsync();
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RoomNumber")]
        public Task<ResponseAjaxResult<bool>> RoomNumberDataAsync() => _receiveService.RoomNumberDataAsync();
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeDivision")]
        public Task<ResponseAjaxResult<bool>> AdministrativeDivisionDataAsync() => _receiveService.AdministrativeDivisionDataAsync();
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Language")]
        public Task<ResponseAjaxResult<bool>> LanguageDataAsync() => _receiveService.LanguageDataAsync();
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("DeviceDetailCode")]
        public Task<ResponseAjaxResult<bool>> DeviceDetailCodeDataAsync() => _receiveService.DeviceDetailCodeDataAsync();
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AccountingOrganization")]
        public Task<ResponseAjaxResult<bool>> AccountingOrganizationDataAsync() => _receiveService.AccountingOrganizationDataAsync();
     
        /// <summary>
        /// 机构主数据
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Institution")]
        public Task<ResponseAjaxResult<bool>> InstitutionDataAsync() => _receiveService.InstitutionDataAsync();
        #endregion

        #region 拉取4A的人员和机构数据
        [Route("api/4A/Receive/Person")]
        [HttpPost]
        public async Task<ResponseAjaxResult<bool>>  PersonAsync([FromBody] ReceiveUserRequestDto receiveUserRequestDto) 
        {
            return await _receiveService.PersonDataAsync(receiveUserRequestDto);
        }
        #endregion
    }
}
