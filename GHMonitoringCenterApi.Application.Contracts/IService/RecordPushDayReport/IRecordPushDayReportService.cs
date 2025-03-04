using GHMonitoringCenterApi.Domain.Shared;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Application.Contracts.Dto.RecordPushDayReport;

namespace GHMonitoringCenterApi.Application.Contracts.IService.RecordPushDayReport
{
    /// <summary>
    /// 每天推送日报结果记录
    /// </summary>
    public interface IRecordPushDayReportService
    {

        /// <summary>
        ///获取每天推送日报结果记录
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<RecordPushDayReportResponseDto>>> GetRecordPushDayReportListAsync(RecordPushDayReportRequestDto requestDto);
        

    }

}
