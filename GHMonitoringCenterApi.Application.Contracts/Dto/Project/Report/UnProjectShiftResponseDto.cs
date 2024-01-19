using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
	/// <summary>
	/// 在建项目带班生产动态未报项目
	/// </summary>
	public class UnProjectShiftResponseDto
	{
		/// <summary>
		/// 日报id
		/// </summary>
        public Guid? DayReportId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }
		/// <summary>
		/// 项目名称
		/// </summary>
        public string? ProjectName { get; set; }
    }
}
