using GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;
using GDCMasterDataReceiveApi.Domain.Shared;
using SqlSugar;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application.Service.ReceiveService
{
    /// <summary>
    /// 接收主数据推送接口实现
    /// </summary>
    public class ReceiveService : IReceiveService
    {
        private readonly ISqlSugarClient _dbContext;
        /// <summary>
        /// 注入上下文
        /// </summary>
        public ReceiveService(ISqlSugarClient dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// 获取通用字典数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> CommonData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> CorresUnitData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> EscrowOrganizationData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 商机项目(含境外商机项目)
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> BusinessProjectData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 国家地区
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> CountryRegionData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> CountryContinentData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RegionalData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> UnitMeasurementData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ProjectClassificationData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> FinancialInstitutionData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> DeviceClassCodeData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AccountingDepartmentData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RegionalCenterData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> BankCardData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> NationalEconomyData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AdministrativeOrganizationData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> InvoiceTypeData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 币种
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> CurrencyData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AdministrativeAccountingMapperData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 项目类
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ProjectData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ScientifiCNoProjectData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 商机项目(不含境外商机项目)
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> BusinessNoCpportunityData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RelationalContractsData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ManagementOrganizationData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> LouDongData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RoomNumberData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AdministrativeDivisionData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> LanguageData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> DeviceDetailCodeData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AccountingOrganizationData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 人员主数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> PersonDataAsync(List<ReceiveUserRequestDto> receiveUserRequestDto)
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            await Console.Out.WriteLineAsync("接收的数据:"+ receiveUserRequestDto.ToJson());
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;



        }
        /// <summary>
        /// 机构主数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> InstitutionData()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
    }
}
