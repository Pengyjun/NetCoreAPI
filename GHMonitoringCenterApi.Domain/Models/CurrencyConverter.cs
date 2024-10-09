using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 汇率换算表
    /// </summary>
    [SugarTable("t_currencyconverter",IsDisabledDelete =true)]
	public class CurrencyConverter : BaseEntity<Guid>
	{
		/// <summary>
		/// 年份
		/// </summary>
		[SugarColumn(ColumnDataType ="int")]
		public int Year { get; set; }
		/// <summary>
		/// 汇率
		/// </summary>
		[SugarColumn(ColumnDataType ="decimal(18,4)")]
		public decimal? ExchangeRate { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		[SugarColumn(Length = 36)]
		public string? CurrencyId { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		[SugarColumn(Length =300)]		
		public string? Remark { get; set; }
	}
}
