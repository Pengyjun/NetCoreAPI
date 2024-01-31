using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.IService.JjtSendMessage;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.IService.Project;
using GHMonitoringCenterApi.Domain.Shared;

namespace GHMonitoringCenterApi.Application.Service.ProductionValueImport
{

    /// <summary>
    ///  生产日报每天推送和节假日日报推送历史数据导出实现层
    /// </summary>
    public class ProductionValueImportService : IProductionValueImportService
    {
        /// <summary>
        /// 注入发消息内容
        /// </summary>
        public IJjtSendMessageService _jjtSendMessageService { get; set; }
        /// <summary>
        /// 注入元旦期间在建项目带班生产动态
        /// </summary>
        public IProjectReportService _projectReportService { get; set; }
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="jjtSendMessageService"></param>
        public ProductionValueImportService(IJjtSendMessageService jjtSendMessageService, IProjectReportService projectReportService)
        {
            this._jjtSendMessageService = jjtSendMessageService;
            this._projectReportService = projectReportService;
        }
        /// <summary>
        /// 导出历史数据产值信息
        /// </summary>
        /// <param name="importHistoryProductionValuesRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseAjaxResult<JjtSendMessageMonitoringDayReportResponseDto>> ImportProductionValuesAsync(ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Excel 智慧运营监控中心图片数据写入表
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<bool>> ExcelJJtSendMessageWriteAsync()
        {
            var responseAjaxResult = new ResponseAjaxResult<bool>();

            //调用监控中心图片信息方法
            var getData = await _jjtSendMessageService.JjtTextCardMsgDetailsAsync();

            //各个公司基本项目情况
            var excelCompanyProjectBasePoduction = getData.Data.projectBasePoduction.CompanyProjectBasePoductions;
            //各个公司基本产值情况
            var excelCompanyBasePoductionValue = getData.Data.projectBasePoduction.CompanyBasePoductionValues;
            //自有船施工运转情况
            var excelOwnerShipBuildInfo = getData.Data.OwnerShipBuildInfo.companyShipBuildInfos;
            //各个公司自有船施工产值情况集合
            var excelCompanyShipProductionValueInfo = getData.Data.OwnerShipBuildInfo.companyShipProductionValueInfos;
            //前五船舶产值
            var excelShipProductionValue = getData.Data.OwnerShipBuildInfo.companyShipTopFiveInfoList;
            //特殊情况
            var excelSpecialProjectInfo = getData.Data.SpecialProjectInfo;
            //各单位填报情况
            var excelCompanyWriteReportInfo = getData.Data.CompanyWriteReportInfos;
            //项目生产数据存在不完整部分主要是以下项目未填报
            var excelCompanyUnWriteReportInfo = getData.Data.CompanyUnWriteReportInfos;
            //船舶生产数据存在不完整部分主要是项目部未填报以下船舶
            var excelCompanyShipUnWriteReportInfo = getData.Data.CompanyShipUnWriteReportInfos;


            //调用元旦期间各在建项目带班生产动态
            var getNewYearDayData = await _projectReportService.GetProjectShiftProductionAsync();

            //项目带班生产动态已填报项目
            var excelProjectShiftProductionInfo = getNewYearDayData.Data.projectShiftProductionInfos;
            //项目带班生产动态未填报项目
            var excelUnProjectShitInfo = getNewYearDayData.Data.unProjectShitInfos;



            return responseAjaxResult;
        }
    }
}
