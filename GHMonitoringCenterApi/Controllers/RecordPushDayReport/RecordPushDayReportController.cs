
using GHMonitoringCenterApi.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GHMonitoringCenterApi.Application.Contracts.IService.Push;
using GHMonitoringCenterApi.Filters;
using GHMonitoringCenterApi.Application.Contracts.IService.RecordPushDayReport;
using GHMonitoringCenterApi.Application.Contracts.Dto.RecordPushDayReport;

namespace GHMonitoringCenterApi.Controllers.RecordPushDayReport;

/// <summary>
///  每天推送日报结果记录
/// </summary>
[Route("api/[controller]")]
[ApiController]
[ThreadLockFilter]
public class RecordPushDayReportController : BaseController
{
    /// <summary>
    /// 推送日报结果记录业务层
    /// </summary>

    private readonly IRecordPushDayReportService _recordPushDayReportService;

    /// <summary>
    /// 推送日报结果记录
    /// </summary>
    public RecordPushDayReportController(IRecordPushDayReportService recordPushDayReportService)
    {
        _recordPushDayReportService = recordPushDayReportService;
    }
    /// <summary>
    ///获取每天推送日报结果
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetRecordPushDayReportList")]
    public async Task<ResponseAjaxResult<List<RecordPushDayReportResponseDto>>> GetRecordPushDayReportListAsync([FromQuery] RecordPushDayReportRequestDto requestDto)
    {
        return await _recordPushDayReportService.GetRecordPushDayReportListAsync(requestDto);
    }

    /// <summary>
    ///获取上一天推送日报结果
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetPrevDatePushDayReport")]
    public async Task<ResponseAjaxResult<RecordPushDayReportResponseDto>> GetPrevDatePushDayReportAsync([FromQuery] SearchRecordPushDayReportRequestDto requestDto)
    {
        return await _recordPushDayReportService.GetPrevDatePushDayReportAsync(requestDto);
    }




}

