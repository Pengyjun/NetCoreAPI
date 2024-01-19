using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Common
{
	/// <summary>
	/// 境内外币种区分
	/// </summary>
	public class SearchCurrencyRequestDto
	{
		/// <summary>
		/// 是否查询全部
		/// </summary>
		public bool IsSearchAll { get; set; }
		/// <summary>
		/// 境内外 0 境内 1 境外
		/// </summary>
		public int Category { get; set; }
	}
}
