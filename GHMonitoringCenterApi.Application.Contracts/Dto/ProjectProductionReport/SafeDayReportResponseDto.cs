using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ProjectProductionReport
{
	/// <summary>
	/// 安监日报响应类
	/// </summary>
	public class SafeDayReportResponseDto
	{
		/// <summary>
		/// excel导出所需标题字段
		/// </summary>
		public string TimeValue { get; set; }
		/// <summary>
		/// 安监日报信息
		/// </summary>
		public List<SafeDayReportInfo> safeDayReportInfos { get; set; }
	}

	/// <summary>
	/// 安监日报信息
	/// </summary>
	public class SafeDayReportInfo
	{
		/// <summary>
		/// Id
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// 项目id
		/// </summary>
		public Guid ProjectId { get; set; }

		/// <summary>
		/// 安监日报id
		/// </summary>
		public Guid SafeId { get; set; }

		/// <summary>
		/// 项目名称
		/// </summary>
		public string? ProjectName { get; set; }
		/// <summary>
		/// 所属公司
		/// </summary>
		public string? ProjectCompany { get; set; }

		/// <summary>
		/// 项目类别
		/// </summary>
		public string? ProjectCategory { get; set; }

		/// <summary>
		/// 项目类型
		/// </summary>
		public string? ProjectType { get; set; }

		/// <summary>
		/// 项目状态
		/// </summary>
		public string? ProjectStatus { get; set; }

		/// <summary>
		/// 地方政府许可复工日期是否正确
		/// </summary>
		public string? Iswork { get; set; }

		/// <summary>
		/// 是否完成隶属单位复工审批
		/// </summary>
		public string? Iscompanywork { get; set; }

		/// <summary>
		/// 项目复工状态
		/// </summary>
		public string? Workstatus { get; set; }

		/// <summary>
		/// 口罩(个)
		/// </summary>
		public int? Masknum { get; set; }

		/// <summary>
		/// 体温计(个)
		/// </summary>
		public int? Thermometernum { get; set; }

		/// <summary>
		/// 消毒液(升)
		/// </summary>
		public int? Disinfectantnum { get; set; }

		/// <summary>
		/// 存储和消防安全措施（简要描述）
		/// </summary>
		public string? Measures { get; set; }

		/// <summary>
		/// 当日安全生产情况
		/// </summary>
		public string? Situation { get; set; }

		/// <summary>
		/// 是否接收过上级督查
		/// </summary>
		public string Issuperiorsupervision { get; set; }

		/// <summary>
		/// 接收上级督查次数
		/// </summary>
		public int? Superiorsupervisioncount { get; set; }

		/// <summary>
		/// 项目上级督查形式
		/// </summary>
		public string? Superiorsupervisionform { get; set; }

		/// <summary>
		/// 上级督查时间
		/// </summary>
		public DateTime? Superiorsupervisiondate { get; set; }

		/// <summary>
		/// 督查单位
		/// </summary>
		public string? Supervisionunit { get; set; }

		/// <summary>
		/// 上级督查领导
		/// </summary>
		public string? Supervisionleader { get; set; }

		/// <summary>
		/// 上级督查其他人员
		/// </summary>
		public string? Supervisionother { get; set; }

		/// <summary>
		/// 其他须说明的事项
		/// </summary>
		public string? Other { get; set; }

		/// <summary>
		/// 填报日期(例：20230418)
		/// </summary>
		public int? DateDay { get; set; }

		/// <summary>
		/// 日期(时间格式)
		/// </summary>
		public string? DateDayTime
		{
			get
			{
				if (ConvertHelper.TryConvertDateTimeFromDateDay((int)DateDay, out DateTime dayTime))
				{
					 return dayTime.ToString("yyyy-MM-dd"); 
				}
				return null;
			}
		}


	}

}
