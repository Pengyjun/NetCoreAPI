using GHMonitoringCenterApi.Application.Contracts.Dto.ProductionValueImport;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;

namespace GHMonitoringCenterApi.Application.Contracts.IService.ProductionValueImport
{
    /// <summary>
    /// 生产日报每天推送和节假日日报推送历史数据导出服务层
    /// </summary>
    public interface IProductionValueImportService
    {
        /// <summary>
        /// 导出历史数据
        /// </summary>
        /// <param name="importHistoryProductionValuesRequestDto">请求参数</param>
        /// <returns></returns>
        //Stream ImportProductionValues(ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto);
        /// <summary>
        /// 根据日期读取所含日期内的数据
        /// </summary>
        /// <param name="importHistoryProductionValuesRequestDto"></param>
        /// <returns></returns>
        //ImportHistoryProductionValuesResponseDto ReadImportProductionData(ImportHistoryProductionValuesRequestDto importHistoryProductionValuesRequestDto);
        /// <summary>
        /// Excel 智慧运营监控中心图片数据写入表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> ExcelJJtSendMessageWriteAsync(DateTime date);
        /// <summary>
        /// 生产监控日报
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ProductionDayReportHistoryResponseDto>>> ExcelJJtSendMessageAsync(int year,int month);
        /// <summary>
        /// 节假日数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>

        Task<ResponseAjaxResult<ProductionDayReportHistoryResponseDto>> ExcelJJtSendMessageHolidaysAsync(int year, int month);
    }
}
