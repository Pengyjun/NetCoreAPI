using MiniExcelLibs.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.ShipPlan
{
    /// <summary>
    /// 船舶年度计划响应DTO
    /// </summary>
    public class ShipPlanResponseDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [ExcelIgnore]
        public Guid Id { get; set; }
        /// <summary>
        /// 船舶id
        /// </summary>
        [ExcelIgnore]
        public Guid? ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        [ExcelColumnName("船舶名称")]
        [ExcelColumnWidth(20)]
        public string ShipName { get; set; }
        /// <summary>
        ///船舶状态类型
        /// </summary>
        [ExcelIgnore]
        public int? ShipStatusType { get; set; }
        /// <summary>
        /// 船舶状态名称
        /// </summary>
        [ExcelColumnName("船舶状态名称")]
        [ExcelColumnWidth(20)]
        public string? ShipStatusName { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        [ExcelIgnore]
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [ExcelColumnName("项目名称")]
        [ExcelColumnWidth(20)]
        public string ProjectName { get; set; }

        /// <summary>
        /// 间隔天数
        /// </summary>
        [ExcelColumnName("间隔天数")]
        [ExcelColumnWidth(20)]
        public int Days { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [ExcelColumnName("开始时间")]
        [ExcelColumnWidth(20)]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [ExcelColumnName("结束时间")]
        [ExcelColumnWidth(20)]
        public DateTime EndTime { get; set; }


        /// <summary>
        /// 工程量
        /// </summary>
        [ExcelColumnName("工程量")]
        [ExcelColumnWidth(20)]
        public decimal QuantityWork { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [ExcelColumnName("单价")]
        [ExcelColumnWidth(20)]
        public decimal Price { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [ExcelIgnore]
        public int? Year { get; set; }

        /// <summary>
        /// 计划产值 第一月   
        /// </summary>
        [ExcelColumnName("1月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal OnePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值   第二月
        /// </summary>
        [ExcelColumnName("2月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal TwoPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第三月
        /// </summary>
        [ExcelColumnName("3月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal ThreePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第四月
        /// </summary>
        [ExcelColumnName("4月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal FourPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第五月
        /// </summary>
        [ExcelColumnName("5月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal FivPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第六月
        /// </summary>
        [ExcelColumnName("6月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal SixPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第七月
        /// </summary>
        [ExcelColumnName("7月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal SevPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第八月
        /// </summary>
        [ExcelColumnName("8月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal EigPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第九月
        /// </summary>
        [ExcelColumnName("9月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal NinPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十月
        /// </summary>
        [ExcelColumnName("10月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal TenPlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十一月
        /// </summary>
        [ExcelColumnName("11月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal ElePlannedOutputValue { get; set; }
        /// <summary>
        /// 计划产值  第十二月
        /// </summary>
        [ExcelColumnName("12月计划产值")]
        [ExcelColumnWidth(20)]
        public decimal TwePlannedOutputValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [ExcelColumnName("备注")]
        [ExcelColumnWidth(20)]
        public string? Remark { get; set; }
    }
}
