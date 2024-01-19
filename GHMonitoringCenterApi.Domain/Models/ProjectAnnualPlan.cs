using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目年度计划产值
    /// </summary>
    [SugarTable("t_projectannualplan",IsDisabledDelete =true)]
    public class ProjectAnnualPlan:BaseEntity<Guid>
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length =100)]
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        [SugarColumn(Length =50)]
        public string ProjectId { get; set; }
        /// <summary>
        /// 计划产值 按季度导入  第一月
        /// </summary>
        [SugarColumn(ColumnDataType ="decimal(18,2)")]
        public decimal OnePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值   第二月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal TwoPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第三月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal ThreePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第四月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal FourPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第五月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal FivPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第六月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal SixPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第七月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal SevPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第八月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal EigPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第九月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal NinPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal TenPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十一月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal ElePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十二月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal TwePlannedOutputValue { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        [SugarColumn(ColumnDataType ="datetime")]
        public DateTime SubmitYear { get; set; }

        /// <summary>
        /// 计划工程量 按季度导入  第一月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal OnePlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量   第二月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal TwoPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第三月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal ThreePlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第四月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal FourPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第五月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal FivPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第六月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal SixPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第七月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal SevPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第八月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal EigPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第九月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal NinPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第十月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal TenPlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第十一月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal ElePlannedQuantities { get; set; }
        /// <summary>
        /// 计划工程量  第十二月
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal TwePlannedQuantities { get; set; }

    }
}
