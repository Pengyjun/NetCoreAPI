using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Shared
{
	public class ErrorMessage
	{
		/// <summary>
		/// 错误字段
		/// </summary>
		public string Key { get; set; }
		/// <summary>
		/// 错误字段的值
		/// </summary>
		public string Value { get; set; }
	}
}
