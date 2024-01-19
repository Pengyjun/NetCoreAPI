using GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionProjectDaily;
using GHMonitoringCenterApi.Application.Contracts.Dto.Project;
using GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport;
using GHMonitoringCenterApi.Domain.Models;
using GHMonitoringCenterApi.Domain.Shared;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.IService.ProjectProductionReport
{
	/// <summary>
	/// 项目生产报表接口层
	/// </summary>
	public interface IProjectProductionReportService
	{
		/// <summary>
		/// 产值日报列表
		/// </summary>
		/// <returns></returns>
		Task<ResponseAjaxResult<DayReportResponseDto>> SearchDayReportAsync(ProductionSafetyRequestDto searchRequestDto);

		/// <summary>
		/// 产值日报列表excel导出用
		/// </summary>
		/// <param name="searchRequestDto"></param>
		/// <returns></returns>
		Task<ResponseAjaxResult<DayReportExcelResponseDto>> SearchDayReportExcelAsync(ProductionSafetyRequestDto searchRequestDto);

		/// <summary>
		/// 产值日报基础查询
		/// </summary>
		/// <param name="searchRequestDto"></param>
		/// <returns></returns>
		Task<ResponseAjaxResult<DayReportResponseDto>> BaseSearchDatReportListAsync(ProductionSafetyRequestDto searchRequestDto,List<Institution> institution);

		/// <summary>
		/// 获取产值日报需要计算的合计值
		/// </summary>
		/// <returns></returns>
		Task<SumDayValue> GetSumDayValueAsync(List<DayReportInfo> dayReports);

		/// <summary>
		/// 获取产值日报处理过的数据
		/// </summary>
		/// <param name="safeDayReports"></param>
		/// <returns></returns>
		Task<List<DayReportInfo>> GetDayResponseDtosAsync(List<DayReportInfo> list, List<Institution> institution);

		/// <summary>
		/// 获取产值日报excel处理过的数据
		/// </summary>
		/// <param name="safeDayReports"></param>
		/// <returns></returns>
		Task<List<DayReportExcel>> GetDayExcelResponseDtosAsync(List<DayReportInfo> list, List<Institution> institution);

		/// <summary>
		/// 船舶日报
		/// </summary>
		/// <returns></returns>
		Task<ResponseAjaxResult<ShipsDayReportResponseDto>> SearchShipDayReportAsync(ShipDailyRequestDto searchRequestDto);

		/// <summary>
		/// 获取船舶日报需要计算的合计值
		/// </summary>
		/// <returns></returns>
		Task<SumShipDayValue> GetSumShipValueAsync(List<ShipsDayReportInfo> dayReports);

		/// <summary>
		/// 获取船舶日报处理过的数据
		/// </summary>
		/// <param name="safeDayReports"></param>
		/// <returns></returns>
		Task<List<ShipsDayReportInfo>> GetShipResponseDtosAsync(List<ShipsDayReportInfo> list,List<Institution> institution);

		/// <summary>
		/// 安监日报
		/// </summary>
		/// <returns></returns>
		Task<ResponseAjaxResult<SafeDayReportResponseDto>> SearchSafeDayReportAsync(ProductionSafetyRequestDto searchRequestDto);

		/// <summary>
		/// 获取安监日报处理过的数据
		/// </summary>
		/// <param name="safeDayReports"></param>
		/// <returns></returns>
		Task<List<SafeDayReportInfo>> GetSafeResponseDtosAsync(List<SafeDayReportInfo> list);
 	}
}
