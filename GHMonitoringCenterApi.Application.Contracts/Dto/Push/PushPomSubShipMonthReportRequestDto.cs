using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Push
{
    /// <summary>
    /// 推送到Pom的分包船舶月报
    /// </summary>
    public class PushPomSubShipMonthReportRequestDto
    {
        public string ProjectSubShipMonthRepJson { get; set; }
    }

    /// <summary>
    /// 项目分包船舶月报
    /// </summary>
    public class PomProjectSubShipMonthRepRequestDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 分包船舶id
        /// </summary>
        public string SubShipId { get; set; }
        /// <summary>
        /// 分包船舶名称
        /// </summary>
        public string SubShipName { get; set; }
        /// <summary>
        /// 分包船舶运转
        /// </summary>
        public decimal SubWorkHours { get; set; }
        /// <summary>
        /// 分包船舶施工天数
        /// </summary>
        public decimal SubConstructionDays { get; set; }
        /// <summary>
        /// 分包船舶工程量
        /// </summary>
        public decimal SubQuantities { get; set; }
        /// <summary>
        /// 分包船舶产值
        /// </summary>
        public decimal SubOutputValue { get; set; }
        /// <summary>
        /// 分包船舶当前动态描述 //例如：施工、待命 退场  取下拉name
        /// </summary>
        public string SubDynamicDescription { get; set; }
        /// <summary>
        /// 分包船舶合同清单类型  例如：疏浚 、吹填 、其他 取下拉name
        /// </summary>
        public string SubContractType { get; set; }
        /// <summary>
        /// 下月计划工程量（万方）
        /// </summary>
        public decimal SubPlannedVolumeNextMonth { get; set; }
        /// <summary>
        /// 分包船舶下月计划产值（万元）
        /// </summary>
        public decimal SubPlannedOutputValueNextMonth { get; set; }
        /// <summary>
        /// 填报日期
        /// </summary>
        public DateTime StatementMonth { get; set; }
        /// <summary>
        /// 分包船舶进场日期
        /// </summary>
        public DateTime SubApproachDate { get; set; }
        /// <summary>
        /// 分包船舶退场日期
        /// </summary>
        public DateTime SubExitDate { get; set; }
    }
}
