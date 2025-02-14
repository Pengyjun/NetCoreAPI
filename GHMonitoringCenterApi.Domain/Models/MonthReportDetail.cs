﻿using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 项目月报明细表
    /// </summary>
    [SugarTable("t_monthreportdetail", IsDisabledDelete = true)]
    public class MonthReportDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 月报Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid MonthReportId { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateMonth { get; set; }

        /// <summary>
        /// 填报年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateYear { get; set; }

        /// <summary>
        /// 施工分类Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectWBSId { get; set; }

        /// <summary>
        /// 产值属性（自有：1，分包：2,分包-自有: 4,）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ConstructionOutPutType OutPutType { get; set; }

        /// <summary>
        /// 船舶Id(PomId)
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? ShipName { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 施工性质
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? ConstructionNature { get; set; }

        /// <summary>
        /// 本月外包支出 （元）（原外包支出）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal OutsourcingExpensesAmount { get; set; } = 0M;
        /// <summary>
        /// 本月完成工程量(方)（原产量）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompletedQuantity { get; set; } = 0M;
        /// <summary>
        /// 本月完成产值（元）(原产值)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompleteProductionAmount { get; set; } = 0M;

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 800)]
        public string? Remark { get; set; }
    }
    /// <summary>
    /// 项目月报明细表 (始于202412开累)
    /// </summary>
    [SugarTable("t_monthreportdetailhistory", IsDisabledDelete = true)]
    public class MonthReportDetailHistory : BaseEntity<Guid>
    {
        /// <summary>
        /// 月报Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid MonthReportId { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateMonth { get; set; }

        /// <summary>
        /// 填报年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateYear { get; set; }

        /// <summary>
        /// 施工分类Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectWBSId { get; set; }

        /// <summary>
        /// 产值属性（自有：1，分包：2,分包-自有: 4,）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ConstructionOutPutType OutPutType { get; set; }

        /// <summary>
        /// 船舶Id(PomId)
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? ShipName { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 施工性质
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? ConstructionNature { get; set; }

        /// <summary>
        /// 本月外包支出 （元）（原外包支出）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal OutsourcingExpensesAmount { get; set; } = 0M;
        /// <summary>
        /// 本月完成工程量(方)（原产量）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompletedQuantity { get; set; } = 0M;
        /// <summary>
        /// 本月完成产值（元）(原产值)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompleteProductionAmount { get; set; } = 0M;
        /// <summary>
        /// 本月外包支出(实际本币)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? CurrencyOutsourcingExpensesAmount { get; set; } = 0M;

        /// <summary>
        /// 本月完成工程量(方)(实际)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? ActualCompQuantity { get; set; } = 0M;
        /// <summary>
        /// 本月完成产值(实际本币)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? ActualCompAmount { get; set; } = 0M;

        /// <summary>
        /// 实际人民币产值（用户 输入）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? RMBHValue { get; set; } = 0M;
        /// <summary>
        /// 实际人民币外包支出（用户 输入）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal? RMBHOutValue { get; set; } = 0M;
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 800)]
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 项目月报明细表(2025年前未出现过的月报数据)
    /// </summary>
    [SugarTable("t_monthreportdetailadd", IsDisabledDelete = true)]
    public class MonthReportDetailAdd : BaseEntity<Guid>
    {
        /// <summary>
        /// 月报Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? MonthReportId { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 填报月份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateMonth { get; set; } = 0;

        /// <summary>
        /// 填报年份
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int DateYear { get; set; } = 0;

        /// <summary>
        /// 施工分类Id
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ProjectWBSId { get; set; }

        /// <summary>
        /// 产值属性（自有：1，分包：2,分包-自有: 4,）
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public ConstructionOutPutType OutPutType { get; set; }

        /// <summary>
        /// 船舶Id(PomId)
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid ShipId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        [SugarColumn(Length = 256)]
        public string? ShipName { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 施工性质
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int? ConstructionNature { get; set; }

        /// <summary>
        /// 本月外包支出 （元）（原外包支出）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal OutsourcingExpensesAmount { get; set; } = 0M;
        /// <summary>
        /// 本月完成工程量(方)（原产量）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompletedQuantity { get; set; } = 0M;
        /// <summary>
        /// 本月完成产值（元）(原产值)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompleteProductionAmount { get; set; } = 0M;

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 800)]
        public string? Remark { get; set; }
    }
}
