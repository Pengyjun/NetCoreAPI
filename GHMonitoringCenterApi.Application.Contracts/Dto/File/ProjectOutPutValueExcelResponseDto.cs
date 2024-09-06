using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.File
{
	/// <summary>
	/// 产值产量汇总
	/// </summary>
	public class ProjectOutPutValueExcelResponseDto
	{
		//日期
		public string TimeValue { get; set; }
		//汇总
		public List<SumOutPutInfo> sumOutPutInfos { get; set; } = new List<SumOutPutInfo>();
		//各公司统计信息
		public List<OutPutInfo> outPutInfos { get; set; } = new List<OutPutInfo>();
        //重点船舶信息
        public List<KeynoteShipInfo> keynoteShipInfos { get; set; }
    }

    /// <summary>
    /// 各公司统计信息
    /// </summary>
    public class OutPutInfo
    {
        /// <summary>
        /// 所属区域
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 所属项目部
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 当月自行产量
        /// </summary>
        public decimal? OwnProduction { get; set; }
        /// <summary>
        /// 当月分包产量
        /// </summary>
        public decimal? SubProduction { get; set; }
        /// <summary>
        /// 当月产量合计
        /// </summary>
        public decimal? SumProduction { get; set; }
        /// <summary>
        /// 当月自行产值
        /// </summary>
        public decimal? OwnOutPutValue { get; set; }
        /// <summary>
        /// 当月分包产值
        /// </summary>
        public decimal? SubOutPutValue { get; set; }
        /// <summary>
        /// 当月分包差价
        /// </summary>
        public decimal? SubDiffValue { get; set; }
        /// <summary>
        /// 当月分包支出
        /// </summary>
        public decimal? SubExpenditure { get; set; }
        /// <summary>
        /// 当月产值合计
        /// </summary>
        public decimal? SumOutPutValue { get; set; }

        /// <summary>
        /// 当年自行产量
        /// </summary>
        public decimal? YearOwnProduction { get; set; }
        /// <summary>
        /// 当年分包产量
        /// </summary>
        public decimal? YearSubProduction { get; set; }
        /// <summary>
        /// 当年产量合计
        /// </summary>
        public decimal? YearSumProduction { get; set; }
        /// <summary>
        /// 当年自行产值
        /// </summary>
        public decimal? YearOwnOutPutValue { get; set; }
        /// <summary>
        /// 当年分包产值
        /// </summary>
        public decimal? YearSubOutPutValue { get; set; }
        /// <summary>
        /// 当年分包差价
        /// </summary>
        public decimal? YearSubDiffValue { get; set; }
        /// <summary>
        /// 当年分包支出
        /// </summary>
        public decimal? YearSubExpenditure { get; set; }
        /// <summary>
        /// 当年产值合计
        /// </summary>
        public decimal? YearSumOutPutValue { get; set; }
        /// <summary>
        /// 开累产值
        /// </summary>
        public decimal? TotalOutPutValue { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int? CompanySort { get; set; }
    }

    /// <summary>
    /// 汇总
    /// </summary>
    public class SumOutPutInfo
    {
        /// <summary>
        /// 当月自行产量
        /// </summary>
        public decimal? OwnProduction { get; set; }
        /// <summary>
        /// 当月分包产量
        /// </summary>
        public decimal? SubProduction { get; set; }
        /// <summary>
        /// 当月产量合计
        /// </summary>
        public decimal? SumProduction { get; set; }
        /// <summary>
        /// 当月自行产值
        /// </summary>
        public decimal? OwnOutPutValue { get; set; }
        /// <summary>
        /// 当月分包产值
        /// </summary>
        public decimal? SubOutPutValue { get; set; }
        /// <summary>
        /// 当月分包差价
        /// </summary>
        public decimal? SubDiffValue { get; set; }
        /// <summary>
        /// 当月分包支出
        /// </summary>
        public decimal? SubExpenditure { get; set; }
        /// <summary>
        /// 当月产值合计
        /// </summary>
        public decimal? SumOutPutValue { get; set; }

		/// <summary>
		/// 当年自行产量
		/// </summary>
		public decimal? YearOwnProduction { get; set; }
		/// <summary>
		/// 当年分包产量
		/// </summary>
		public decimal? YearSubProduction { get; set; }
		/// <summary>
		/// 当年产量合计
		/// </summary>
		public decimal? YearSumProduction { get; set; }
		/// <summary>
		/// 当年自行产值
		/// </summary>
		public decimal? YearOwnOutPutValue { get; set; }
		/// <summary>
		/// 当年分包产值
		/// </summary>
		public decimal? YearSubOutPutValue { get; set; }
		/// <summary>
		/// 当年分包差价
		/// </summary>
		public decimal? YearSubDiffValue { get; set; }
		/// <summary>
		/// 当年分包支出
		/// </summary>
		public decimal? YearSubExpenditure { get; set; }
		/// <summary>
		/// 当年产值合计
		/// </summary>
		public decimal? YearSumOutPutValue { get; set; }
        /// <summary>
		/// 年累计产值合计
		/// </summary>
        public decimal? TotalSumOutPutValue { get; set; }
    }

    /// <summary>
    /// 项目基础信息
    /// </summary>
    public class baseProjectData
	{
		/// <summary>
		/// 公司id
		/// </summary>
		public Guid? CompanyId { get; set; }
		/// <summary>
		/// 公司名称
		/// </summary>
		public string? CompanyName { get; set; }
		/// <summary>
		/// 项目id
		/// </summary>
		public Guid ProjectId { get; set; }
		/// <summary>
		/// 项目名称
		/// </summary>
		public string? ProjectName { get; set; }
		/// <summary>
		/// 部门名称
		/// </summary>
		public string? DeptName { get; set; }
        /// <summary>
        /// 汇率
        /// </summary>
        public decimal? hv {  get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int? Sort { get; set; }
    }

    /// <summary>
    /// 重点船舶
    /// </summary>
    public class KeynoteShipInfo
    {
        /// <summary>
        /// 重点船舶
        /// </summary>
        public string ShipType { get; set; }
		/// <summary>
		/// 船舶Id
		/// </summary>
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string ShipName { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 产量
        /// </summary>
        public decimal? ProductionValue { get; set; }
        /// <summary>
        /// 产值
        /// </summary>
        public decimal? OutputValue { get; set; }
        /// <summary>
        /// 在场天
        /// </summary>
        public int? OnSiteDays { get; set; }
        /// <summary>
        /// 运转时间
        /// </summary>
        public decimal? WorkingHours { get; set; }
        /// <summary>
        /// 产量*10000除运转时间 暂时不知道列名叫什么
        /// </summary>
        public int? MyPropertyRate1 { get; set; }
        /// <summary>
        /// 产量 年累计
        /// </summary>
        public decimal? YearProductionValue { get; set; }
        /// <summary>
        /// 产值 年累计
        /// </summary>
        public decimal? YearOutputValue { get; set; }
        /// <summary>
        /// 在场天 年累计
        /// </summary>
        public int? YearOnSiteDays { get; set; }
        /// <summary>
        /// 运转时间 年累计
        /// </summary>
        public decimal? YearWorkingHours { get; set; }
        /// <summary>
        /// 产量*10000除运转时间 年累计 暂时不知道列名叫什么
        /// </summary>
        public int? MyPropertyRate2 { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 重点类型排序字段
        /// </summary>
        public int? ShipTypeSort { get; set; }

        /// <summary>
        /// 船舶排序字段
        /// </summary>
        public int? ShipSort { get; set; }

    }
}
