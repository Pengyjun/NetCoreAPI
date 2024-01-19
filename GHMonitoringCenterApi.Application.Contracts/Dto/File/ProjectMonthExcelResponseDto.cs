using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.File
{
	/// <summary>
	/// 在建项目月报excel导出用
	/// </summary>
	public class ProjectMonthExcelResponseDto
	{
		public DateTime TimeValue { get; set; }
		//项目总体数据
		public List<ProjectOverall> projectOverall { get; set; }
		//项目Wbs数据
		public List<ProjectWbsData> projectWbsDatas { get; set; }
	}

	/// <summary>
	/// 项目总体数据
	/// </summary>
	public class ProjectOverall
	{
		/// <summary>
		/// 项目分类名称
		/// </summary>
		public string ProjectClassificationName { get; set; }
		/// <summary>
		/// 单价
		/// </summary>
		public decimal? UnitPrice { get; set; }
		/// <summary>
		/// 合同工程量
		/// </summary>
		public decimal? ContractQuantity { get; set; }
		/// <summary>
		/// 合同产值
		/// </summary>
		public decimal? ContractAmount { get; set; }
		/// <summary>
		/// 本月完成工程量(方)
		/// </summary>
		public decimal? CompletedQuantity { get; set; }
		/// <summary>
		/// 本月完成产值（人民币/美元/欧元等）
		/// </summary>
		public decimal? CompleteProductionAmount { get; set; }
		/// <summary>
		/// 本月外包支出(元)
		/// </summary>
		public decimal? OutsourcingExpensesAmount { get; set; }
		/// <summary>
		/// 本年完成工程量(方)
		/// </summary>
		public decimal? YearCompletedQuantity { get; set; }
		/// <summary>
		/// 本年完成产值(元)
		/// </summary>
		public decimal? YearCompleteProductionAmount { get; set; }
		/// <summary>
		/// 本年外包支出
		/// </summary>
		public decimal? YearOutsourcingExpensesAmount { get; set; }
		/// <summary>
		/// 累计完成工程量(方)
		/// </summary>
		public decimal? TotalCompletedQuantity { get; set; }
		/// <summary>
		/// 累计完成产值(元)
		/// </summary>
		public decimal? TotalCompleteProductionAmount { get; set; }
		/// <summary>
		/// 累计外包支出(元)
		/// </summary>
		public decimal? TotalOutsourcingExpensesAmount { get; set; }
	}

	/// <summary>
	/// 项目Wbs数据
	/// </summary>
	public class ProjectWbsData
	{
		/// <summary>
		/// 层级
		/// </summary>
		public string Level { get; set; }
		/// <summary>
		/// 项目分类名称
		/// </summary>
		public string ProjectWBSName { get; set; }
		/// <summary>
		/// 产值属性名称
		/// </summary>
		public string OutPutTypeName { get; set; }
		/// <summary>
		/// 施工性质名称
		/// </summary>
		public string ConstructionNatureName { get; set; }
		/// <summary>
		/// 资源
		/// </summary>
		public string ShipName { get; set; }
		/// <summary>
		/// 单价
		/// </summary>
		public decimal? UnitPrice { get; set; }
		/// <summary>
		/// 合同工程量
		/// </summary>
		public decimal? ContractQuantity { get; set; }
		/// <summary>
		/// 合同产值
		/// </summary>
		public decimal? ContractAmount { get; set; }
		/// <summary>
		/// 本月完成工程量(方)
		/// </summary>
		public decimal? CompletedQuantity { get; set; }
		/// <summary>
		/// 本月完成产值（人民币/美元/欧元等）
		/// </summary>
		public decimal? CompleteProductionAmount { get; set; }
		/// <summary>
		/// 本月外包支出(元)
		/// </summary>
		public decimal? OutsourcingExpensesAmount { get; set; }
		/// <summary>
		/// 本年完成工程量(方)
		/// </summary>
		public decimal? YearCompletedQuantity { get; set; }
		/// <summary>
		/// 本年完成产值(元)
		/// </summary>
		public decimal? YearCompleteProductionAmount { get; set; }
		/// <summary>
		/// 本年外包支出
		/// </summary>
		public decimal? YearOutsourcingExpensesAmount { get; set; }
		/// <summary>
		/// 累计完成工程量(方)
		/// </summary>
		public decimal? TotalCompletedQuantity { get; set; }
		/// <summary>
		/// 累计完成产值(元)
		/// </summary>
		public decimal? TotalCompleteProductionAmount { get; set; }
		/// <summary>
		/// 累计外包支出(元)
		/// </summary>
		public decimal? TotalOutsourcingExpensesAmount { get; set; }
	}
}
