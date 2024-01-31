using GHMonitoringCenterApi.Application.Contracts.Dto.JjtSendMsg;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Application.Contracts.IService.JjtSendMessage;
using GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport;
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
        /// 依赖注入
        /// </summary>
        /// <param name="jjtSendMessageService"></param>
        public ProductionValueImportService(IJjtSendMessageService jjtSendMessageService)
        {
            this._jjtSendMessageService = jjtSendMessageService;
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
            var responseAjaxResult=new ResponseAjaxResult<bool>(); 
            //调用监控中心图片信息方法
            var getData = await _jjtSendMessageService.JjtTextCardMsgDetailsAsync();

            return responseAjaxResult;
        }
    }
}
