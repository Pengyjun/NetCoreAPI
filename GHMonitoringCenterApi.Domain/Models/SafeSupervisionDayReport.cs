using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHMonitoringCenterApi.Domain.Enums;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 安监日报表
    /// </summary>
    [SugarTable("t_safesupervisiondayreport", IsDisabledDelete = true)]
    [SugarIndex("INDEX_SSDR_UQ_KEY", nameof(ProjectId), OrderByType.Asc, nameof(DateDay), OrderByType.Asc, true)]
    public class SafeSupervisionDayReport : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报日期（例：20230401）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateDay { get; set; }

        /// <summary>
        /// 地方政府许可复工日期是否明确  1 明确 0 不明确
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsWork { get; set; }

        /// <summary>
        /// 地方政府许可复工日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? WorkDate { get; set; }

        /// <summary>
        /// 是否完成隶属单位复工审批  是 1 否 0
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsCompanyWork { get; set; }

        /// <summary>
        /// 项目复工状态（1：春节未复工，2：未开工或停工未复工，3：已复工，4：已完工）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public SafeSupervisionWorkStatus WorkStatus { get; set; }

        /// <summary>
        /// 当天主要施工内容【未停工】
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? ConstructionContent { get; set; }

        /// <summary>
        /// 当天安全生产情况【未停工】
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? ProductionSituation { get; set; }

        /// <summary>
        /// 【未开复工】项目现计划复工日期
        /// </summary>
        public DateTime? PlanWorkDate { get; set; }

        /// <summary>
        /// 未开复工原因
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Reason { get; set; }

        /// <summary>
        /// 项目未开复工原因具体情况
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? Details { get; set; }

        /// <summary>
        /// 【已复工】项目实际复工日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? ActualWorkDate { get; set; }

        /// <summary>
        /// 当天主要施工内容【已复工】
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? ActualConstructionContent { get; set; }

        /// <summary>
        /// 当天安全生产情况【已复工】
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? ActualSituation { get; set; }

        /// <summary>
        /// 是否接受过上级督查  是 1  否 0
        /// </summary>
        [SugarColumn(ColumnDataType = "bit")]
        public bool IsSuperiorSupervision { get; set; }

        /// <summary>
        /// 上级督查次数
        /// </summary>
        [SugarColumn(ColumnDataType = "int", IsNullable = true)]
        public int? SuperiorSupervisionCount { get; set; }

        /// <summary>
        /// 上级督查形式(1:远程督查,2:现场督查)
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public SuperiorSupervisionForm SuperiorSupervisionForm { get; set; }

        /// <summary>
        /// 上级督查时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? SuperiorSupervisionDate { get; set; }

        /// <summary>
        /// 督查单位
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? SupervisionUnit { get; set; }

        /// <summary>
        /// 督查领导
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? SupervisionLeader { get; set; }

        /// <summary>
        /// 上级督查其他人员
        /// </summary>
        [SugarColumn(Length = 200)]
        public string? SupervisionOther { get; set; }

        /// <summary>
        /// 【今日返回】项目管理人员数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ManagementNumber { get; set; }

        /// <summary>
        /// 【今日返回】船舶人员数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int PersonNumber { get; set; }

        /// <summary>
        /// 【今日返回】产业工人数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int WorkerNumber { get; set; }

        /// <summary>
        /// 【今日返回】来自中高风险地区人数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int FromHighRiskAreasPersonNumber { get; set; }

        /// <summary>
        /// 【当前在场】项目管理人员数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int InManagementNumber { get; set; }

        /// <summary>
        /// 【当前在场】船舶人员数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int InPersonNumber { get; set; }

        /// <summary>
        /// 【当前在场】产业工人数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int InWorkerNumber { get; set; }

        /// <summary>
        /// 【当前在场】来自中高风险地区人数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int InHighRiskAreasPersonNumber { get; set; }

        /// <summary>
        /// 【当前隔离】项目管理人员数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int QuarantineManagementNum { get; set; }

        /// <summary>
        /// 【当前隔离】船舶人员数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int QuarantinePersonNum { get; set; }

        /// <summary>
        /// 【当前隔离】产业工人数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int QuarantineWorkerNum { get; set; }

        /// <summary>
        /// 隔离原因
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? QuarantineReason { get; set; }

        /// <summary>
        /// 确诊人数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DiagnosisNum { get; set; }

        /// <summary>
        /// 疑似人数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int SuspectsNum { get; set; }

        /// <summary>
        /// 【应在场】项目管理人员数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ShouldManagementNum { get; set; }

        /// <summary>
        /// 【应在场】船舶人员数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ShouldPersonNum { get; set; }

        /// <summary>
        /// 【应在场】产业工人数量
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ShouldWorkerNum { get; set; }

        /// <summary>
        /// 口罩（个）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int MaskNum { get; set; }

        /// <summary>
        /// 体温计（个）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int ThermometerNum { get; set; }

        /// <summary>
        /// 消毒液（升）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DisinfectantNum { get; set; }

        /// <summary>
        /// 存储和消防安全措施（简要描述）
        /// </summary>
        [SugarColumn(Length = 1000)]
        public string? Measures { get; set; }

        /// <summary>
        /// 当日安全生产情况(1:安全，2：事故)
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public SafeMonitoringSituation Situation { get; set; }

        /// <summary>
        /// 其他须说明的事项
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string? Other { get; set; }
    }
}
