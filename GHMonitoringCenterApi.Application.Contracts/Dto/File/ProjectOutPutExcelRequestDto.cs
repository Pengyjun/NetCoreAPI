using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.File
{
	/// <summary>
	/// 产值产量汇总请求dto
	/// </summary>
	public class ProjectOutPutExcelRequestDto
	{
		/// <summary>
		/// 查询日期
		/// </summary>
		public DateTime SearchTime { get; set; }

    }
}
