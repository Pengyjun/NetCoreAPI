using GHMonitoringCenterApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport
{
	/// <summary>
	///  产值日报excel导出用
	/// </summary>
	public class DayReportExcelResponseDto
	{
		/// <summary>
		/// 日期
		/// </summary>
		public string TimeValue { get; set; }

		/// <summary>
		/// 产值日报集合
		/// </summary>
		public List<DayReportExcel> dayReportExcels { get; set; }
	}

	/// <summary>
	/// 产值日报excel导出用
	/// </summary>
	public class DayReportExcel
	{
		/// <summary>
		/// 项目名称
		/// </summary>
		public string? ProjectName { get; set; }

		/// <summary>
		/// 所属公司
		/// </summary>
		public string? CompanyName { get; set; }

		/// <summary>
		/// 项目类别
		/// </summary>
		public string? ProjectCategory { get; set; }

		/// <summary>
		/// 项目类型
		/// </summary>
		public string? ProjectType { get; set; }

		/// <summary>
		/// 项目状态
		/// </summary>
		public string? ProjectStatus { get; set; }

		/// <summary>
		/// 项目合同额
		/// </summary>
		public decimal? Amount { get; set; }
		/// <summary>
		/// 是否是节假日
		/// </summary>
		public bool IsHoliday { get; set; }

		/// <summary>
		/// 填报人
		/// </summary>
		public string? CreateUser { get; set; }

		/// <summary>
		/// 填报日期(例：20230418)
		/// </summary>
		public int DateDay { get; set; }

		/// <summary>
		/// 日期(时间格式)
		/// </summary>
		public string? DateDayTime
		{
			get
			{
				if (ConvertHelper.TryConvertDateTimeFromDateDay((int)DateDay, out DateTime dayTime))
				{
					return dayTime.ToString("yyyy-MM-dd");
				}
				return null;
			}
		}

		/// <summary>
		///  产值属性（自有：1，分包：2，分包-自有：4）
		/// </summary>
		public string? OutPutType { get; set; }

		/// <summary>
		/// 单价(元)
		/// </summary>
		public decimal? UnitPrice { get; set; }

		/// <summary>
		/// 外包支出(元)
		/// </summary>
		public decimal? OutsourcingExpensesAmount { get; set; }

		/// <summary>
		/// 实际日产量(m³)
		/// </summary>
		public decimal? ActualDailyProduction { get; set; }

		/// <summary>
		/// 实际日产值(元)
		/// </summary>
		public decimal? ActualDailyProductionAmount { get; set; }

		/// <summary>
		/// 施工性质
		/// </summary>
		public string? ConstructionNature { get; set; }

		/// <summary>
		/// 施工层级名称
		/// </summary>
		public string? ProjectWBSName { get; set; }
		/// <summary>
		/// 资源
		/// </summary>
		public string? Resources { get; set; }
		/// <summary>
		/// 陆域9人以上作业地点（处）
		/// </summary>
		public int LandWorkplace { get; set; }

		/// <summary>
		/// 带班领导
		/// </summary>
		public string? ShiftLeader { get; set; }

		/// <summary>
		/// 带班领导电话
		/// </summary>
		public string? ShiftLeaderPhone { get; set; }

		/// <summary>
		/// 陆域3-9人以上作业地点（处）
		/// </summary>
		public int FewLandWorkplace { get; set; }

		/// <summary>
		/// 现场船舶（艘）
		/// </summary>
		public int SiteShipNum { get; set; }

		/// <summary>
		/// 在船人员（人）
		/// </summary>
		public int OnShipPersonNum { get; set; }

		/// <summary>
		///  简述当日危大工程施工内容
		/// </summary>
		public string? HazardousConstructionDescription { get; set; }
	}
}
