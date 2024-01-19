using GHMonitoringCenterApi.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{
    /// <summary>
    /// 年初项目计划明细表
    /// </summary>
    [SugarTable("t_yearprojectplandetail", IsDisabledDelete = true)]
    public class YearProjectPlanDetail : BaseEntity<Guid>
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
        /// 外包支出
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal OutsourcingExpensesAmount { get; set; }

        /// <summary>
        /// 完成工程量(方)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompletedQuantity { get; set; }

        /// <summary>
        /// 完成产值（元）
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(20,8)")]
        public decimal CompleteProductionAmount { get; set; }
    }
}
