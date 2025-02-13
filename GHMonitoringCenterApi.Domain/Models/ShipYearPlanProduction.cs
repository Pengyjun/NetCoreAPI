using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 船舶年度计划产值表
    /// </summary>
    [SugarTable("t_shipyearplanproduction", IsDisabledDelete = true)]
    public class ShipYearPlanProduction:BaseEntity<Guid>
    {
       
        /// <summary>
        /// 船舶id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string ShipName { get; set; }
        /// <summary>
        /// 船舶类型
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipTypeId { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string ProjectName { get; set; }

        /// <summary>
        ///船舶状态类型
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? ShipStatusType { get; set; }
        /// <summary>
        /// 船舶状态名称
        /// </summary>
        [SugarColumn(Length = 36)]
        public string? ShipStatusName { get; set; }

        /// <summary>
        /// 间隔天数
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Days { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime EndTime { get; set; }


        /// <summary>
        /// 工程量  单位万m3
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal QuantityWork { get; set; }

        /// <summary>
        /// 单价  单位 元
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? Year { get; set; }

        /// <summary>
        /// 计划产值 第一月    单位万元 
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18,2)")]
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
        /// 备注
        /// </summary>
        [SugarColumn(Length =512)]
        public string? Remark { get; set; }
    }
}
