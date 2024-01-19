using GHMonitoringCenterApi.Domain.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport
{
	/// <summary>
	/// 产值日报响应类
	/// </summary>
	public class DayReportResponseDto
	{
		/// <summary>
		/// excel导出所需标题字段
		/// </summary>
		public string? TimeValue { get; set; }
		/// <summary>
		/// 产值日报集合
		/// </summary>
		public List<DayReportInfo> dayReportInfos { get; set; }

		/// <summary>
		/// 合计数据
		/// </summary>
		public SumDayValue sumDayValue { get; set; }
	}

	/// <summary>
	/// 合计
	/// </summary>
	public  class SumDayValue
	{
		/// <summary>
		/// 外包支出(元) 合计
		/// </summary>
		public decimal? SumOutsourcingExpensesAmount { get; set; }

		/// <summary>
		/// 实际日产量(m³) 合计
		/// </summary>
		public decimal? SumActualDailyProduction { get; set; }

		/// <summary>
		/// 实际日产值(元)  合计
		/// </summary>
		public decimal? SumActualDailyProductionAmount { get; set; }
	}

	/// <summary>
	/// 产值日报信息
	/// </summary>
	public class DayReportInfo
	{
		/// <summary>
		/// 排序  1 倒序  2 正序
		/// </summary>
		public string[]? Sort { get; set; } = new string[] { "ActualDailyProductionAmount", "desc" };
		/// <summary>
		/// Id
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// 项目Id
		/// </summary>
		public Guid ProjectId { get; set; }
		/// <summary>
		/// 日报Id
		/// </summary>
		public Guid DayId { get; set; }
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
		/// 是否是节假日
		/// </summary>
		public bool IsHoliday { get; set; }

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
		/// 币种汇率
		/// </summary>
        public decimal CurrencyExchangeRate { get; set; }

        /// <summary>
        /// 外包支出(元) 合计
        /// </summary>
        public decimal? ProSumOutsourcingExpensesAmount { get; set; }
		/// <summary>
		/// 单价(元) 合计
		/// </summary>
		public decimal? ProSumUnitPrice { get; set; }

		/// <summary>
		/// 实际日产量(m³) 合计
		/// </summary>
		public decimal? ProSumActualDailyProduction { get; set; }

		/// <summary>
		/// 实际日产值(元) 合计
		/// </summary>
		public decimal? ProSumActualDailyProductionAmount { get; set; }
		/// <summary>
		/// 实际日产值
		/// </summary>
		public decimal? DayActualProductionAmount { get; set; }
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
		public DateTime? DateDayTime
		{
			get
			{
				if (ConvertHelper.TryConvertDateTimeFromDateDay((int)DateDay, out DateTime dayTime))
				{
					return dayTime;
				}
				return null;
			}
		}
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
		/// <summary>
		/// 施工记录集合
		/// </summary>
		public DayResConstruction[] DayReportConstructions { get; set; } = new DayResConstruction[0];
	}


	/// <summary>
	/// 施工记录
	/// </summary>
	public class DayResConstruction
	{
		/// <summary>
		/// 施工记录Id
		/// </summary>
		public Guid? ConstructionId { get; set; }
		/// <summary>
		/// 项目关联id
		/// </summary>
		public Guid? PId { get; set; }
		/// <summary>
		/// 施工分类Id(注：ProjectWBS.Id)
		/// </summary>
		public Guid? ProjectWBSId { get; set; }

		/// <summary>
		///  产值属性（自有：1，分包：2，分包-自有：4）
		/// </summary>
		public ConstructionOutPutType OutPutType { get; set; }

		/// <summary>
		/// 自有船舶Id
		/// </summary>
		public Guid? OwnerShipId { get; set; }

		/// <summary>
		/// 分包船舶Id 或 往来单位Id
		/// </summary>
		public Guid? SubShipId { get; set; }

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
		public int? ConstructionNature { get; set; }


		/// <summary>
		/// 施工层级名称
		/// </summary>
		public string? ProjectWBSName { get; set; }

		/// <summary>
		/// 自有船舶名称 或 分包-自有
		/// </summary>
		public string? OwnerShipName { get; set; }

		/// <summary>
		/// 分包船舶名称 或 往来单位名称
		/// </summary>
		public string? SubShipName { get; set; }

	}

}
