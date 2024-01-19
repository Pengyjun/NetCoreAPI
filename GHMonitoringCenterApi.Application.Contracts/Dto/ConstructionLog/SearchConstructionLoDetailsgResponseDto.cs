using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ConstructionLog
{
    /// <summary>
    /// 获取施工日志详情响应Dto
    /// </summary>
    public class SearchConstructionLoDetailsgResponseDto
    {
        /// <summary>
        /// 日报Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 日报名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime? DateTime { get; set; }
        /// <summary>
        /// 填报日期
        /// </summary>
        public int? FillingDateTime { get; set; }
        /// <summary>
        /// 天气
        /// </summary>
        public string? Weather { get; set; }
      
        /// <summary>
        /// 现场管理人员
        /// </summary>
        public int? Management { get; set; }
        /// <summary>
        /// 现场施工人员数量
        /// </summary>
        public int? ConstructionPersonnel { get; set; }
        /// <summary>
        /// 投入施工设备
        /// </summary>
        public string? ConstructionEquipment { get; set; }
        /// <summary>
        /// 施工备注
        /// </summary>
        public string? ConstructionRemarks { get; set; }
        /// <summary>
        /// 其他记录
        /// </summary>
        public string? OtherRecords { get; set; }
        /// <summary>
        /// 班组长
        /// </summary>
        public string? TeamLeader { get; set; }
        /// <summary>
        /// 陆地9人以上作业地点（处）
        /// </summary>
        public int LandWorkplace { get; set; }

        /// <summary>
        /// 带班领导
        /// </summary>
        public string? ShiftLeader { get; set; }

        /// <summary>
        /// 带班领导电话
        /// </summary>
        public string? ShiftLeaderPhone { get; set; }
        /// <summary>
        /// 投入设备
        /// </summary>
        public int? ConstructionDeviceNum { get; set; }
        /// <summary>
        /// 危大工程施工（项）
        /// </summary>
        public int? HazardousConstructionNum { get; set; }

        /// <summary>
        /// 陆域3-9人以上作业地点（处）
        /// </summary>
        public int FewLandWorkplace { get; set; }

        /// <summary>
        /// 现场船舶（艘）
        /// </summary>
        public int SiteShipNum { get; set; }

        /// <summary>
        /// 在船人员（人）
        /// </summary>
        public int OnShipPersonNum { get; set; }

        /// <summary>
        ///  简述当日危大工程施工内容
        /// </summary>
        public string? HazardousConstructionDescription { get; set; }

        /// <summary>
        /// 是否是节假日
        /// </summary>
        public bool IsHoliday { get; set; }

        /// <summary>
        /// 项目日报施工记录
        /// </summary>
        public List<DayReportConstructions> DayReportConstruction { get; set; }

        /// <summary>
        /// 记录人
        /// </summary>
        public string? RecorderName { get; set; }
    }
    /// <summary>
    /// 项目日报施工记录
    /// </summary>
    public  class DayReportConstructions
    {
        /// <summary>
        /// 当日施工主要内容
        /// </summary>
        public string? ConstructionRecord { get; set; }
        /// <summary>
        /// 施工内容
        /// </summary>
        public string? ConstructionContent { get; set; }
        /// <summary>
        /// 实际日产量(m³)
        /// </summary>
        public decimal? ActualDailyProduction { get; set; }

        /// <summary>
        /// 实际日产值(元)
        /// </summary>
        public decimal? ActualDailyProductionAmount { get; set; }
    }
}
