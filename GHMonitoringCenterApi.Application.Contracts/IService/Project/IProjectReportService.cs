
using GHMonitoringCenterApi.Application.Contracts.Dto.File;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Application.Contracts.Dto.Ship;
using GHMonitoringCenterApi.Domain.Shared;

namespace GHMonitoringCenterApi.Application.Contracts.IService.Project
{
    /// <summary>
    ///  项目-填报业务层
    /// </summary>
    public interface IProjectReportService
    {
        /// <summary>
        /// 保存一个项目日报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveProjectDayReportAsync(AddOrUpdateDayReportRequestDto model);

        /// <summary>
        /// 搜索一个项目日报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectDayReportResponseDto>> SearchProjectDayReportAsync(ProjectDayReportRequestDto model);

        /// <summary>
        /// 新增或修改一个安监日报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveSafeSupervisionDayReportAsync(AddOrUpdateSafeSupervisionDayReportRequestDto model);

        /// <summary>
        /// 搜索一个安监日报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<SafeSupervisionDayReportResponseDto>> SearchSafeSupervisionDayReportAsync(SafeSupervisionDayReportRequestDto model);

        /// <summary>
        /// 新增或修改一个船舶日报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveShipDayReportAsync(SaveShipDayReportRequestDto model);

        /// <summary>
        /// 搜索一个船舶日报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<ShipDayReportResponseDto>> SearchShipDayReportAsync(ShipDayReportRequestDto model);

        /// <summary>
        ///保存一条项目月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveProjectMonthReportAsync(SaveProjectMonthReportRequestDto model);

        /// <summary>
        /// 搜索一条项目月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectMonthReportResponseDto>> SearchProjectMonthReportAsync(ProjectMonthReportRequestDto model);

        /// <summary>
        /// 项目月报状态是否已完成
        /// </summary>
        /// <returns></returns>
        Task<bool> IsFinishMonthReport(Guid projectId, int dateMonth);

        /// <summary>
        ///保存一条舶动态月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveShipDynamicMonthReportAsync(SaveShipDynamicMonthReportRequestDto model);

        /// <summary>
        /// 搜索一条船舶动态月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<ShipDynamicMonthReportResponseDto>> SearchShipDynamicMonthReportAsync(ShipDynamicMonthReportRequestDto model);

        /// <summary>
        /// 保存一条自有船舶月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveOwnerShipMonthReportAsync(SaveOwnerShipMonthReportRequestDto model);

        /// <summary>
        /// 搜索一条自有船舶月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<OwnerShipMonthReportResponseDto>> SearchOwnerShipMonthReportAsync(OwnerShipMonthReportRequestDto model);

        /// <summary>
        /// 搜索月报船舶列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<ShipsForReportResponseDto>> SearchShipsForMonthReportAsync(ShipsForReportRequestDto model);

        /// <summary>
        /// 保存一条分包船舶月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveSubShipMonthReportAsync(SaveSubShipMonthReportRequestDto model);

        /// <summary>
        /// 搜索一条分包船舶月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<SubShipMonthReportResponseDto>> SearchSubShipMonthReportAsync(SubShipMonthReportRequestDto model);

        /// <summary>
        /// 搜索未填项目日报的项目列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<UnReportProjectResponseDto>>> SearchUnReportProjectsAsync(UnReportProjectsRequestDto model);

        /// <summary>
        /// 搜索未填安监日报的项目列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<UnReportProjectResponseDto>>> SearchUnReportSafeProjectsAsync(UnReportProjectsRequestDto model);

        /// <summary>
        /// 搜索未填船舶日报的船舶列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<UnReportShipResponseDto>>> SearchUnReportShipsAsync(UnReportShipsRequestDto model);
        /// <summary>
        /// 未填报交建通消息通知
        /// </summary>
        /// <param name="type">1是产值日报未填报通知   2是船舶日报  3是安监日报</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> UnReportJjtNotifAsync(int type);

        /// <summary>
        /// 搜索项目月报列表
        /// </summary>
        Task<ResponseAjaxResult<MonthtReportsResponseDto>> SearchMonthReportsAsync(MonthtReportsRequstDto model);
        /// <summary>
        /// 自有船舶月报列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<SearchOwnShipMonthRepResponseDto>> GetSearchOwnShipMonthRepAsync(MonthRepRequestDto requestDto, int import);
        /// <summary>
        /// 分包船舶月报列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SearchSubShipMonthRepResponseDto>>> GetSearchSubShipMonthRepAsync(MonthRepRequestDto requestDto, int import);
        /// <summary>
        /// 自有船舶未报月报列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<NotFillOwnShipSearchResponseDto>>> GetSearchNotFillOwnShipSearchResponseDto(NotFillOwnShipRequestDto requestDto);
        /// <summary>
        /// 自有船舶月报导出
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ExcelImportResponseDto>> OwnShipMonthRepExcel(MonthRepRequestDto requestDto);
        /// <summary>
        /// 分包船舶月报导出
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<ExcelImportResponseDto>> SubShipMonthRepExcel(MonthRepRequestDto requestDto);
        /// <summary>
        ///暂存项目月报
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> StagingMonthReportAsync(StagingMonthReportRequestDto model);

        /// <summary>
        /// 失效项目月报暂存数据
        /// </summary>
        /// <returns></returns>
        Task CeaseEffectStagingMonthReportAsync(Guid projectId, int dateMonth);
        /// <summary>
        /// 获取项目月报未填报列表
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<UnReportProjectRepResponseDto>>> GetSearchNotFillProjectMonthRepAsync(ProjectMonthRepRequestDto projectMonthRepRequestDto);

        /// <summary>
        /// 项目月度产报excel数据导出
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectMonthExcelResponseDto>> ProjectMonthExcelAsync(ProjectMonthReportRequestDto model);

        /// <summary>
        /// 产值产量汇总excel数据导出
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectOutPutValueExcelResponseDto>> SearchProjectOutPutExcelAsync(ProjectOutPutExcelRequestDto putExcelRequestDto);

        /// <summary>
        /// 产值产量汇总excel 数据导出 Npoi
        /// </summary>
        /// <returns></returns>
        Task<byte[]> SearchProjectOutPutNpoiExcelAsync(ProjectOutPutExcelRequestDto putExcelRequestDto);

        /// <summary>
        /// 项目月报导出Word
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<MonthReportProjectWordResponseDto>>> SearchMonthReportProjectWordAsync(MonthtReportsRequstDto model);

        /// <summary>
        /// 修改项目月报特殊字段推送
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> UpdatePushMonthReportSpeailFiledAsync(ModifyPushMonthReportSpecialFieldsRequestDto requestDto);

        /// <summary>
        /// 获取项目带班生产动态
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<ProjectShiftProductionResponseDto>> GetProjectShiftProductionAsync();
	}
}
