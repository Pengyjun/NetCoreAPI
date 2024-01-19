using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
	/// <summary>
	/// 获取币种汇率
	/// </summary>
	public class ProjectCurrencyResponseDto
	{
		/// <summary>
		/// 汇率
		/// </summary>
		public decimal? ExchangeRate { get; set; }
	}
}
