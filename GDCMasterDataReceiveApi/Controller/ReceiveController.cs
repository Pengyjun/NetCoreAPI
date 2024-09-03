using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
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
        public Task<ResponseAjaxResult<bool>> CommonDataAsync() => _receiveService.CommonData();
        /// <summary>
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("CorresUnit")]
        public Task<ResponseAjaxResult<bool>> CorresUnitDataAsync() => _receiveService.CorresUnitData();
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("EscrowOrganization")]
        public Task<ResponseAjaxResult<bool>> EscrowOrganizationDataAsync() => _receiveService.EscrowOrganizationData();
        /// <summary>
        /// 商机项目(含境外商机项目)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BusinessProject")]
        public Task<ResponseAjaxResult<bool>> BusinessProjectDataAsync() => _receiveService.BusinessProjectData();
        /// <summary>
        /// 国家地区
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("CountryRegion")]
        public Task<ResponseAjaxResult<bool>> CountryRegionDataAsync() => _receiveService.CountryRegionData();
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("CountryContinent")]
        public Task<ResponseAjaxResult<bool>> CountryContinentDataAsync() => _receiveService.CountryContinentData();
        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Regional")]
        public Task<ResponseAjaxResult<bool>> RegionalDataAsync() => _receiveService.RegionalData();
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("UnitMeasurement")]
        public Task<ResponseAjaxResult<bool>> UnitMeasurementDataAsync() => _receiveService.UnitMeasurementData();
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ProjectClassification")]
        public Task<ResponseAjaxResult<bool>> ProjectClassificationDataAsync() => _receiveService.ProjectClassificationData();
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("FinancialInstitution")]
        public Task<ResponseAjaxResult<bool>> FinancialInstitutionDataAsync() => _receiveService.FinancialInstitutionData();
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("DeviceClassCode")]
        public Task<ResponseAjaxResult<bool>> DeviceClassCodeDataAsync() => _receiveService.DeviceClassCodeData();
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AccountingDepartment")]
        public Task<ResponseAjaxResult<bool>> AccountingDepartmentDataAsync() => _receiveService.AccountingDepartmentData();
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RegionalCenter")]
        public Task<ResponseAjaxResult<bool>> RegionalCenterDataAsync() => _receiveService.RegionalCenterData();
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BankCard")]
        public Task<ResponseAjaxResult<bool>> BankCardDataAsync() => _receiveService.BankCardData();
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("NationalEconomy")]
        public Task<ResponseAjaxResult<bool>> NationalEconomyDataAsync() => _receiveService.NationalEconomyData();
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeOrganization")]
        public Task<ResponseAjaxResult<bool>> AdministrativeOrganizationDataAsync() => _receiveService.AdministrativeOrganizationData();
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("InvoiceType")]
        public Task<ResponseAjaxResult<bool>> InvoiceTypeDataAsync() => _receiveService.InvoiceTypeData();
        /// <summary>
        /// 币种
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Currency")]
        public Task<ResponseAjaxResult<bool>> CurrencyDataAsync() => _receiveService.CurrencyData();
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeAccountingMapper")]
        public Task<ResponseAjaxResult<bool>> AdministrativeAccountingMapperDataAsync() => _receiveService.AdministrativeAccountingMapperData();
        /// <summary>
        /// 项目类
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Project")]
        public Task<ResponseAjaxResult<bool>> ProjectDataAsync() => _receiveService.ProjectData();
        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ScientifiCNoProject")]
        public Task<ResponseAjaxResult<bool>> ScientifiCNoProjectDataAsync() => _receiveService.ScientifiCNoProjectData();
        /// <summary>
        /// 商机项目(不含境外商机项目)
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("BusinessNoCpportunity")]
        public Task<ResponseAjaxResult<bool>> BusinessNoCpportunityDataAsync() => _receiveService.BusinessNoCpportunityData();
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RelationalContracts")]
        public Task<ResponseAjaxResult<bool>> RelationalContractsDataAsync() => _receiveService.RelationalContractsData();
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("ManagementOrganization")]
        public Task<ResponseAjaxResult<bool>> ManagementOrganizationDataAsync() => _receiveService.ManagementOrganizationData();
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("LouDong")]
        public Task<ResponseAjaxResult<bool>> LouDongDataAsync() => _receiveService.LouDongData();
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("RoomNumber")]
        public Task<ResponseAjaxResult<bool>> RoomNumberDataAsync() => _receiveService.RoomNumberData();
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AdministrativeDivision")]
        public Task<ResponseAjaxResult<bool>> AdministrativeDivisionDataAsync() => _receiveService.AdministrativeDivisionData();
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Language")]
        public Task<ResponseAjaxResult<bool>> LanguageDataAsync() => _receiveService.LanguageData();
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("DeviceDetailCode")]
        public Task<ResponseAjaxResult<bool>> DeviceDetailCodeDataAsync() => _receiveService.DeviceDetailCodeData();
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("AccountingOrganization")]
        public Task<ResponseAjaxResult<bool>> AccountingOrganizationDataAsync() => _receiveService.AccountingOrganizationData();
        /// <summary>
        /// 人员主数据
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Person")]
        public Task<ResponseAjaxResult<bool>> PersonDataAsync([FromBody]List<ReceiveUserRequestDto> receiveUserRequestDto) => _receiveService.PersonDataAsync(receiveUserRequestDto);
        /// <summary>
        /// 机构主数据
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost("Institution")]
        public Task<ResponseAjaxResult<bool>> InstitutionDataAsync() => _receiveService.InstitutionData();
        #endregion



        #region 拉取4A的人员和机构数据
        [Route("api/4A/Receive/Person")]
        [HttpPost]
        public async Task<bool> PersonAsync([FromBody] ReceiveUserRequestDto receiveUserRequestDto) 
        {
            await Console.Out.WriteLineAsync("接收的数据："+ receiveUserRequestDto.ToJson());
            return true;
        }
        #endregion
    }
}
