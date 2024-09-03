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
            this._dbContext = dbContext;
        }
        /// <summary>
        /// 获取通用字典数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> CommonDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 往来单位主数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> CorresUnitDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-税务代管组织(行政)
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> EscrowOrganizationDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 商机项目(含境外商机项目)
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> BusinessProjectDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 国家地区
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> CountryRegionDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 大洲
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> CountryContinentDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交区域总部
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RegionalDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 常用计量单位
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> UnitMeasurementDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ProjectClassificationDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 金融机构
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> FinancialInstitutionDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 物资设备分类编码
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> DeviceClassCodeDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 核算部门
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AccountingDepartmentDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 中交区域中心
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RegionalCenterDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 银行账号
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> BankCardDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 国民经济行业分类
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> NationalEconomyDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-行政组织
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AdministrativeOrganizationDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 发票类型
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> InvoiceTypeDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 币种
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> CurrencyDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 行政机构和核算机构映射关系
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AdministrativeAccountingMapperDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 项目类
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ProjectDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 科研项目
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ScientifiCNoProjectDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 商机项目(不含境外商机项目)
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> BusinessNoCpportunityDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 委托关系
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RelationalContractsDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 生产经营管理组织
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ManagementOrganizationDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> LouDongDataAsync()
        {
            ///***
            // * 测试写入数据
            // */
            //var tt = new List<LouDong>();
            //var test = new LouDong()
            //{
            //    Id = SnowFlakeAlgorithmUtil.GenerateSnowflakeId(),
            //    ZZSERIAL = "1",
            //    ZBLDG = "2",
            //    ZBLDG_NAME = "3",
            //    ZSTATE = "4",
            //    ZFORMATINF = "5",
            //    ZSYSTEM = "6",
            //    ZPROJECT = "7"
            //};
            //tt.Add(test);
            //var x = _dbContext.Storageable(tt).ToStorage();
            //await x.AsInsertable.ExecuteCommandAsync();//不存在插入
            //await x.AsUpdateable.ExecuteCommandAsync();//存在更新
            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 房号
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> RoomNumberDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 境内行政区划
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AdministrativeDivisionDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 语言语种
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> LanguageDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 物资设备明细编码
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> DeviceDetailCodeDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
        /// <summary>
        /// 多组织-核算机构
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> AccountingOrganizationDataAsync()
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
            await Console.Out.WriteLineAsync("接收的数据:" + receiveUserRequestDto.ToJson());
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;



        }
        /// <summary>
        /// 机构主数据
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> InstitutionDataAsync()
        {

            var responseAjaxResult = new ResponseAjaxResult<bool>();
            responseAjaxResult.SuccessResult(true);
            return responseAjaxResult;
        }
    }
}
