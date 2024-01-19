using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
	/// <summary>
	/// 重点船舶 23年六月份数据
	/// </summary>
	[SugarTable("t_junekeynote", IsDisabledDelete = true)]
	public class JuneKeyNote : BaseEntity<Guid>
	{
		/// <summary>
		/// 船舶id
		/// </summary>
		[SugarColumn(Length = 36)]
		public Guid ShipId { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        [SugarColumn(Length = 50)]
		public string ShipType { get; set; }
		/// <summary>
		/// 船舶名称
		/// </summary>
		[SugarColumn(Length =50)]
		public string ShipName { get; set; }
		/// <summary>
		/// 项目id
		/// </summary>
		[SugarColumn(Length = 36)]
		public string? ProjectId { get; set; }
		/// <summary>
		/// 项目名称
		/// </summary>
		[SugarColumn(Length =100)]
		public string ProjectName { get; set; }
		/// <summary>
		/// 产量
		/// </summary>
		[SugarColumn(ColumnDataType ="decimal(18,6)")]
		public decimal? ProductionValue { get; set; }
		/// <summary>
		/// 产值
		/// </summary>
		[SugarColumn(ColumnDataType = "decimal(18,6)")]
		public decimal? OutputValue { get; set; }
		/// <summary>
		/// 在场天
		/// </summary>
		[SugarColumn(ColumnDataType = "int")]
		public int? OnSiteDays { get; set; }
		/// <summary>
		/// 运转时间
		/// </summary>
		[SugarColumn(ColumnDataType = "decimal(18,6)")]
		public decimal? WorkingHours { get; set; }
		/// <summary>
		/// 产量*10000除运转时间 暂时不知道列名叫什么
		/// </summary>
		[SugarColumn(ColumnDataType = "int")]
		public int? MyPropertyRate1 { get; set; }
		/// <summary>
		/// 产量 年累计
		/// </summary>
		[SugarColumn(ColumnDataType = "decimal(18,6)")]
		public decimal? YearProductionValue { get; set; }
		/// <summary>
		/// 产值 年累计
		/// </summary>
		[SugarColumn(ColumnDataType = "decimal(18,6)")]
		public decimal? YearOutputValue { get; set; }
		/// <summary>
		/// 在场天 年累计
		/// </summary>
		[SugarColumn(ColumnDataType = "int")]
		public int? YearOnSiteDays { get; set; }
		/// <summary>
		/// 运转时间 年累计
		/// </summary>
		[SugarColumn(ColumnDataType = "decimal(18,6)")]
		public decimal? YearWorkingHours { get; set; }
		/// <summary>
		/// 产量*10000除运转时间 年累计 暂时不知道列名叫什么
		/// </summary>
		[SugarColumn(ColumnDataType = "int")]
		public int? MyPropertyRate2 { get; set; }
	}
}
