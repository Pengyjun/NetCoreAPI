using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project.Report
{
    /// <summary>
    /// 项目带班生产动态
    /// </summary>
    public class ProjectShiftProductionResponseDto
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime TimeValue { get; set; }
        public ProjectShiftProductionSumInfo sumInfo { get; set; }
        //已填报项目
        public List<ProjectShiftProductionInfo> projectShiftProductionInfos { get; set; } = new List<ProjectShiftProductionInfo>();
        //未填报项目
        public List<UnProjectShitInfo> unProjectShitInfos { get; set; } = new List<UnProjectShitInfo> { };
    }


    public class ProjectShiftProductionSumInfo
    {
        /// <summary>
        /// 现场管理人员
        /// </summary>
        public int? SumSiteManagementPersonNum { get; set; }
        /// <summary>
        /// 陆域作业人员
        /// </summary>
        public int? SumSiteConstructionPersonNum { get; set; }
        /// <summary>
        /// 路域设备
        /// </summary>
        public int? SumConstructionDeviceNum { get; set; }
        /// <summary>
        /// 陆域3-9人以上作业地点（处）
        /// </summary>
        public int? SumFewLandWorkplace { get; set; }
        /// <summary>
        /// 陆域10人以上作业地点（处）
        /// </summary>
        public int? SumLandWorkplace { get; set; }
        /// <summary>
        /// 在场船舶（艘）
        /// </summary>
        public int? SumSiteShipNum { get; set; }
        /// <summary>
        /// 在船人员（人）
        /// </summary>
        public int? SumOnShipPersonNum { get; set; }
        /// <summary>
        /// 当日危大工程施工（项）
        /// </summary>
        public int SumHazardousConstructionNum { get; set; }
    }
	public class ProjectShiftProductionInfo
    {
		/// <summary>
		/// 日报id
		/// </summary>
		public Guid? DayReportId { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }
		/// <summary>
		/// 项目名称
		/// </summary>
		public string? ProjectName { get; set; }
        /// <summary>
        /// 现场带班领导
        /// </summary>
        public string? ShiftLeader { get; set; }
        /// <summary>
        /// 值班电话
        /// </summary>
        public string? ShiftPhone { get; set; }
        /// <summary>
        /// 现场管理人员
        /// </summary>
        public int? SiteManagementPersonNum { get; set; }
        /// <summary>
        /// 陆域作业人员
        /// </summary>
        public int? SiteConstructionPersonNum { get; set; }
        /// <summary>
        /// 路域设备
        /// </summary>
        public int? ConstructionDeviceNum { get; set; }
        /// <summary>
        /// 陆域3-9人以上作业地点（处）
        /// </summary>
        public int? FewLandWorkplace { get; set; }
        /// <summary>
        /// 陆域10人以上作业地点（处）
        /// </summary>
        public int? LandWorkplace { get; set; }
        /// <summary>
        /// 在场船舶（艘）
        /// </summary>
        public int? SiteShipNum { get; set; }
        /// <summary>
        /// 在船人员（人）
        /// </summary>
        public int? OnShipPersonNum { get; set; }
        /// <summary>
        /// 当日危大工程施工（项）
        /// </summary>
        public int HazardousConstructionNum { get; set; }

        /// <summary>
        ///  简述当日危大工程施工内容
        /// </summary>
        public string? HazardousConstructionDescription { get; set; }
    }

    public class UnProjectShitInfo
    {
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
